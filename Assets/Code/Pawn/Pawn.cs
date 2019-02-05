using System;
using Game.Pathfinding;
using UnityEngine;

namespace Game.Assets
{
    [Serializable]
    public class Pawn : Agent<NodeData>, ICloneable
    {
        public string Name                      = "Dorky";
        public string Dimension { get; set; }   = "C-137";
        public string Status { get; set; }      = "Scared";

        public GameObject Prefab;

        public PawnComponent PawnComponent { get; set; }
        public Player Owner { get; set; }
        public bool CanMove { get; set; } = false;

        public int MovePointsMax    = 1;
        public int MovePoints       = 1;
        public int HealthMax        = 1;
        public int Health           = 1;
        public int Defence          = 1;
        public int Damage           = 1;
        public int AttackRange      = 1;
        public int Attack           = 1;
        public bool UseDimensionInName = true;

        public override bool CanEnter(Node<NodeData> node)
        {
            return node.Data.Walkable;
        }

        public bool IsDead()
        {
            return Health < 1;
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
            pawn.HealthMax = HealthMax;
            pawn.Health = Health;
            pawn.AttackRange = AttackRange;
            pawn.Defence = Defence;
            pawn.Damage = Damage;
            pawn.Attack = Attack;
            pawn.UseDimensionInName = UseDimensionInName;
            return pawn;
        }

        public string GetFullName()
        {
            if(UseDimensionInName)
            {
                return string.Format("{0} {1}", Name, Dimension);
            }
            else
            {
                return Name;
            }
        }

    }
}
