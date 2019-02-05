using Game.Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Assets
{
    public class TerrainInfo : MonoBehaviour
    {
        public Text Name;
        public Text DefenceBonus;
        public Text MoveCost;

        private void Awake()
        {
            Clear();
        }

        public void Show(NodeData nodeData)
        {
            Clear();

            if (nodeData == null)
            {
                return;
            }

            Name.text = nodeData.Name;
            DefenceBonus.text = "+0";
            MoveCost.text = "1";
        }

        public void Clear()
        {
            Name.text = "";
            DefenceBonus.text = "";
            MoveCost.text = "";
        }
    }
}