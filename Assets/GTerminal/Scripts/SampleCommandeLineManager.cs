using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCommandeLineManager : MonoBehaviour
{
    public void writeToTerminal(GTerminal terminal, String cmd){
        terminal.AppendLine("You can write condition and display text depending on the executed command.");
        terminal.AppendLine("Command is : " + cmd);
    }
}
