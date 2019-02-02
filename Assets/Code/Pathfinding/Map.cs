﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Pathfinding
{
    public class Map<T>
    {
        protected Node<T>[,] _nodes = new Node<T>[1, 1];

        public int MaxX { get; }  = 1;
        public int MaxY { get; } = 1;

        public Map(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            _nodes = new Node<T>[MaxX, MaxY];

            for (int x = 0; x < MaxX; x++)
            {
                for (int y = 0; y < MaxY; y++)
                {
                 
                    var node = new Node<T>(x, y);
                    _nodes[x, y] = node;
                }
            }
        }

        public void SetData(Point position, T data)
        {
            _nodes[position.X, position.Y].Data = data;
        }

        public T GetData(Point position)
        {
            return _nodes[position.X, position.Y].Data;
        }

        protected float GetCost(Node<T> from, Node<T> to)
        {
            var xDifference = to.X - from.X;
            var yDifference = to.Y - from.Y;
            return (float)Math.Sqrt((xDifference * xDifference) + (yDifference * yDifference));
        }

        protected List<Node<T>> ConstructPath(Dictionary<Node<T>, (Node<T> cameFrom, float g, float f)> metaData, Node<T> goal)
        {
            var path = new List<Node<T>>();
            var current = goal;
            path.Add(current);

            while (metaData.ContainsKey(current))
            {
                current = metaData[current].cameFrom;
                if(current == null)
                {
                    break;
                }
                path.Add(current);
            }

            return path;
        }

        protected List<Node<T>> GetNeighbors(Node<T> node)
        {
            var neighbors = new List<Node<T>>();
            for(int xOffset = -1; xOffset < 2; xOffset += 1)
            {
                for(int yOffset = -1; yOffset < 2; yOffset += 1)
                {
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }

                    var xTarget = node.X + xOffset;
                    var yTarget = node.Y + yOffset;

                    if ((xTarget >= 0) && (yTarget >= 0) && (xTarget < MaxX) && (yTarget < MaxY))
                    {
                        neighbors.Add(_nodes[xTarget, yTarget]);
                    }
                }
            }
            return neighbors;
        }

        public List<Node<T>> GetPath(Point from, Point to, Action<String> log)
        {
            float Distance(int x1, int y1, int x2, int y2)
            {
                var xDifference = x2 - x1;
                var yDifference = y2 - y1;
                return (float)Math.Sqrt((xDifference * xDifference) + (yDifference * yDifference));
            }

            var start = _nodes[from.X, from.Y];
            var goal = _nodes[to.X, to.Y];

            var closedSet   = new List<Node<T>>();
            var openSet     = new List<Node<T>>();
            openSet.Add(start);

            var metaData    = new Dictionary<Node<T>, (Node<T> cameFrom, float g, float f)>();
            metaData[start] = (cameFrom: null, g: 0, f: GetCost(start, goal));

            while (openSet.Count > 0)
            {
                log("---------- Iteration ----------");

                var minFScore   = openSet.Min(node => metaData[node].f);
                var current     = openSet.First(node => metaData[node].f == minFScore);

                log("Open Set Size: " + openSet.Count);
                log("Min F:" + minFScore);
                log("Current: " + current.ToString());

                if (current == goal)
                {
                    return ConstructPath(metaData, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                var neighbors = GetNeighbors(current);

                log("Neighbors: " + neighbors.Count);

                neighbors.ForEach(neighbor =>
                {
                    log(">>>> Neighbor(" + neighbor.X + "," + neighbor.Y + "):");

                    if (closedSet.Contains(neighbor))
                    {
                        log("     *In closed set");
                        return;
                    }
                    var g = metaData[current].g + Distance(current.X, current.Y, neighbor.X, neighbor.Y);
                    if(openSet.Contains(neighbor) == false)
                    {
                        openSet.Add(neighbor);
                    }
                    else if(g > metaData[neighbor].g)
                    {
                        log("     *g score too high");
                        return;
                    }
                    metaData[neighbor] = (cameFrom: current, g: g, f: g + GetCost(neighbor, goal));
                    log("     Added (g: " + metaData[neighbor].g + ",f: " + metaData[neighbor].f + ")");
                });
            }

            throw new NoPathException();
        }
    }
}