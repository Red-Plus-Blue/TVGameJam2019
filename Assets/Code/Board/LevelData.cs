using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class LevelData
    {
        public Map<NodeData> Map { get; set; }
        public Pawn Pawn { get; set; }
        
        public LevelData(Map<NodeData> map)
        {
            Map = map;
        }
    }
}