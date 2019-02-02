using System;
using Game.Pathfinding;
using UnityEngine;

namespace Game.Assets
{
    [Serializable]
    public class Pawn : Agent<NodeData>, ICloneable
    {
        public String Name;
        public GameObject Prefab;
        public PawnComponent PawnComponent { get; set; }

        public override bool CanEnter(Node<NodeData> node)
        {
            return node.Data.Walkable;
        }

        public object Clone()
        {
            var pawn = new Pawn();
            pawn.Name = Name;
            pawn.Prefab = Prefab;
            pawn.X = X;
            pawn.Y = Y;

            return pawn;
        }
    }
}
