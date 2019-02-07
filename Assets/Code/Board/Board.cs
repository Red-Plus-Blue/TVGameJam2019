using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class Board : MonoBehaviour
    {
        protected Map<NodeData> _map;
        protected LevelData _levelData;

        protected Dictionary<Node<NodeData>, GameObject> _nodesToGameObejcts = new Dictionary<Node<NodeData>, GameObject>();

        protected TurnManager _turnManager;

        private void Awake()
        {
            GameManager.Instance.Board = this;
            _map = GameManager.Instance.LevelData.Map;

            _map.GetNodes().ForEach(node =>
            {
                var position = new Vector2(node.X, node.Y);
                var nodeObject = GameObject.Instantiate(node.Data.Prefab, position, Quaternion.identity);
                _nodesToGameObejcts.Add(node, nodeObject);
            });

            _map.GetAgents().ForEach(agent =>
            {
                var pawn = (Pawn)agent;
                var position = new Vector2(pawn.X, pawn.Y);
                var pawnObject = GameObject.Instantiate(pawn.Prefab, position, Quaternion.identity);
                var pawnComponent = pawnObject.GetComponent<PawnComponent>();
                pawn.PawnComponent = pawnComponent;
            });
        }

        private void Start()
        {
            _levelData = GameManager.Instance.LevelData;
            var players = _levelData.Players;

            _turnManager = new TurnManager(players);
            _turnManager.OnRoundStart = (round, callback) => { Debug.Log("Starting round: " + round); callback(); };
            _turnManager.OnTurnStart += OnTurnStart;
            _turnManager.NextRound();
        }

        public void OnTurnStart(Player player, Action turnManagerCallback)
        {
            var missionComplete = _levelData.Mission.CheckMissionComplete(_map);

            if(missionComplete)
            {
                Debug.Log("Victory");
                return;
            }

            var playerDefeated = _levelData.Mission.CheckPlayerDefated(_map);

            if(playerDefeated)
            {
                Debug.Log("Defeat");
                return;
            }

            turnManagerCallback();
        }

        public void DeathCheck(Pawn pawn)
        {
            if(pawn.IsDead())
            {
                var position = new Vector2(pawn.X, pawn.Y);
                var roll = UnityEngine.Random.Range(0, 4);
                var orientation = Quaternion.Euler(0.0f, 0.0f, roll * 90);
                GameObject.Instantiate(GameManager.Instance.FXAtlas.Blood, position, orientation);

                GameObject.Destroy(pawn.PawnComponent.gameObject);
                pawn.X = -1;
                pawn.Y = -1;
                _map.RemoveAgent(pawn);

                UIController.Instance.LogMessage(string.Format("{0} has been slain!", pawn.GetFullName()));
            }
        }

        public List<ActionInfo> GetActionsFor(Pawn pawn)
        {
            var actions = new List<ActionInfo>();

            // Determine Moves
            var moves = new List<ActionInfo>();

            if(pawn.MovePoints < 1)
            {
                return moves;
            }

            var nodesInRange = _map.GetTilesInRange(pawn, pawn.MovePoints);

            moves = nodesInRange.Select(node => {
                return  new ActionInfo(
                    null,
                    null,
                    new Point(node.X, node.Y),
                    ActionType.MOVE
                );
            }).ToList();

            moves.ForEach(move => actions.Add(move));

            // Determine Attacks
            var attacks = new List<ActionInfo>();

            var enemies = _map.GetAgents()
               .Select(agent => (Pawn)agent)
               .Where(otherPawn => {
                   return otherPawn.Owner != pawn.Owner;
               }).ToList();

            int Distance(int x1, int y1, int x2, int y2)
            {
                var xDistance = Math.Abs(x2 - x1);
                var yDistance = Math.Abs(y2 - y1);
                return Math.Max(xDistance, yDistance);
            }

            moves.ForEach(move =>
            {
                enemies.ForEach(enemy =>
                {
                    var distance = Distance(move.Location.X, move.Location.Y, enemy.X, enemy.Y);
                    var seenBefore = attacks.Where(attack => {
                        return (attack.Location.X == enemy.X) && (attack.Location.Y == enemy.Y); 
                    }).ToList().Count > 0;

                    if(distance <= pawn.AttackRange && seenBefore == false)
                    {
                        var action = new ActionInfo(
                            move,
                            null,
                            new Point(enemy.X, enemy.Y),
                            ActionType.ATTACK);
                        action.Target = enemy;
                        attacks.Add(action);
                    }
                });
                
            });

            attacks.ForEach(attack => actions.Add(attack));
            return actions;
        }

        public IEnumerator Attack(Pawn attacker, Pawn defender)
        {
            // Animate Attack
            yield return attacker.PawnComponent.Attack(new Vector2(defender.X, defender.Y));

            // Determine result
            var combatResult = Combat.Attack(attacker, defender);
            defender.Health -= combatResult.Damage;

            // Print result
            var attackString = string.Format(
                "{0} attacks {1} for {2} damage. (Roll: {3})",
                attacker.GetFullName(),
                defender.GetFullName(),
                combatResult.Damage,
                combatResult.LuckRoll
            );
            UIController.Instance.LogMessage(attackString);
        }

        public IEnumerator Move(Pawn selectedUnit, ActionInfo action)
        {
            var pawnComponent = selectedUnit.PawnComponent;
            var start = new Point(selectedUnit.X, selectedUnit.Y);
            var path = _map.GetPath(start, action.Location, selectedUnit);
            selectedUnit.X = action.Location.X;
            selectedUnit.Y = action.Location.Y;
            yield return pawnComponent.Move(path.Select(node => new Vector2(node.X, node.Y)).ToList());
        }
    }
}