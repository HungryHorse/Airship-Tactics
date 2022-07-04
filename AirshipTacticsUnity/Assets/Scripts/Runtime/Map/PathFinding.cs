using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathFinding
{
    public static List<MapTile> AStar(MapTile[,] Map, MapTile start, MapTile goal)
    {
        List<PathFindingNode> openPath = new List<PathFindingNode>();
        List<MapTile> cameFrom = new List<MapTile>();

        int width = Map.GetLength(1);
        int height = Map.GetLength(0);

        List<PathFindingNode> pathFindingNodes = Enumerable.Range(0, width * height).Select(n => new PathFindingNode()).ToList();

        PathFindingNode startNode = pathFindingNodes[GetIndexFromCoord(start, width)];
        startNode.Tile = start;
        startNode.GScore = 0;
        startNode.FScore = Heuristic(start, goal);
        openPath.Add(startNode);

        while (openPath.Count > 0)
        {
            PathFindingNode current = openPath.Aggregate((curMin, x) => curMin == null || x.FScore < curMin.FScore ? x : curMin);

            if (current.Tile == goal)
            {
                return GetPath(current);
            }
            openPath.Remove(current);

            foreach (MapTile neighbour in current.Tile.Neighbours.Values)
            {
                if (neighbour == null)
                {
                    continue;
                }
                PathFindingNode neighbourNode = pathFindingNodes[GetIndexFromCoord(neighbour, width)];
                neighbourNode.Tile = neighbour;
                float tentativeGScore = current.GScore + neighbour.Cost;
                if (tentativeGScore < neighbourNode.GScore)
                {
                    neighbourNode.CameFrom = current;
                    neighbourNode.GScore = tentativeGScore;
                    neighbourNode.FScore = tentativeGScore + Heuristic(neighbourNode.Tile, goal);
                    if (!openPath.Contains(neighbourNode))
                    {
                        openPath.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    private static List<MapTile> GetPath(PathFindingNode current)
    {
        List<MapTile> path = new List<MapTile>();
        path.Add(current.Tile);
        current = current.CameFrom;
        while (current.CameFrom != null)
        {
            path.Insert(0, current.Tile);
            current = current.CameFrom;
        }
        return path;
    }

    private static int GetIndexFromCoord(MapTile mapTile, int rowLength)
        => (mapTile.Coordinates.x * rowLength) + mapTile.Coordinates.y;

    private static float Heuristic(MapTile start, MapTile end)
        => Mathf.Sqrt(Mathf.Pow(end.Coordinates.x - start.Coordinates.x, 2) + Mathf.Pow(end.Coordinates.y - start.Coordinates.y, 2));
}
