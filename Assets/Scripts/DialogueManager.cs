using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        #region Singleton
        public static DialogueManager instance;

        void Awake()
        {
            if (instance != null)
                Destroy(gameObject);

            instance = this;
            DontDestroyOnLoad(this);
        }
        #endregion

        public GameObject dialoguePanel;
        public Button continueButton;
        public TextMeshProUGUI speakerName;
        public TextMeshProUGUI dialogueText;
        private Queue<string> sentences;

        public bool dialogueIsActive = false;

        Animator continueButtonAnimator;

        [Header("Settings")]
        [SerializeField] float defaultTypeSpeed = 3f;

        bool clearNextLine = false;
        float typeSpeedDelay = 1f;

        void Start()
        {
            sentences = new Queue<string>();
            speakerName.text = "";
            dialogueText.text = "";
            dialoguePanel.SetActive(false);
            dialogueIsActive = false;
            continueButtonAnimator = continueButton.GetComponent<Animator>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            if (speakerName != null)
                speakerName.text = dialogue.speakerName;
            dialogueText.text = "";
            dialoguePanel.SetActive(true);
            dialogueIsActive = true;
            continueButtonAnimator.SetBool("IsReady", false);
            continueButton.enabled = false;

            sentences.Clear();
            foreach (string sentence in dialogue.sentences)
            {
                string localizedText = sentence;
                sentences.Enqueue(localizedText);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            continueButtonAnimator.SetBool("IsReady", false);
            continueButton.enabled = false;
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();

            if (CheckTextForKey(sentence, DialogueKeys.typeDelay))
            {
                sentence = ParseTypingSpeed(sentence);
            }
            else
            {
                typeSpeedDelay = defaultTypeSpeed;
            }

            StartCoroutine(TypeText(sentence));
        }

        public void EndDialogue()
        {
            dialoguePanel.SetActive(false);
            dialogueIsActive = false;
        }

        IEnumerator TypeText(string sentence)
        {
            // Check if we want to use [CLS]
            if (clearNextLine)
            {
                dialogueText.text = "";
                clearNextLine = false;
            }

            if (CheckTextForKey(sentence, DialogueKeys.cls))
            {
                clearNextLine = true;
                sentence = RemoveKeyFromString(sentence, DialogueKeys.cls);
            }

            // Actual Typing is done here
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(Mathf.Pow(typeSpeedDelay, -1));

            }

            yield return new WaitForEndOfFrame();
            continueButtonAnimator.SetBool("IsReady", true);
            continueButton.enabled = true;
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        }

        bool CheckTextForKey(string text, string key)
        {
            if (text.Contains(key))
            {
                return true;
            }

            return false;
        }

        string ParseTypingSpeed(string text)
        {
            string output = text;
            string[] words = text.Split(new[] { DialogueKeys.typeDelay }, StringSplitOptions.None);

            var regex = new Regex(@"\d+");
            string numericText = regex.Match(words[1]).Value;

            Debug.Log(numericText);
            output = RemoveKeyFromString(output, numericText);
            output = RemoveKeyFromString(output, DialogueKeys.typeDelay);
            float.TryParse(numericText, out typeSpeedDelay);


            return output;
        }

        string RemoveKeyFromString(string text, string key)
        {
            string output = text.Remove(text.IndexOf(key), key.Length);
            return output;
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        public string speakerName;
        [TextArea] public List<string> sentences;
    }

    /// <summary>
    /// A collection of the unique keys for the dialogue system
    /// The system parses these "keys" and changes what appears in the dialogue box
    /// These keys will not be visible in the dialogue box
    /// </summary>
    public static class DialogueKeys
    {
        public static string cls = "[CLS]"; // "Clear Screen" - Tells the dialogue screen to clear the text field after hitting continue
        public static string typeDelay = "/tspd=";
    }
}