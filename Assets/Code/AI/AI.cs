using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class AI : Player
    {
        public void Execute()
        {
            StartCoroutine(ExecuteTurn_Coroutine());
        }

        public IEnumerator ExecuteTurn_Coroutine()
        {
            var map = GameManager.Instance.LevelData.Map;
            var board = GameManager.Instance.Board;

            var units = map.GetAgents()
                .Select(agent => (Pawn)agent)
                .Where(pawn => pawn.Owner == this)
                .ToList();

            foreach(var unit in units)
            {
                var actions = board.GetActionsFor(unit);

                var attacks = actions.Where(action => action.ActionType == ActionType.ATTACK).ToList();
                if (attacks.Count > 0)
                {
                    yield return StartCoroutine(ExecuteAction(attacks[0], unit));
                }
                else
                {
                    yield return StartCoroutine(ExecuteAction(actions[0], unit));
                }
            }

            EndTurn();
            yield return null;
        }

        protected IEnumerator ExecuteAction(ActionInfo action, Pawn pawn)
        {
            if (action.Prerequisite != null)
            {
                yield return StartCoroutine(ExecuteAction(action.Prerequisite, pawn));
            }

            var board = GameManager.Instance.Board;

            switch (action.ActionType)
            {
                case ActionType.MOVE:
                    {
                        yield return board.StartCoroutine(board.Move(pawn, action));
                        break;
                    }
                case ActionType.ATTACK:
                    {
                        var attacker = pawn;
                        var defender = action.Target;

                        yield return board.StartCoroutine(board.Attack(attacker, defender));
                        board.DeathCheck(defender);

                        if (defender.IsDead())
                        {
                            break;
                        }

                        yield return board.StartCoroutine(board.Attack(defender, attacker));
                        board.DeathCheck(attacker);
                        break;
                    }
            }
        }

    }
}