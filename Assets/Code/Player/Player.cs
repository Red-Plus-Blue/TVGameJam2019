using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Assets
{
    public class Player
    {
        public string Name { get; set; } = "Player";
        public bool IsHuman { get; set; } = false;
        public Action OnStartTurn { get; set; }
        protected Action _onEndTurn { get; set; }

        public Player(string name)
        {
            Name = name;
        }

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
            // Do nothing
        }
    }
}