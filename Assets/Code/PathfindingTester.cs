using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class PathfindingTester : MonoBehaviour
    {
        protected Map<GameObject> _map = new Map<GameObject>(10, 10);

        private void Start()
        {
            var from = new Point(0, 0);
            var to = new Point(9, 9);

            _map.GetPath(from, to, Debug.Log).ForEach(node => Debug.Log(String.Format("({0}, {1})", node.X, node.Y)));
        }

    }
}
