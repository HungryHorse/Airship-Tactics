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
                transform.Rotate(rotation * sensitivity * deltaTime);
            }
        }
    }

    public class ZoomCameraByInputMotion : AbstractEchoMotion<CameraController>
    {
        public const float DefaultSensitivity = 5f;

        [Serializable]
        public class Builder : AbstractBuilder
        {
#pragma warning disable IDE0044, RCS1169
            [SerializeField]
            private float sensitivity = DefaultSensitivity;
            [SerializeField]
            private AnimationCurve animationCurve;
#pragma warning restore IDE0044, RCS1169
            public override IEchoMotion Build()
                => new ZoomCameraByInputMotion(item, animationCurve, sensitivity);
        }

        public ZoomCameraByInputMotion(CameraController item, AnimationCurve zoomCurve, float sensitivity = DefaultSensitivity) : base(item)
        {
            this.sensitivity = sensitivity;
            transform = item.transform;
            this.zoomCurve = zoomCurve;
            maxGradient = zoomCurve.Differentiate(1);
            minGradient = zoomCurve.Differentiate(0);
            startingHeight = transform.position.y;
        }

        private readonly float sensitivity;
        private readonly Transform transform;
        private readonly AnimationCurve zoomCurve;

        private readonly float maxGradient;
        private readonly float minGradient;

        private readonly float startingHeight;

        private float x = 1;

        public override void OnUpdate(float deltaTime)
        {
            x = Mathf.Clamp01(x + (Input.GetAxis("Mouse ScrollWheel") * sensitivity * deltaTime));

            float normalizedGradient = (zoomCurve.Differentiate(x) - minGradient) / (maxGradient - minGradient);

            transform.position = new Vector3(transform.position.x, startingHeight * zoomCurve.Evaluate(x), transform.position.z);
            transform.rotation = Quaternion.Euler(Mathf.Lerp(90, 0, normalizedGradient), transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
