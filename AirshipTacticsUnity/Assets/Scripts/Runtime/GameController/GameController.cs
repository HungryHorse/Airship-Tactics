using System.Collections;
using System.Collections.Generic;
using FriedSynapse.Quickit;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour, IGameController, IInitializable
{
    private CameraController CameraController;
    private PlayerUnitFactory PlayerUnitFactory;
    private MapController MapController;

    [Inject]
    public void Construct(CameraController cameraController, PlayerUnitFactory playerUnitFactory, MapController mapController)
    {
        CameraController = cameraController;
        PlayerUnitFactory = playerUnitFactory;
        MapController = mapController;
    }

    public void Initialize()
    {
        PlayerUnitFactory.Create(UnitClasses.Frigate, MapController.MapTiles[0, 0]);
    }
}
