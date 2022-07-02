using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CustomPlayerUnitFactory : IFactory<UnitClasses, MapTile, AbstractUnit>
{
    private DiContainer Container { get; set; }
    private UnitPrefabs UnitPrefabs { get; set; }

    [Inject]
    public void Construct(DiContainer container, UnitPrefabs unitPrefabs)
    {
        Container = container;
        UnitPrefabs = unitPrefabs;
    }

    public AbstractUnit Create(UnitClasses unitClass, MapTile spawnLocation)
    {
        AbstractUnit unit = Container.InstantiatePrefabForComponent<AbstractUnit>(UnitPrefabs[unitClass]);
        PlayerUnit playerComponent = unit.gameObject.AddComponent<PlayerUnit>();
        Container.Inject(playerComponent);
        unit.transform.position = spawnLocation.transform.position + unit.Offset;
        return unit;
    }
}
