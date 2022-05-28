using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;

public static class CameraControllerMotionExtensions
{
    public static EchoMotionProxy<TCameraController> MoveByInput<TCameraController>(this EchoMotionProxy<TCameraController> proxy, float speed = CameraController.MoveCameraByInputMotion.DefaultSpeed)
        where TCameraController : CameraController
        => proxy.Apply(new CameraController.MoveCameraByInputMotion(proxy.Item, speed));

    public static EchoMotionProxy<TCameraController> RotateByInput<TCameraController>(this EchoMotionProxy<TCameraController> proxy, float speed = CameraController.RotateCameraByInputMotion.DefaultSensitivity)
        where TCameraController : CameraController
        => proxy.Apply(new CameraController.RotateCameraByInputMotion(proxy.Item, speed));
}
