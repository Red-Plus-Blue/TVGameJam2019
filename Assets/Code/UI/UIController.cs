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

        public PawnInfo SelectedPawnInfo;
        public PawnInfo VsPawnInfo;
        public Text AttackerDamage;
        public Text DefenderDamage;
        public TerrainInfo TerrainInfo;
        public GameObject[] VsPawnObjects = new GameObject[0];
        public Log Log;

        private void Awake()
        {
            Instance = this;
            HideVsPawn();
        }

        public void SetPawnData(Pawn pawn)
        {
            SelectedPawnInfo.Show(pawn);
        }

        public void LogMessage(string message)
        {
            Log.LogMessage(message);
        }

        public void SetTerrainData(NodeData data)
        {
            if(data == null)
            {
                return;
            }
            TerrainInfo.Show(data);
        }

        public void SetSelectedPawn(Pawn pawn)
        {
            SelectedPawnInfo.Show(pawn);
        }

        public void SetVsPawnData(Pawn selected, Pawn vs)
        {
            if(vs == null || selected == null)
            {
                HideVsPawn();
                return;
            }

            AttackerDamage.text = Combat.Damage(selected.Attack, vs.Defence, selected.Damage, 50).ToString();
            DefenderDamage.text = Combat.Damage(vs.Attack, selected.Defence, vs.Damage, 50).ToString();

            ShowVsPawn();
            VsPawnInfo.Show(vs);
        }

        public void ShowVsPawn()
        {
            VsPawnObjects.ToList().ForEach(element => element.SetActive(true));
        }

        public void HideVsPawn()
        {
            VsPawnObjects.ToList().ForEach(element => element.SetActive(false));
        }
    }
}