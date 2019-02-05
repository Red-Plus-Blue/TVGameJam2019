using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class Mission
    {
        protected Player _player;

        public Mission(Player player)
        {
            _player = player;
        }

        public virtual bool CheckMissionComplete(Map<NodeData> map)
        {
            return true;
        }

        public virtual bool CheckPlayerDefated(Map<NodeData> map)
        {
            return map.GetAgents()
                .Select(agent => (Pawn)agent)
                .Where(pawn => pawn.Owner == _player)
                .Count() < 1;
        }

    }
}