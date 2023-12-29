using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class PlayerCameraManager : MonoBehaviour
    {
        PlayerManager player; // Reference the Player Manager script
        public static PlayerCameraManager singleton; // Singleton gameobject for Player Camera Manager
        public Transform cameraObject; // Transform component for the camera object
        public Transform targetTransform; // Camera target position which will be the player
        public Transform cameraTransform; // Access to camera transform component
        public Transform cameraPivotTransform; // Access to camera pivot transform component
        private Vector3 cameraTransformPosition; // Position of the camera transform
        public LayerMask ignoreLayers; // Layer to ignore
        public LayerMask environmentLayer; // Layer for environment, this is used in the script to block LockOn if env objects are there
        private Vector3 cameraFollowVelocity = Vector3.zero; // The velocity for the Camera Follow(for proper camera collision)
        
        public float lookSpeed = 0.1f; // Camera looking speed
        public float groundedFollowSpeed = 20f; // Camera follow speed when on ground
        public float aerialFollowSpeed = 1f; //  Camera follow speed when in air
        public float pivotSpeed = 0.03f; // Camera Pivot center

        private float targetPosition; // Camera Target for collision
        private float defaultPosition; // Default position of the Camera
        private float lookAngle; // Camera Looking Angle as easy as that
        private float pivotAngle; // Pivot Camera Angle

        public float minimumPivot = -35; // Minimum pivot for the camera pivot
        public float maximumPivot = 35; // Maximum pivot for the camera pivot
        public float cameraSphereRadius = 0.2f; // For the rotation around the camera and it's limit
        public float cameraCollisionOffset = 0.2f; // Default offset for camera Collision
        public float mimumumCollisionOffset = 0.2f; // Minimum offset for camera Collision
        public float lockedPivotPosition = 2.25f; // Locked pivot position, for locking the position
        public float unlockedPivotPosition = 1.65f; // unlocked pivot position, for unlocking the position
        
        [Header("LOCK ON DATA")]
        public CharacterManager currentLockOnTarget; // The target which is currently locked on
        List<CharacterManager> availableTargets = new List<CharacterManager>(); // List of all the available targets
        public CharacterManager nearestLockOnTarget; // Transform for the nearest Lock on Target for the player
        public CharacterManager leftLockOnTarget; // Transform for the left nearest target for the player
        public CharacterManager rightLockOnTarget;// Transform for the right nearest target for the player
        public float maximumLockOnDistance = 30; //Maxmimum Lock on Distance between player
        public void Awake()
        {
            singleton = this;
            cameraObject = Camera.main.transform;
            cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform.GetComponent<Transform>();
            defaultPosition = cameraTransform.localPosition.z;
            targetTransform = FindObjectOfType<PlayerManager>().transform; //Better use this!! 
            player = FindObjectOfType<PlayerManager>();
        }
        private void Start()
        {
            environmentLayer = LayerMask.NameToLayer("Environment");
        }
        public void FollowTarget(float delta)
        {
            if(player.isGrounded)
            {
                Vector3 targetPosition = Vector3.SmoothDamp
                    (transform.position, targetTransform.position, ref cameraFollowVelocity, groundedFollowSpeed * Time.deltaTime);
                transform.position = targetPosition;
            }
            else
            {
                Vector3 targetPosition = Vector3.SmoothDamp
                    (transform.position, targetTransform.position, ref cameraFollowVelocity, aerialFollowSpeed * Time.deltaTime);
                transform.position = targetPosition;       
            }

            UseCameraCollisions(delta);
        }
        public void UseCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if(player.playerInputManager.lockOnFlag == false && currentLockOnTarget == null)
            {
                lookAngle += mouseXInput * lookSpeed * delta;
                pivotAngle -= mouseYInput * pivotSpeed * delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                transform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                //float velocity = 0;

                Vector3 dir = currentLockOnTarget.lockOnTransform.transform.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.lockOnTransform.transform.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles= eulerAngle;
            }
        }
        private void UseCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit rayHit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast
                (cameraPivotTransform.position, cameraSphereRadius, direction, out rayHit, Mathf.Abs(targetPosition),
                    ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, rayHit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < mimumumCollisionOffset)
            {
                targetPosition = -mimumumCollisionOffset;
            }
            
            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }
        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if(character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if(character.transform.root != targetTransform.transform.root 
                        && viewableAngle > -50  && viewableAngle < 50 
                        && distanceFromTarget <= maximumLockOnDistance)
                    {
                        if (Physics.Linecast(player.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(player.lockOnTransform.position, character.lockOnTransform.position);

                            if(hit.transform.gameObject.layer == environmentLayer)
                            {
                                // Cannot lock onto target, objects in the way
                                player.playerInputManager.lockOnFlag = false;
                                Debug.Log("Cannot lock on targets");
                            }
                            else
                            {
                                availableTargets.Add(character);
                            }
                        }
                    }
                }
            }
            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position,
                 availableTargets[k].transform.position);

                if(distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }
                if(player.playerInputManager.lockOnFlag && currentLockOnTarget)
                {
                    Vector3 relativeEnemyPosition = player.playerInputManager.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget 
                        && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                       leftLockOnTarget = availableTargets[k]; //Enable for Quick lock on Target on Left
                    }

                    else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget
                        && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockOnTarget = availableTargets[k]; //Enable for Quick lock on Target on Right
                    }
                }
            }
        }
        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            currentLockOnTarget = null;
            nearestLockOnTarget = null;
        }
        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if(currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }
}