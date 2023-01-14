using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Ashbay{
    [Serializable]
    public class CommandLineEvent : UnityEvent<GTerminal, string> {}

    /**
    * DO NOT MODIFY THIS FILE
    */
    public class GTerminal : MonoBehaviour{

        // Attributes available in unity inspector
        [SerializeField] Canvas Canvas;
        [SerializeField] Color TextColor;
        [SerializeField] Color BackgroundColor;
        public CommandLineEvent onCommandLineEvent;

        // UI Element
        protected TMP_InputField inputFieldTMP;
        protected TextMeshProUGUI textGUI;
        protected TextMeshProUGUI placeholderGUI;

        // Vars
        protected Prompt Prompt = new Prompt();
        protected Boolean hasFocus = false;
        protected Boolean hasEnterCommandLine = false;
        protected Boolean hasToRecoverFromPreservedText = false;
        protected String preservedText = null;
        protected QueuedTerminalInputSystem queuedInputSystem = new QueuedTerminalInputSystem();
        public float textDisplayRate = 0.025f;

        // Coroutine tick
        private IEnumerator tickCoroutine = null;
        private Boolean isTickCoroutineRunning = false;

        private static ILogger logger = Debug.unityLogger;

        void Start(){
            GameObject inputFieldGameObject = this.transform.GetChild(0).gameObject;
            this.inputFieldTMP = inputFieldGameObject.GetComponent<TMP_InputField>();

            // Set InputField size match canvas size (means full screen)
            if(this.Canvas != null){
                RectTransform canvasTransform = this.Canvas.GetComponent<RectTransform>();
                RectTransform inputFieldTransform = inputFieldGameObject.GetComponent<RectTransform>();
                inputFieldTransform.sizeDelta = canvasTransform.sizeDelta;
            } else {
                logger.Log(this.GetType().Name, "The terminal is not fully set up. Please provide a canvas in to the terminal object in Unity Editor.");
            }

            // Set background and text colors
            inputFieldGameObject.GetComponent<Image>().color = this.BackgroundColor;
            foreach(TextMeshProUGUI textUI in inputFieldTMP.GetComponentsInChildren<TextMeshProUGUI>()){
                textUI.color = this.TextColor;

                // Use the current loop to identify TMP text and TMP placeholder
                if(textUI.name == "Text") this.textGUI = textUI;
                else if(textUI.name == "Placeholder") this.placeholderGUI = textUI;
            }

            // Set prompt text
            this.inputFieldTMP.text = this.Prompt.ToString();

            // Add input validation only to check if Enter key is typed on keyboard
            this.inputFieldTMP.onValidateInput += CheckInputValidation;
            
            // Loop every 1 second to display animated text
            //InvokeRepeating("Tick", 0f, this.textDisplayRate);
            this.tickCoroutine = Tick();
        }

        void FixedUpdate(){
            if(this.hasToRecoverFromPreservedText){
                this.recoverFromPreservedText();
                this.forceCaretPosition(GCaret.POSITION.END);
            }
        }

        IEnumerator Tick(){
            while(true){
                String characterToDisplay = queuedInputSystem.Next();

                if(characterToDisplay != null){
                    this.Append(characterToDisplay);
                }else{
                    StopCoroutine(this.tickCoroutine);
                    this.isTickCoroutineRunning = false;
                    this.inputFieldTMP.readOnly = false;
                }
                yield return new WaitForSeconds(textDisplayRate);
            }
        }

        /**
        * Execute each time text change. At the moment, this function check if there is a deletion in the text and allow deletion ONLY if it happens after prompt text
        * @param insertedString
        */
        public void onTextChange(String insertedString){
            // Prevent from deleting characters located before the last prompt display
            this.hasToRecoverFromPreservedText = this.hasFocus && (this.inputFieldTMP.stringPosition+1 <= this.preservedText.LastIndexOf(this.Prompt.ToString()) + this.Prompt.Length);
            if(!hasToRecoverFromPreservedText){
                this.preservedText = this.inputFieldTMP.text;
            }
        }

        /**
        * Executed when a selection event happen. Set focus to true
        * @param insertedString
        */
        public void onSelectChange(String insertedString){
            this.hasFocus = true;
        }

        /**
        * Executed when a deselection event happen. Set focus to false
        * @param insertedString
        */
        public void onDeselectChange(String insertedString){
            this.hasFocus = false;
        }

        /**
        * Append characters to the end of terminal
        * @param appendValue
        */
        public void Append(String appendValue){
            this.inputFieldTMP.text += appendValue;
            this.forceCaretPosition(GCaret.POSITION.END);
        }

        /**
        * Append characters to the terminal at a new line
        * @param appendValue
        * @param queued
        */
        public void AppendLine(String appendValue, Boolean allCharAtOnce = false){
            this.queuedInputSystem.Enqueue('\n'+appendValue, allCharAtOnce);

            if(!this.isTickCoroutineRunning){
                this.isTickCoroutineRunning = true;
                this.inputFieldTMP.readOnly = true;
                StartCoroutine(this.tickCoroutine);
            }
        }

        /**
        * Check which character is typed, if it's "Enter" key then go to new line and add prompt text and set caret at the end of the text
        * @param text
        * @param charIndex
        * @param addedChar
        * @return The character type by the user OR a spacement in case of a newline
        */
        private char CheckInputValidation(string text, int charIndex, char addedChar){
            if(addedChar == '\n'){
                int lastIndexOfNewline = this.inputFieldTMP.text.LastIndexOf('\n') >=0 ? this.inputFieldTMP.text.LastIndexOf('\n')+1 : 0;
                String command = this.inputFieldTMP.text.Substring(lastIndexOfNewline).Trim().Replace(this.Prompt.ToString().Trim(), "");
                // Since we type a new line, then emit an event that tells a new command line has been entered
                if(this.onCommandLineEvent != null) onCommandLineEvent.Invoke(this, command.Trim());

                // Add prompt
                this.AppendLine(this.Prompt.ToString(), true);
                return ' ';
            }

            return addedChar;
        }

        /**
        * Force caret at a given position.
        * @param position Position to force caret (END or AFTER_PROMPT)
        * @return void
        */
        private void forceCaretPosition(GCaret.POSITION position){
            switch(position){
                case GCaret.POSITION.AFTER_PROMPT:
                    this.inputFieldTMP.stringPosition = this.inputFieldTMP.text.LastIndexOf(this.Prompt.ToString()) + this.Prompt.Length;
                    break;
                case GCaret.POSITION.END:
                    this.inputFieldTMP.stringPosition = this.inputFieldTMP.text.Length;
                    break;
            }
        }

        /**
        * Show previous terminal text ( = text before modification)
        */
        private void recoverFromPreservedText(){
            this.inputFieldTMP.text = this.preservedText;
            this.hasToRecoverFromPreservedText = false;
        }
    }
}