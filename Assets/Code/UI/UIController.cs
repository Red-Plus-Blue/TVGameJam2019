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
        public Text SelectedText;

        private void Awake()
        {
            Instance = this;
        }

        public void SetPawnData(Pawn pawn)
        {
            PawnText.text = GetPawnDescription(pawn);
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

        public void SetSelectedPawn(Pawn pawn)
        {
            SelectedText.text = GetPawnDescription(pawn);
        }

        protected string GetPawnDescription(Pawn pawn)
        {
            if (pawn == null)
            {
                return "";
            }

            var description = String.Format(
                "Name: {0}\n" +
                "Owner: {1}",
                pawn.Name,
                pawn.Owner.Name
            );
            return description;
        }

    }
}