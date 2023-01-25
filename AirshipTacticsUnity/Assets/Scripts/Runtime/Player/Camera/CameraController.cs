using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;
using Zenject;

public partial class CameraController : MonoBehaviour, IInitializable
{
#pragma warning disable IDE0044, RCS1169
    [SerializeField]
    private Camera playerCamera;
    public Camera Camera => playerCamera;

    [SerializeField]
    private float moveSpeed;
    private float MoveSpeed => moveSpeed;

    [SerializeField]
    private float altitudeSpeed;
    private float AltitudeSpeed => altitudeSpeed;

    [SerializeField]
    private float rotationSpeed;
    private float RotationSpeed => rotationSpeed;

    [SerializeField]
    private AnimationCurve zoomCurve;
    private AnimationCurve ZoomCurve => zoomCurve;

    [SerializeField]
    private float zoomSpeed;
    private float ZoomSpeed => zoomSpeed;
#pragma warning restore IDE0044, RCS1169

    private Echo CameraMovementEcho { get; set; }

    public void Initialize()
    {
        CameraMovementEcho = new Echo()
            .For(this)
                .MoveByInput(MoveSpeed, AltitudeSpeed)
                .RotateByInput(RotationSpeed)
                .ZoomByInput(ZoomCurve, ZoomSpeed);
        CameraMovementEcho.Start();
    }

    public void StartEcho()
    {
        CameraMovementEcho?.Start();
    }

    public void StopEcho()
    {
        CameraMovementEcho?.Stop();
    }
}
