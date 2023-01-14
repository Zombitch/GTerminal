using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ashbay;

namespace Ashbay.Sample{
    public class SampleCommandeLineManager : MonoBehaviour
    {
        private static ILogger logger = Debug.unityLogger;

        public void writeToTerminal(GTerminal terminal, String cmd){
            String[] args = cmd.Split(" ");
            if(args.Length == 2 && args[0].Equals("/speed")){
                terminal.textDisplayRate = float.Parse(args[1]);
                terminal.AppendLine("Changing text speed", false);
            }else{
                terminal.AppendLine("You can write condition and display text depending on the executed command.");
                terminal.AppendLine("Command is : " + cmd);
            }
        }
    }
}