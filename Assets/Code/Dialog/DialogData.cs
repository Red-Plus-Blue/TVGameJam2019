using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Assets
{
    [Serializable]
    public class DialogData
    {
        public string ID = "";

        public string LeftCharacterName;
        public Sprite LeftCharacterImage;

        public string RightCharacterName;
        public Sprite RightCharacterImage;

        public DialogSegment[] Segments;
    }
}