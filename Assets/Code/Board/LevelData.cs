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
        public List<Player> Players { get; set; } = new List<Player>();
        public Mission Mission { get; set; }
        public List<Point> Objectives { get; set; } = new List<Point>();

        public LevelData(Map<NodeData> map)
        {
            Map = map;
        }
    }
}