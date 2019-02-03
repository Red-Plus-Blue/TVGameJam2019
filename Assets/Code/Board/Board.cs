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
    }
}