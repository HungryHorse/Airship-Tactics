using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerUnit : MonoBehaviour, ISelectable, ISerialisable
{
    private bool IsHovering { get; set; }

    public AbstractUnit BaseUnit { get; set; }
    private PlayerUnitController UnitController { get; set; }
    private MapController MapController { get; set; }

    [Inject]
    public void Construct(PlayerUnitController unitController, MapController mapController, SerialisationController serialisationController)
    {
        UnitController = unitController;
        MapController = mapController;
        serialisationController.Subscribe(this);
    }

    private void OnMouseEnter()
    {
        IsHovering = true;
    }

    private void OnMouseExit()
    {
        IsHovering = false;
    }

    private void Update()
    {
        if (IsHovering && Input.GetButtonUp("Fire1"))
        {
            Select();
        }
    }

    public void Select()
    {
        UnitController.Select(this);
    }

    public AbstractSerialisationModel GetModel()
    {
        return new PlayerUnitSerialisationModel()
        {
            CurrentPositionX = BaseUnit.CurrentLocation.Coordinates.x,
            CurrentPositionY = BaseUnit.CurrentLocation.Coordinates.y
        };
    }

    public void Load(AbstractSerialisationModel model)
    {
        PlayerUnitSerialisationModel playerModel = (PlayerUnitSerialisationModel)model;
        BaseUnit.SetTile(MapController.MapTiles[playerModel.CurrentPositionX, playerModel.CurrentPositionY]);
    }
}
