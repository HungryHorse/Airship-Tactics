using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;

public class MapController : MonoBehaviour
{
#pragma warning disable IDE0044, RCS1169
    [SerializeField]
    private MapTile mapTilePrefab;
    private MapTile MapTilePrefab => mapTilePrefab;
#pragma warning restore IDE0044, RCS1169

    public MapTile[,] MapTiles;

    public Vector2Int Dimensions { get; private set; }

    private Flow BoardFlow { get; set; }

#pragma warning disable IDE0051, RCS1213
    private void Start()
    {
        Load(new Vector2Int(11, 11));
    }
#pragma warning restore IDE0051, RCS1213

    public void Load(Vector2Int dimensions)
    {
        Dimensions = dimensions;
        BuildBoard();
        PopulateNeighbours();

        PlayBoardAnimation();
    }

    private void BuildBoard()
    {
        MapTiles = new MapTile[Dimensions.x, Dimensions.y];
        for (int x = 0; x < Dimensions.x; x++)
        {
            for (int y = 0; y < Dimensions.y; y++)
            {
                MapTiles[x, y] = CreateGridCell(x, y);
            }
        }
    }

    private MapTile CreateGridCell(int x, int y)
    {
        MapTile tile = Instantiate(MapTilePrefab, transform);
        tile.name = $"Cell ({x},{y})";
        tile.Init(new Vector2Int(x, y));
        Vector3 modelScale = Vector3.one * 1.75f;
        float halfGridSize = Dimensions.x / 2f;
        Vector2 offsetPosition = new Vector2((x - halfGridSize) / 1.15f, (y + (x % 2 == 0 ? 0 : 0.5f) - halfGridSize) * -1);
        Vector2 scaledOffsetPosition = Vector2.Scale(offsetPosition, new Vector2(modelScale.x, modelScale.y));
        tile.transform.localPosition = new Vector3(transform.position.x + scaledOffsetPosition.x, 0, transform.position.y + scaledOffsetPosition.y);
        return tile;
    }

    private void PopulateNeighbours()
    {
        for (int x = 0; x < Dimensions.x; x++)
        {
            for (int y = 0; y < Dimensions.y; y++)
            {
                MapTiles[x, y].Neighbours = new Dictionary<CardinalDirections, MapTile>
                {
                    {CardinalDirections.North, y-1 >= 0 ? MapTiles[x, y-1] : null},
                    {CardinalDirections.East, x+1 < Dimensions.x ? MapTiles[x+1, y] : null},
                    {CardinalDirections.South, y+1 < Dimensions.y ? MapTiles[x, y+1] : null},
                    {CardinalDirections.West, x-1 >= 0 ? MapTiles[x-1, y] : null},
                };
            }
        }
    }

    private void PlayBoardAnimation()
    {
        BoardFlow?.Stop();
        BoardFlow = new Flow();

        for (int x = 0; x < Dimensions.x; x++)
        {
            for (int y = 0; y < Dimensions.y; y++)
            {
                BoardFlow.At((x / 2f) + (y / 10f), MapTiles[x, y].GetWobbleAnimation());
            }
        }

        BoardFlow.Start();
    }
}