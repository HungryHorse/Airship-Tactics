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
    private SerialisationController SerialisationController { get; set; }

    [Inject]
    public void Construct(CameraController cameraController, PlayerUnitFactory playerUnitFactory, MapController mapController, SerialisationController serialisationController)
    {
        CameraController = cameraController;
        PlayerUnitFactory = playerUnitFactory;
        MapController = mapController;
        SerialisationController = serialisationController;
    }

    public void Initialize()
    {
        PlayerUnitFactory.Create(UnitClasses.Frigate, MapController.MapTiles[0, 0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SerialisationController.SaveAll("TestFile.JSON").RunParallel();
        }
    }
}
