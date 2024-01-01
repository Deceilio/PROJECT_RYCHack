using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Deceilio.Psychain
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
        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>(); 
            eventManager = FindObjectOfType<WRLD_EVENT_MANAGER>();
        }

        void Start()
        {
            sentences = new Queue<string>();
            currentDialogueIndex = 0;
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

        void EndDialogues()
        {
            dialogueAnimator.SetBool("isOpen", false);
        }

        private void PlayerHandWave()
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Hand Waving", true);
        }

        private void MarcusEventFinished()
        {
            Debug.Log("Marcus event finished");
            eventManager.marcusEventFinished = true;
        }

    }
}
