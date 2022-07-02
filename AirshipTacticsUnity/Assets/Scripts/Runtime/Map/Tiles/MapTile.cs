using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;
using Zenject;

public class MapTile : MonoBehaviour, ISelectable
{
    public Vector2Int Coordinates { get; private set; }
    public Vector3 StartPosition { get; private set; }

    public Dictionary<Directions, MapTile> Neighbours { get; set; } = new Dictionary<Directions, MapTile>();

    private AbstractAnimation TileAnimation { get; set; }

    public bool IsHovered { get; private set; }

    private PlayerUnitController PlayerUnitController { get; set; }

    [Inject]
    public void Construct(PlayerUnitController playerUnitController)
    {
        PlayerUnitController = playerUnitController;
    }

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

    private void Update()
    {
        if (Input.GetButtonUp("Fire1") && IsHovered)
        {
            PlayerUnitController.Select(this);
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

    public void Select()
    {
        PlayerUnitController.Select(this);
    }

    public class Factory : PlaceholderFactory<MapTile>
    {
    }
}
