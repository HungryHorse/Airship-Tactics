using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public Vector2Int Coordinates { get; private set; }

    public Dictionary<CardinalDirections, MapTile> Neighbours { get; set; } = new Dictionary<CardinalDirections, MapTile>();

    private AbstractAnimation TileAnimation { get; set; }

    public void Init(Vector2Int coordinates)
    {
        Coordinates = coordinates;
    }

    public AbstractAnimation GetWobbleAnimation()
    {
        TileAnimation?.Stop();
        TileAnimation = new Flow()
            .Queue(new Tween(0.5f)
                .SetEasing(Easing.EaseOutBack)
                .For(transform)
                    .MoveLocalY(1)
            )
            .QueueDelay(0.1f)
            .Queue(new Tween(0.5f)
                .SetEasing(Easing.EaseOutBack)
                .For(transform)
                    .MoveLocalY(-1)
            );

        return TileAnimation;
    }
}
