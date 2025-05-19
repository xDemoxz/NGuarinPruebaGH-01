/*
 * Copyright (c) 2022-2024 Qualcomm Technologies, Inc. and/or its subsidiaries.
 * All rights reserved.
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Management;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class FloatingPanelController : MonoBehaviour
    {
        [Tooltip("Whether the Game Object should follow or not the camera movement.")]
        public bool FollowGaze = true;

        [Tooltip("Lock the Y position of the panel to the Y position of the Main Camera")]
        public bool LockYPosition = true;

        [Tooltip("Target distance between the camera and the Game Object.")]
        public float TargetDistance = 1.0f;

        [Tooltip("Smoothness on the movement following the Game Object.")]
        public float MovementSmoothness = 0.2f;

        [Tooltip("Vertical follow threshold.")]
        public float VerticalBias = 0.8f;

        [Tooltip("Horizontal follow threshold.")]
        public float HorizontalBias = 0.5f;

        public Toggle mainMenuToggle;
        public Toggle infoMenuToggle;

        private Transform _arCameraTransform;
        private Camera _arCamera;
        private XRControllerManager _xrControllerManager;

        public void SwitchToScene(string name)
        {
            _xrControllerManager?.SendHapticImpulse(controllerHand: ControllerHand.LeftController);
            _xrControllerManager?.SendHapticImpulse(controllerHand: ControllerHand.RightController);

            SceneManager.LoadScene(name);
        }

        private void Start()
        {
            _arCamera = OriginLocationUtility.GetOriginCamera();
            _arCameraTransform = _arCamera.transform;
            _xrControllerManager ??= FindFirstObjectByType<XRControllerManager>(FindObjectsInactive.Include);
        }

        private void Update()
        {
            if (FollowGaze)
            {
                AdjustPanelPosition();
            }
        }

        public void MainMenuPinUI(bool pin)
        {
            FollowGaze = !pin;
            if (infoMenuToggle != null)
            {
                infoMenuToggle.isOn = pin;
            }
        }

        public void InfoMenuPinUI(bool pin)
        {
            FollowGaze = !pin;
            mainMenuToggle.isOn = pin;
        }

        // Adjusts the position of the Panel if the gaze moves outside of the inner rectangle of the FOV,
        //  which is half the length in both axis.
        private void AdjustPanelPosition()
        {
            var headPosition = _arCameraTransform.position;
            var gazeDirection = _arCameraTransform.forward;
            var myPosition = transform.position;
            var direction = (myPosition - headPosition).normalized;
            var targetPosition = headPosition + (gazeDirection * TargetDistance);
            var targetDirection = (targetPosition - headPosition).normalized;
            var eulerAngles = Quaternion.LookRotation(direction).eulerAngles;
            var targetEulerAngles = Quaternion.LookRotation(targetDirection).eulerAngles;
            var verticalHalfAngle = _arCamera.fieldOfView * VerticalBias;
            eulerAngles.x += GetAdjustedDelta(targetEulerAngles.x - eulerAngles.x, verticalHalfAngle);
            var horizontalHalfAngle = _arCamera.fieldOfView * HorizontalBias * _arCamera.aspect;
            eulerAngles.y += GetAdjustedDelta(targetEulerAngles.y - eulerAngles.y, horizontalHalfAngle);
            targetPosition = headPosition + (Quaternion.Euler(eulerAngles) * Vector3.forward * TargetDistance);
            var newTargetWithYHeadLocked = new Vector3(targetPosition.x, headPosition.y, targetPosition.z);

            // Calculate new position
            var newPosition = Vector3.Lerp(myPosition, LockYPosition ? newTargetWithYHeadLocked : targetPosition, MovementSmoothness);
            var newRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newPosition - headPosition), MovementSmoothness);
            transform.SetPositionAndRotation(newPosition, newRotation);
        }

        // Returns the normalized delta to a certain threshold, if it exceeds that threshold. Otherwise return 0.
        private float GetAdjustedDelta(float angle, float threshold)
        {
            // Normalize angle to be between 0 and 360.
            angle = ((540f + angle) % 360f) - 180f;
            if (Mathf.Abs(angle) > threshold)
            {
                return -angle / Mathf.Abs(angle) * (threshold - Mathf.Abs(angle));
            }

            return 0f;
        }
    }
}
