using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode
{
    public float GScore = Mathf.Infinity;
    public float FScore = Mathf.Infinity;
    public PathFindingNode CameFrom { get; set; }
    public MapTile Tile { get; set; }
}
