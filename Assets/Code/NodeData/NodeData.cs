using System;
using UnityEngine;

namespace Game.Assets
{
    [Serializable]
    public class NodeData
    {
        public string ID = "";
        public string Name = "";
        public bool Walkable = true;
        public GameObject Prefab;
    }
}