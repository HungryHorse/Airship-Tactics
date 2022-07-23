using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialisationModelConverter : AbstractEnumSpecifiedClassConverter<AbstractSerialisationModel, SerialisationModelType>
{
    protected override Dictionary<SerialisationModelType, Type> TypeDictionary => new Dictionary<SerialisationModelType, Type>()
    {
        {SerialisationModelType.PlayerUnit, typeof(PlayerUnitSerialisationModel)}
    };
}
