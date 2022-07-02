using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerUnitController : IInitializable
{
    private MapController MapController { get; set; }
    private PlayerUnitFactory UnitFactory { get; set; }

    private PlayerUnit SelectedUnit { get; set; }

    [Inject]
    public void Construct(MapController mapController, PlayerUnitFactory unitFactory)
    {
        MapController = mapController;
        UnitFactory = unitFactory;
    }

    public void Initialize()
    {

    }

    public void Select(ISelectable selectable)
    {
        switch (selectable)
        {
            case PlayerUnit playerUnit:
                SelectUnit(playerUnit);
                break;
            case MapTile mapTile:
                SelectMapTile(mapTile);
                break;
        }
    }

    private void SelectUnit(PlayerUnit selectedUnit)
    {
        Debug.Log("Player unit clicked");
        SelectedUnit = selectedUnit;
    }

    private void SelectMapTile(MapTile mapTile)
    {
        Debug.Log("Map tile clicked");
        SelectedUnit?.BaseUnit.MoveToTile(mapTile);
        SelectedUnit = null;
    }

    private void SelectTarget()
    {

    }
}
