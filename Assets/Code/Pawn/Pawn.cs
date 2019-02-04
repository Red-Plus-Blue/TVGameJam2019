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
        public Player Owner { get; set; }
        public bool CanMove { get; set; } = false;
        public int MovePointsMax    = 1;
        public int MovePoints       = 1;
        public int HealthMax        = 1;
        public int Health           = 1;
        public int AttackRange      = 1;

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
            pawn.MovePointsMax = MovePointsMax;
            pawn.MovePoints = MovePoints;
            pawn.AttackRange = AttackRange;
            pawn.HealthMax = HealthMax;
            pawn.Health = Health;
            return pawn;
        }
    }
}
