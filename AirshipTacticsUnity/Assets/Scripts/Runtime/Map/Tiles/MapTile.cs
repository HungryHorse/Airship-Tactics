using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public Vector2Int Coordinates { get; private set; }
    public Vector3 StartPosition { get; private set; }

    public Dictionary<Directions, MapTile> Neighbours { get; set; } = new Dictionary<Directions, MapTile>();

    private AbstractAnimation TileAnimation { get; set; }

    public bool IsHovered { get; private set; }

    public void Init(Vector2Int coordinates, Vector3 startPosition)
    {
        Coordinates = coordinates;
        StartPosition = startPosition;
    }

    #region Unity
#pragma warning disable IDE0051, RCS1213
    private void OnMouseOver()
    {
        if (!IsHovered)
        {
            GetUpDownAnimation(0.4f).OnStarted(() => IsHovered = true).Start();
        }
    }

    private void OnMouseExit()
    {
        if (IsHovered)
        {
            GetReturnToNeutralAnimation(0.5f).OnStarted(() => IsHovered = false).Start();
        }
    }
#pragma warning restore IDE0051, RCS1213
    #endregion

    #region FlowEnt animations
    public AbstractAnimation GetReturnToNeutralAnimation(float time)
    {
        return new Tween(time)
            .SetEasing(Easing.EaseOutSine)
            .For(transform)
                .MoveLocalTo(StartPosition);
    }

    public AbstractAnimation GetUpDownAnimation(float distance)
    {
        return new Tween(0.5f)
            .SetEasing(Easing.EaseOutBack)
            .For(transform)
                .MoveLocalYTo(distance);
    }

    public AbstractAnimation GetWobbleAnimation()
    {
        TileAnimation?.Stop();
        TileAnimation = new Flow()
            .Queue(GetUpDownAnimation(1))
            .QueueDelay(0.1f)
            .Queue(GetReturnToNeutralAnimation(0.5f));

        return TileAnimation;
    }
    #endregion
}
