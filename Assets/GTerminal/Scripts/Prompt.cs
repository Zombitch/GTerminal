using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Ashbay{
    public class Prompt {
        private String user;
        private String network;
        private String location;
        public int Length{
            get { return this.ToString().Length; }
        }

        public Prompt(String usr="admin", String net = "localhost", String loc = "/"){
            this.Set(usr, net, loc);
        }

        public void Set(String usr, String net, String loc){
            this.user = usr;
            this.network = net;
            this.location = loc;
        }

        public override string ToString(){
            return this.user + "@" + this.network + ":" + this.location + "$ ";
        }  
    }
}