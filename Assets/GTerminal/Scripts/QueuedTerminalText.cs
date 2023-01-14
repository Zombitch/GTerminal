using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Ashbay{
    [Serializable]
    public class QueuedTerminalText {
        public String text{ get; set; }
        public Boolean isDisplayingAllAtOnce{ get; set; }

        public QueuedTerminalText(String txt = "", Boolean isDisplayingAll = false){
            this.text = txt;
            this. isDisplayingAllAtOnce = isDisplayingAll;
        }
    }
}