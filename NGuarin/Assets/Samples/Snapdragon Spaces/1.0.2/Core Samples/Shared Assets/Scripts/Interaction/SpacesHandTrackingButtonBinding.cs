/*
 * Copyright (c) 2023-2024 Qualcomm Technologies, Inc. and/or its subsidiaries.
 * All rights reserved.
 */

using UnityEngine;
#if UNITY_6000_0_OR_NEWER
using UnityEngine.XR.Interaction.Toolkit.Interactables;
#else
using UnityEngine.XR.Interaction.Toolkit;
#endif

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class SpacesHandTrackingButtonBinding : MonoBehaviour
    {
        [Tooltip("Reference to the XR Simple Interactable if there is one on the Game Object.")]
        public XRSimpleInteractable XrSimpleInteractable;

        [Tooltip("Reference to the Snapping volume Game Object if there is one for this component.")]
        public GameObject SnappingVolumeGameObject;

        private void OnEnable()
        {
            if (InteractionManager.Instance != null)
            {
                HandleInputSwitch(InteractionManager.Instance.InputType);
            }
            InteractionManager.onInputTypeSwitch += HandleInputSwitch;
        }

        private void OnDisable()
        {
            InteractionManager.onInputTypeSwitch -= HandleInputSwitch;
        }

        private void HandleInputSwitch(InputType InputType)
        {
            if (XrSimpleInteractable != null)
            {
                XrSimpleInteractable.enabled = InputType == InputType.HandTracking;
            }

            if (SnappingVolumeGameObject != null)
            {
                SnappingVolumeGameObject.SetActive(InputType == InputType.HandTracking);
            }
        }
    }
}
