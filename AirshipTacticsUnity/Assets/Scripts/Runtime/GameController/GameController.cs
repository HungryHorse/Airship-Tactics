using System.Collections;
using System.Collections.Generic;
using FriedSynapse.Quickit;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour, IGameController, IInitializable
{
    private CameraController CameraController;
    private PlayerUnitFactory PlayerUnitFactory;

    [Inject]
    public void Construct(CameraController cameraController, PlayerUnitFactory playerUnitFactory)
    {
        CameraController = cameraController;
        PlayerUnitFactory = playerUnitFactory;
    }

    public void Initialize()
    {
        PlayerUnitFactory.Create(UnitClasses.Frigate);
    }
}
