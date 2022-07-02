using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;
using Zenject;

public class MapController : MonoBehaviour, IInitializable
{
    public MapTile[,] MapTiles;

    private MapTile.Factory TileFactory { get; set; }

    public Vector2Int Dimensions { get; private set; }

    private Flow BoardFlow { get; set; }

    [Inject]
    public void Construct(MapTile.Factory tileFactory)
    {
        TileFactory = tileFactory;
    }

    public void Initialize()
    {
        Load(new Vector2Int(11, 11));
    }

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
        MapTile tile = TileFactory.Create();
        tile.gameObject.transform.SetParent(transform);
        Vector3 modelScale = Vector3.one * 1.75f;
        float halfGridSize = Dimensions.x / 2f;
        Vector2 offsetPosition = new Vector2((x - halfGridSize) / 1.15f, (y + (x % 2 == 0 ? 0 : 0.5f) - halfGridSize) * -1);
        Vector2 scaledOffsetPosition = Vector2.Scale(offsetPosition, new Vector2(modelScale.x, modelScale.y));
        Vector3 localPos = new Vector3(transform.position.x + scaledOffsetPosition.x, 0, transform.position.y + scaledOffsetPosition.y);
        tile.transform.localPosition = localPos;
        tile.name = $"Cell ({x},{y})";
        tile.Init(new Vector2Int(x, y), localPos);
        return tile;
    }

    private void PopulateNeighbours()
    {
        for (int x = 0; x < Dimensions.x; x++)
        {
            for (int y = 0; y < Dimensions.y; y++)
            {
                int yOffset = x % 2 == 0 ? 1 : 0;
                MapTiles[x, y].Neighbours = new Dictionary<Directions, MapTile>
                {
                    {Directions.North, y-1 >= 0 ? MapTiles[x, y-1] : null},
                    {Directions.NorthEast, x+1 < Dimensions.x && y - yOffset >= 0 ? MapTiles[x+1, y - yOffset] : null},
                    {Directions.NorthWest, x-1 >= 0 && y - yOffset >= 0 ? MapTiles[x-1, y - yOffset] : null},

                    {Directions.South, y+1 < Dimensions.y ? MapTiles[x, y+1] : null},
                    {Directions.SouthEast, x+1 < Dimensions.x && y - yOffset + 1 < Dimensions.y ? MapTiles[x+1, y - yOffset + 1] : null},
                    {Directions.SouthWest, x-1 >= 0 &&  y - yOffset + 1 < Dimensions.y ? MapTiles[x-1, y - yOffset + 1] : null},
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

    private MapTile GetTile(Vector2Int coordinate)
    {
        return MapTiles[coordinate.x, coordinate.y];
    }
}
