using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Adnan.RYCHack
{
    public class WRLD_DIALOGUE_MANAGER : MonoBehaviour
    {
        [HideInInspector] public WRLD_EVENT_MANAGER eventManager;
        [HideInInspector] public PlayerManager player;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        public Queue<string> sentences;
        public List<WRLD_DIALOGUE> dialogues;
        private int currentDialogueIndex = 0;

        public Animator dialogueAnimator;
        public AudioSource audioSource;
        public Button continueButton;

        [Header("CHOICES")]
        public GameObject choiceBox;
        public Button choiceAButton;
        public Button choiceBButton;
        public TextMeshProUGUI choiceAText;
        public TextMeshProUGUI choiceBText;
        public bool isChoiceAButtonPressed = false;
        public bool isChoiceBButtonPressed = false;
        public bool isInMarcusFirstChoice = false;
        public bool isInMarcusSecondChoice = false;
        public bool isInMarcusThirdChoice = false;
        public WRLD_DIALOGUE_TRIGGER marcusDialogueTrigger2;

        public Slider healthSlider;
        public float decreaseSpeed = 0.1f;
        public float depleteSpeed = 0.3f;

        public GameObject gameOver;
        public GameObject winnerOver;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>(); 
            eventManager = FindObjectOfType<WRLD_EVENT_MANAGER>();
        }

        void Start()
        {
            choiceAButton.onClick.AddListener(ChoiceAButtonPressed);
            choiceBButton.onClick.AddListener(ChoiceBButtonPressed);
            sentences = new Queue<string>();
            currentDialogueIndex = 0;
        }
        private void Update()
        {
            if(dialogueAnimator.GetBool("isOpen"))
            {
                player.canMove = false;
            }

            if(isInMarcusFirstChoice)
            {
                if (isChoiceAButtonPressed)
                {
                    isChoiceAButtonPressed = false;
                    isChoiceBButtonPressed = false;
                    continueButton.interactable = true;
                    choiceBox.SetActive(false);
                    isInMarcusFirstChoice = false;
                    DisplayNextSentence();
                }
                else if (isChoiceBButtonPressed)
                {
                    isChoiceAButtonPressed = false;
                    isChoiceBButtonPressed = false;
                    choiceBox.SetActive(false);
                    continueButton.interactable = true;
                    isInMarcusFirstChoice = false;
                    EndDialogues();
                }
            }
            else if (isInMarcusSecondChoice)
            {
                if (isChoiceAButtonPressed)
                {
                    isChoiceAButtonPressed = false;
                    isChoiceBButtonPressed = false;
                    continueButton.interactable = true;
                    choiceBox.SetActive(false);
                    isInMarcusSecondChoice = false;
                    DisplayNextSentence();
                }
                else if (isChoiceBButtonPressed)
                {
                    isChoiceAButtonPressed = false;
                    isChoiceBButtonPressed = false;
                    choiceBox.SetActive(false);
                    continueButton.interactable = true;
                    isInMarcusSecondChoice = false;
                    marcusDialogueTrigger2.TriggerDialogues();
                }
            }
            else if (isInMarcusThirdChoice)
            {
                if (isChoiceAButtonPressed)
                {
                    isChoiceAButtonPressed = false;
                    isChoiceBButtonPressed = false;
                    choiceBox.SetActive(false);
                    EndDialogues();
                    isInMarcusThirdChoice = false;
                    StartCoroutine(DepleteHealthOverTime());
                }
                else if (isChoiceBButtonPressed)
                {
                    isChoiceAButtonPressed = false;
                    isChoiceBButtonPressed = false;
                    choiceBox.SetActive(false);
                    continueButton.interactable = true;
                    isInMarcusThirdChoice = false;
                    DisplayNextSentence();
                }
            }

            if(healthSlider.value == 0f)
            {
                gameOver.SetActive(true);
            }
        }
        private Action GetFunctionByName(string functionName)
        {
            if (string.IsNullOrEmpty(functionName))
                return null;

            System.Type type = this.GetType();
            System.Reflection.MethodInfo method = type.GetMethod(functionName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            if (method != null)
            {
                return (Action)Delegate.CreateDelegate(typeof(Action), this, method);
            }
            else
            {
                Debug.LogWarning("Function not found: " + functionName);
                return null;
            }
        }

        public void StartDialogues(List<WRLD_DIALOGUE> dialoguesList)
        {
            if (dialoguesList.Count == 0)
            {
                Debug.LogWarning("No dialogues provided.");
                return;
            }

            dialogues = new List<WRLD_DIALOGUE>(dialoguesList);
            currentDialogueIndex = 0;

            foreach (var dialogue in dialogues)
            {
                dialogue.functions = new Action[dialogue.functionNames.Length];
                for (int i = 0; i < dialogue.functionNames.Length; i++)
                {
                    dialogue.functions[i] = GetFunctionByName(dialogue.functionNames[i]);
                }
            }

            StartDialogue(dialogues[currentDialogueIndex]);
        }

        public void StartDialogue(WRLD_DIALOGUE dialogue)
        {
            dialogueAnimator.SetBool("isOpen", true);
            nameText.text = dialogue.name;

            sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                if (currentDialogueIndex < dialogues.Count - 1)
                {
                    currentDialogueIndex++;
                    StartDialogue(dialogues[currentDialogueIndex]);
                }
                else
                {
                    EndDialogues();
                    return;
                }
            }

            string sentence = sentences.Dequeue();
            AudioClip audioClip = dialogues[currentDialogueIndex].voiceLines[sentences.Count];
            Action function = dialogues[currentDialogueIndex].functions[sentences.Count];
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence, audioClip, function));
        }

        IEnumerator TypeSentence(string sentence, AudioClip audioClip, Action function)
        {
            dialogueText.text = "";
            // Execute the function
            if (function != null)
            {
                function.Invoke();
            }
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
            if (audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        private void EndDialogues()
        {
            dialogueAnimator.SetBool("isOpen", false);
            audioSource.Stop();
            player.canMove = true;
            Invoke("ResetPlayerAnim", 0.2f);
        }

        private void PlayerHandWave()
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Hand Waving", true);
        }

        private void PlayerSitting()
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Sitting NPC", true);
        }

        private void MarcusEventFinished()
        {
            Debug.Log("Marcus event finished");
            eventManager.marcusEventFinished = true;
        }

        private void ResetPlayerAnim()
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Empty", true);
        }
        private void MarcusStartingFacingPlayer()
        {
            eventManager.marcusAnimator.SetBool("faceUp", true);
        }
        private void MarcusStartDrinking()
        {
            StartCoroutine(DecreaseHealthOverTime());
            eventManager.marcusAnimator.SetBool("drinking", true);
        }
        IEnumerator DecreaseHealthOverTime()
        {
            while (healthSlider.value > 0.5f)
            {
                healthSlider.value -= decreaseSpeed * Time.deltaTime;
                yield return null;
            }

            healthSlider.value = 0.5f;
        }

        IEnumerator DepleteHealthOverTime()
        {
            while (healthSlider.value > 0)
            {
                healthSlider.value -= depleteSpeed * Time.deltaTime;
                yield return null;
            }

            // Ensure the final value is exactly 0
            healthSlider.value = 0f;
        }
        private void MarcusEventFirstChoice()
        {
            isInMarcusFirstChoice = true;
            continueButton.interactable = false;
            choiceAText.text = "Ask about Marcus's well-being";
            choiceBText.text = "Keep walking and mind your own business.";
            choiceBox.SetActive(true);
        }
        private void MarcusEventSecondChoice()
        {
            isInMarcusSecondChoice = true;
            continueButton.interactable = false;
            choiceAText.text = "Introduce yourself";
            choiceBText.text = "Stay silent and observe.";
            choiceBox.SetActive(true);
        }
        private void MarcusEventThirdChoice()
        {
            isInMarcusThirdChoice = true;
            continueButton.interactable = false;
            choiceAText.text = "Continue the conversation";
            choiceBText.text = "Interrupt his drinking.";
            choiceBox.SetActive(true);
        }
        private void ChoiceAButtonPressed()
        {
            isChoiceAButtonPressed = true;
        }
        private void ChoiceBButtonPressed()
        {
            isChoiceBButtonPressed = true;
        }
        private void WinTheDemo()
        {
            winnerOver.SetActive(true);
        }
    }
}
