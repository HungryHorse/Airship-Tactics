using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SampleInstaller : MonoInstaller
{
#pragma warning disable IDE0044, RCS1169
    [SerializeField]
    private GameController gameController;
    private GameController GameController => gameController;

    [SerializeField]
    private MapController mapController;
    private MapController MapController => mapController;

    [SerializeField]
    private CameraController cameraControllerPrefab;
    private CameraController CameraControllerPrefab => cameraControllerPrefab;

    [SerializeField]
    private UnitPrefabs unitPrefabs;
    private UnitPrefabs UnitPrefabs => unitPrefabs;
#pragma warning restore IDE0044, RCS1169

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MapController>()
            .FromComponentInNewPrefab(MapController)
            .AsSingle()
            .NonLazy();
        Container.BindInterfacesAndSelfTo<CameraController>()
            .FromComponentInNewPrefab(CameraControllerPrefab)
            .AsSingle()
            .NonLazy();
        Container.Bind(typeof(IGameController), typeof(IInitializable))
            .To<GameController>()
            .FromComponentInNewPrefab(GameController)
            .AsSingle()
            .NonLazy();
        Container.Bind<UnitPrefabs>().FromInstance(UnitPrefabs).AsSingle();
        Container.BindFactory<UnitClasses, AbstractUnit, PlayerUnitFactory>().FromFactory<CustomPlayerUnitFactory>();
    }
}
