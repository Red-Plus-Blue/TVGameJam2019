using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Assets
{
    public class Player : MonoBehaviour
    {
        public string Name { get; set; } = "Player";
        public bool IsHuman { get; set; } = false;
        public Action OnStartTurn { get; set; }
        protected Action _onEndTurn { get; set; }

        public void TakeTurn(Action onEndTurn)
        {
            _onEndTurn = onEndTurn;
            if (OnStartTurn != null)
            {
                OnStartTurn();
            }
            else
            {
                _onEndTurn();
            }
        }

        public void EndTurn()
        {
            _onEndTurn();
        }

        public void WaitForInput()
        {
            // Do nothing...
        }
    }
}