using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CustomPlayerUnitFactory : IFactory<UnitClasses, AbstractUnit>
{
    private DiContainer Container { get; set; }
    private UnitPrefabs UnitPrefabs { get; set; }

    [Inject]
    public void Construct(DiContainer container, UnitPrefabs unitPrefabs)
    {
        Container = container;
        UnitPrefabs = unitPrefabs;
    }

    public AbstractUnit Create(UnitClasses unitClass)
    {
        AbstractUnit unit = Container.InstantiatePrefabForComponent<AbstractUnit>(UnitPrefabs[unitClass]);
        unit.gameObject.AddComponent<PlayerUnit>();
        return unit;
    }
}
