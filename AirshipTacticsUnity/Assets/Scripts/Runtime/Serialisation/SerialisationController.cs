using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialisationController : MonoBehaviour
{
    private HashSet<ISerialisable> Observers { get; }

    public void Subscribe(ISerialisable serialisable)
    {
        Observers.Add(serialisable);
    }

    public void Save()
    {
        foreach (ISerialisable serialisable in Observers)
        {

        }
    }
}
