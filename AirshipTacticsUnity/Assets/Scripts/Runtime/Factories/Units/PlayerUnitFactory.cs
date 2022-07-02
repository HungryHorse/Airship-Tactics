using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


[Serializable]
public class UnitPrefabs : BatteryAcid.Serializables.SerializableDictionary<UnitClasses, AbstractUnit> { }

public class PlayerUnitFactory : PlaceholderFactory<UnitClasses, MapTile, AbstractUnit>
{
}
