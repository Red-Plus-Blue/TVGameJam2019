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
        protected Dictionary<Pawn, PawnComponent> _pawnsToGameObjects = new Dictionary<Pawn, PawnComponent>();

        public Pawn Pawn;

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

            var pawn = GameManager.Instance.LevelData.Pawn;
            var pawnObject = GameObject.Instantiate(pawn.Prefab, Vector2.zero, Quaternion.identity);
            var pawnComponent = pawnObject.GetComponent<PawnComponent>();

            _pawnsToGameObjects.Add(pawn, pawnComponent);

            Pawn = pawn;
        }

        public PawnComponent ComponentForPawn(Pawn pawn)
        {
            return _pawnsToGameObjects[pawn];
        }
    }
}