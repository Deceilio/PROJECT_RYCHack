using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain
{
    public class ElevatorInteractableManager : WRLD_INTERACTABLE
    {
        [Header("INTERACTABLE HITBOX")]
        [SerializeField] Collider interactableHitbox; // Reference to the Elevator collider

        [Header("DESTINATION")]
        [SerializeField] Vector3 destinationHigh; // Highest point which elevator will travel
        [SerializeField] Vector3 destinationLow; // Lowest point which elevator will travel
        [SerializeField] bool isTravelling = false; // Checks if the elevator is currently travelling or not
        [SerializeField] bool buttonIsReleased = true; // Checks if the button for the elevator is released or not
        
        [Header("ANIMATOR")]
        [SerializeField] Animator elevatorAnimator; // Animator component for the elevator
        [SerializeField] string buttonPressedAnimation = "Elevator_Button_Press_01"; // Animation name for the pressing button on the elevator
        [SerializeField] List<CharacterManager> charactersOnButton; // Characters which are on top of the elevator button
        
        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if(character != null)
            {
                AddCharacterToListOfCharactersStandingOnButton(character);

                if(!isTravelling && buttonIsReleased)
                {
                    ActivateElevator();
                }
            }
        } 

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if(character != null)
            {
                StartCoroutine(ReleaseButton(character));
            }
        }

        private bool IsCloseEnough(Vector3 position, Vector3 target, float tolerance = 0.01f)
        {
            return Vector3.Distance(position, target) <= tolerance;
        }

        private void ActivateElevator()
        {
            elevatorAnimator.SetBool("isPressed", true);
            buttonIsReleased = false;
            elevatorAnimator.Play(buttonPressedAnimation);

            // BELOW CODE: Inside your ActivateElevator function
            if (IsCloseEnough(transform.position, destinationHigh))
            {
                StartCoroutine(MoveElevator(destinationLow, 1));
            }
            else if (IsCloseEnough(transform.position, destinationLow))
            {
                StartCoroutine(MoveElevator(destinationHigh, 1));
            }
        }

        private IEnumerator MoveElevator(Vector3 finalPosition, float duration)
        {
            isTravelling = true;

            if(duration > 0)
            {
                float startTime = Time.time;
                float endTime = startTime + duration;
                yield return null;

                while(Time.time < endTime)
                {
                    transform.position = Vector3.Lerp(transform.position, finalPosition, duration * Time.deltaTime);
                    Vector3 movementVelocity = Vector3.Lerp(transform.position, finalPosition, duration * Time.deltaTime);
                    Vector3 characterMovementVelocity = new Vector3(0, movementVelocity.y, 0);

                    foreach(var character in charactersOnButton)
                    {
                        character.characterController.Move(characterMovementVelocity * Time.deltaTime);
                    }

                    yield return null;
                }

                transform.position = finalPosition;
                isTravelling = false;
            }
        }

        private IEnumerator ReleaseButton(CharacterManager character)
        {
            while(isTravelling)
                yield return null;

            yield return new WaitForSeconds(2);

            RemoveCharacterToListOfCharactersStandingOnButton(character);

            if(charactersOnButton.Count == 0)
            {
                elevatorAnimator.SetBool("isPressed", false);
                buttonIsReleased = true;
            }
        }

        private void AddCharacterToListOfCharactersStandingOnButton(CharacterManager character)
        {
            if(charactersOnButton.Contains(character))
                return;

            charactersOnButton.Add(character);
        }

        private void RemoveCharacterToListOfCharactersStandingOnButton(CharacterManager character)
        {
            charactersOnButton.Remove(character);

            // BELOW CODE: Removes null entries
            for(int i = charactersOnButton.Count - 1; i > -1; i--)
            {
                if(charactersOnButton[i] == null)
                {
                    charactersOnButton.RemoveAt(i);
                }
            }
        }
    }
}