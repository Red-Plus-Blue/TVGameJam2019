using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Assets
{
    [Serializable]
    public class DialogSegment
    {
        public bool IsLeftCharacter = false;
        public string Text = "";
        public string Option1 = "";
        public string Option2 = "";
    }
}