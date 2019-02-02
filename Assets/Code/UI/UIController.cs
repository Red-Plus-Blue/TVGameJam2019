using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Assets
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance;

        public Text TerrainText;
        public Text PawnText;

        private void Awake()
        {
            Instance = this;
        }

        public void SetPawnData(Pawn pawn)
        {

        }

        public void SetTerrainData(NodeData data)
        {
            if(data == null)
            {
                TerrainText.text = "";
                return;
            }

            TerrainText.text = String.Format(
                "{0}\n" +
                "Walkable: {1}",
                data.Prefab.name,
                data.Walkable
            );
        }

    }
}