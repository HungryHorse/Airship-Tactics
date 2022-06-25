using System;
using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt.Motions.Echo.Abstract;
using UnityEngine;

public partial class CameraController
{
    public class MoveCameraByInputMotion : AbstractFloatSpeedMotion<CameraController>
    {
        [Serializable]
        public class Builder : AbstractFloatSpeedBuilder
        {
            public override IEchoMotion Build()
                => new MoveCameraByInputMotion(item, speed);
        }

        public MoveCameraByInputMotion(CameraController item, float speed = DefaultSpeed, float altitudeSpeed = DefaultSpeed) : base(item, speed)
        {
            transform = item.transform;
            this.altitudeSpeed = altitudeSpeed;
        }

        private readonly Transform transform;
        private readonly float altitudeSpeed;

        public override void OnUpdate(float deltaTime)
        {
            Vector3 move = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            Vector3 altitudeChange = transform.up * Input.GetAxis("Altitude");
            move = Vector3.ClampMagnitude(move, 1);
            altitudeChange = Vector3.ClampMagnitude(altitudeChange, 1);

            transform.position += move * speed * deltaTime;
            transform.position += altitudeChange * altitudeSpeed * deltaTime;
        }
    }

    public class RotateCameraByInputMotion : AbstractEchoMotion<CameraController>
    {
        public const float DefaultSensitivity = 5f;

        [Serializable]
        public class Builder : AbstractBuilder
        {
#pragma warning disable IDE0044, RCS1169
            [SerializeField]
            private float sensitivity = DefaultSensitivity;
#pragma warning restore IDE0044, RCS1169
            public override IEchoMotion Build()
                => new RotateCameraByInputMotion(item, sensitivity);
        }

        public RotateCameraByInputMotion(CameraController item, float sensitivity = DefaultSensitivity) : base(item)
        {
            this.sensitivity = sensitivity;
            transform = item.transform;
        }

        private readonly float sensitivity;
        private readonly Transform transform;

        public override void OnUpdate(float deltaTime)
        {
            if (Input.GetButton("Swivel"))
            {
                Vector3 rotation = transform.up * Input.GetAxis("Mouse X");
                transform.Rotate(rotation * sensitivity);
            }
        }
    }
}
