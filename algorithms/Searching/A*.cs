using System;
using System.Collections.Generic;
using System.Linq;

public class AStar
{
    private int width;
    private int height;
    private Node[,] grid;

    public AStar(int[,] map)
    {
        width = map.GetLength(0);
        height = map.GetLength(1);
        grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Node(x, y, map[x, y] == 0);
            }
        }
    }

    public List<Node> FindPath(Node start, Node target)
    {
        var openList = new List<Node>();
        var closedList = new HashSet<Node>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            Node current = openList.OrderBy(n => n.F).First();

            if (current == target)
                return RetracePath(start, target);

            openList.Remove(current);
            closedList.Add(current);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (!neighbor.Walkable || closedList.Contains(neighbor))
                    continue;

                int newCost = current.G + 1;

                if (newCost < neighbor.G || !openList.Contains(neighbor))
                {
                    neighbor.G = newCost;
                    neighbor.H = GetDistance(neighbor, target);
                    neighbor.Parent = current;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null;
    }

    private List<Node> RetracePath(Node start, Node end)
    {
        var path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    private List<Node> GetNeighbors(Node node)
    {
        var neighbors = new List<Node>();

        int[,] directions =
        {
            { 0, 1 },
            { 1, 0 },
            { 0, -1 },
            { -1, 0 }
        };

        foreach (var _ in Enumerable.Range(0, 4))
        {
            int dx = directions[_, 0];
            int dy = directions[_, 1];

            int x = node.X + dx;
            int y = node.Y + dy;

            if (x >= 0 && x < width && y >= 0 && y < height)
                neighbors.Add(grid[x, y]);
        }

        return neighbors;
    }

    public Node GetNode(int x, int y)
    {
        return grid[x, y];
    }
}
