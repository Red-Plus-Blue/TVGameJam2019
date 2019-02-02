using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Pathfinding
{
    public class Node<T>
    {
        public int      X { get; } = 0;
        public int      Y { get; } = 0;
        public T        Data { get; set; }

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}