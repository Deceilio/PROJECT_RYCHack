using UnityEngine;

namespace Deceilio.Psychain
{
    public class WRLD_ROTATE_GEARS : MonoBehaviour
    {
        [Tooltip("This is the object that the script's game object will rotate around")]
        public Transform target; // The object to rotate around
        public float xAngle, yAngle, zAngle; // The angle to rotate
        void Start()
        {
            if (target == null)
            {
                target = this.gameObject.transform;
                //Debug.Log("RotateAround target not specified. Defaulting to this GameObject");
            }
        }
        void Update()
        {

            target.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
        }
    }
}