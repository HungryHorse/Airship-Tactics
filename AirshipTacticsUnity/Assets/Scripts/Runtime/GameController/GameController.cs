using System.Collections;
using System.Collections.Generic;
using FriedSynapse.Quickit;
using UnityEngine;

public class GameController : ManualSingletonBehaviour<GameController>
{
#pragma warning disable IDE0044, RCS1169
    [SerializeField]
    private CameraController cameraController;
    private CameraController CameraController => cameraController;
#pragma warning restore IDE0044, RCS1169

#pragma warning disable IDE0051, RCS1213
    private void Awake()
    {
        CameraController.Init();
        CameraController.StartEcho();
    }
#pragma warning restore IDE0051, RCS1213
}
