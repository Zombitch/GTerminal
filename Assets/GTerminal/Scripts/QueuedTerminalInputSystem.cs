using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Ashbay{
    [Serializable]
    public class QueuedTerminalInputSystem {

        private Queue<QueuedTerminalText> queue;
        private int cursor;
        private QueuedTerminalText textToDisplay;

        private static ILogger logger = Debug.unityLogger;

        public QueuedTerminalInputSystem(){
            this.queue = new Queue<QueuedTerminalText>();
            this.cursor = 0;
            this.textToDisplay = null;
        }

        public void Enqueue(String str, Boolean isDisplayingAllAtOnce = false){
            QueuedTerminalText newTerminalText = new QueuedTerminalText(str, isDisplayingAllAtOnce);
            this.queue.Enqueue(newTerminalText);
        }

        public Boolean TryDequeue(out QueuedTerminalText result){
            return this.queue.TryDequeue(out result);
        }

        public QueuedTerminalText Dequeue(){
            return this.queue.Dequeue();
        }

        public QueuedTerminalText Peek(){
            return this.queue.Peek();
        }

        public String Next(){
            if(this.textToDisplay == null && this.queue.Count == 0) return null;
            else if((this.textToDisplay != null && this.cursor == this.textToDisplay.text.Length) || (this.textToDisplay == null && this.queue.Count > 0)){
                this.cursor = 0;
                Boolean succeedToDequeue = this.TryDequeue(out this.textToDisplay);

                if(!succeedToDequeue){
                    this.textToDisplay = null;
                    return null;
                }
            }

            if(this.cursor < this.textToDisplay.text.Length){
                if(this.textToDisplay.isDisplayingAllAtOnce){
                    this.cursor = this.textToDisplay.text.Length;
                    return this.textToDisplay.text;
                }else{
                    int position = this.cursor++;
                    return this.textToDisplay.text.Substring(position, 1);
                }
            }
            return null;
        }

    }
}