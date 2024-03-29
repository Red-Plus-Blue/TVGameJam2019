﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Game.Pathfinding
{
    public class Map<T>
    {
        protected Node<T>[,] _nodes = new Node<T>[1, 1];
        protected List<Agent<T>> _agents = new List<Agent<T>>();

        public int MaxX { get; } = 1;
        public int MaxY { get; } = 1;

        public Map(int maxX, int maxY, T fill)
        {
            MaxX = maxX;
            MaxY = maxY;
            _nodes = new Node<T>[MaxX, MaxY];

            for (int x = 0; x < MaxX; x++)
            {
                for (int y = 0; y < MaxY; y++)
                {
                 
                    var node = new Node<T>(x, y);
                    node.Data = fill;
                    _nodes[x, y] = node;
                }
            }
        }

        public bool IsOnMap(Point point)
        {
            return (point.X >= 0) && (point.X < MaxX) && (point.Y >= 0) && (point.Y < MaxY);
        }

        public void AddAgent(Agent<T> agent)
        {
            _agents.Add(agent);
        }

        public void RemoveAgent(Agent<T> agent)
        {
            _agents.Remove(agent);
        }

        public List<Agent<T>> GetAgent(Point point)
        {
            return _agents.Where(agent => { return (agent.X == point.X) && (agent.Y == point.Y); }).ToList();
        }

        public List<Agent<T>> GetAgents()
        {
            return _agents;
        }

        public List<Node<T>> GetNodes()
        {
            var list = new List<Node<T>>();
            foreach(var node in _nodes)
            {
                list.Add(node);
            }
            return list;
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

        public List<Node<T>> GetNeighbors(Point point)
        {
            var neighbors = new List<Node<T>>();
            for (int xOffset = -1; xOffset < 2; xOffset += 1)
            {
                for (int yOffset = -1; yOffset < 2; yOffset += 1)
                {
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }

                    var xTarget = point.X + xOffset;
                    var yTarget = point.Y + yOffset;

                    if ((xTarget >= 0) && (yTarget >= 0) && (xTarget < MaxX) && (yTarget < MaxY))
                    {
                        neighbors.Add(_nodes[xTarget, yTarget]);
                    }
                }
            }
            return neighbors;
        }

        protected List<Node<T>> GetNeighbors(Node<T> node)
        {
            return GetNeighbors(new Point(node.X, node.Y));
        }

        public List<Node<T>> GetPath(Point from, Point to, Agent<T> agent)
        {
            float Distance(int x1, int y1, int x2, int y2)
            {
                var xDifference = x2 - x1;
                var yDifference = y2 - y1;
                return (float)Math.Sqrt((xDifference * xDifference) + (yDifference * yDifference));
            }

            var start = _nodes[to.X, to.Y];
            var goal = _nodes[from.X, from.Y];

            if (agent.CanEnter(start) == false)
            {
                throw new NoPathException();
            }

            var closedSet   = new List<Node<T>>();
            var openSet     = new List<Node<T>>();
            openSet.Add(start);

            var metaData    = new Dictionary<Node<T>, (Node<T> cameFrom, float g, float f)>();
            metaData[start] = (cameFrom: null, g: 0, f: GetCost(start, goal));

            while (openSet.Count > 0)
            {
                var minFScore   = openSet.Min(node => metaData[node].f);
                var current     = openSet.First(node => metaData[node].f == minFScore);

                if (current == goal)
                {
                    return ConstructPath(metaData, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                var neighbors = GetNeighbors(current);

                neighbors.ForEach(neighbor =>
                {
                    var others = GetAgent(new Point(neighbor.X, neighbor.Y));
                    var containsOther = (others.Count > 0) && (others.Contains(agent) == false);

                    if (agent.CanEnter(neighbor) == false || containsOther)
                    {
                        closedSet.Add(neighbor);
                    }

                    if (closedSet.Contains(neighbor))
                    {
                        return;
                    }
                    var g = metaData[current].g + Distance(current.X, current.Y, neighbor.X, neighbor.Y);
                    if(openSet.Contains(neighbor) == false)
                    {
                        openSet.Add(neighbor);
                    }
                    else if(g > metaData[neighbor].g)
                    {
                        return;
                    }
                    metaData[neighbor] = (cameFrom: current, g: g, f: g + GetCost(neighbor, goal));
                });
            }

            throw new NoPathException();
        }

        public List<Node<T>> GetTilesInRange(Agent<T> agent, int range)
        {
            var currentPosition = _nodes[agent.X, agent.Y];
            var tiles = new List<Node<T>>();
            var previousNeighbors = new List<Node<T>>();
            previousNeighbors.Add(currentPosition);

            for(var currentRange = 1; currentRange <= range; currentRange++)
            {
                var neighbors = new List<Node<T>>();

                previousNeighbors.ForEach(node =>
                {
                    GetNeighbors(node).ForEach(neighbor => {
                        var seenBefore = neighbors.Contains(neighbor) || tiles.Contains(neighbor);

                        if (seenBefore == false)
                        {
                            neighbors.Add(neighbor);
                        }
                    });
                });

                previousNeighbors.Clear();

                neighbors.ForEach(neighbor =>
                {
                    var others = GetAgent(new Point(neighbor.X, neighbor.Y));
                    var containsOther = (others.Count > 0) && (others.Contains(agent) == false);

                    if (agent.CanEnter(neighbor) && containsOther == false)
                    {
                        tiles.Add(neighbor);
                        previousNeighbors.Add(neighbor);
                    }
                });
            }

            return tiles;
        }
    }
}