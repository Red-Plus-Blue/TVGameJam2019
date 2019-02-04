using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class Board : MonoBehaviour
    {
        protected Map<NodeData> _map;

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
            var players = GameManager.Instance.LevelData.Players;
            _turnManager = new TurnManager(players);
            _turnManager.OnRoundStart = (round, callback) => { Debug.Log("Starting round: " + round); callback(); };
            _turnManager.OnTurnStart = (player, callback) => { Debug.Log("It is " + player.Name + "'s turn"); callback(); };
            _turnManager.NextRound();
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
                        attacks.Add(new ActionInfo(
                            move,
                            null,
                            new Point(enemy.X, enemy.Y),
                            ActionType.ATTACK
                        ));
                    }
                });
                
            });

            attacks.ForEach(attack => actions.Add(attack));
            return actions;
        }
    }
}