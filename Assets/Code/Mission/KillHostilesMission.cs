using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class KillHostilesMission : Mission
    {
        public KillHostilesMission(Player player) : base(player) { }

        public override bool CheckMissionComplete(Map<NodeData> map)
        {
            return map.GetAgents()
                .Select(agent => (Pawn)agent)
                .Where(pawn => pawn.Owner != _player)
                .Count() < 1;
        }
    }
}