using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class PathfindingTester : MonoBehaviour
    {
        protected static NodeData Walkable = new NodeData();
        protected static NodeData Blocked = new NodeData() { Walkable = false };

        protected Map<NodeData> _map = new Map<NodeData>(10, 10, Walkable);

        public GameObject HighlightPrefab;
        public GameObject OpenTilePrefab;
        public GameObject BlockedTilePrefab;

        private void Start()
        {
            for(int i = 0; i < 9; i++)
            {
                var position1 = new Point(i, 3);
                _map.SetData(position1, Blocked);

                var position2 = new Point(1 + i, 6);
                _map.SetData(position2, Blocked);
            }

            _map.GetNodes().ForEach(node =>
            {
                var prefab = node.Data.Walkable ? OpenTilePrefab : BlockedTilePrefab;
                var prefabObject = Instantiate(prefab, new Vector2(node.X, node.Y), Quaternion.identity);
            });

            var pawn = new Pawn();

            var from = new Point(0, 0);
            var to = new Point(9, 9);

            var path = _map.GetPath(from, to, pawn);
            path.ForEach(node => Debug.Log(String.Format("({0}, {1})", node.X, node.Y)));
            path.ForEach(node => Instantiate(HighlightPrefab, new Vector3(node.X, node.Y, -1.0f), Quaternion.identity));
        }

    }
}
