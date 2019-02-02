using System;
using Game.Pathfinding;
using UnityEngine;

namespace Game.Assets
{
    [Serializable]
    public class Pawn : Agent<NodeData>
    {
        public String Name;
        public GameObject Prefab;

        public override bool CanEnter(Node<NodeData> node)
        {
            return node.Data.Walkable;
        }
    }
}
