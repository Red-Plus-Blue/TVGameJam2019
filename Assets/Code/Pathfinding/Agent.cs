using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Pathfinding
{
    public class Agent<T>
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public virtual bool CanEnter(Node<T> node)
        {
            return true;
        }
    }
}