/*
 * Copyright (c) 2023-2024 Qualcomm Technologies, Inc. and/or its subsidiaries.
 * All rights reserved.
 */

#if QCHT_UNITY_CORE
using QCHT.Interactions.Core;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    [RequireComponent(typeof(Button))]
    public class HandTrackingSampleChecker : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        private void OnValidate()
        {
            _button = _button ? _button : GetComponent<Button>();
        }

        private void Start()
        {
#if !QCHT_UNITY_CORE
            _button.interactable = false;
#elif QCHT_UNITY_CORE && UNITY_EDITOR
            if (XRHandTrackingSimulationSettings.Instance.enabled)
            {
                return;
            }

            Debug.LogWarning("To use Editor hand tracking simulation, enable it at Project Settings > XR Plug-in Management > Hand Tracking Simulation");
            _button.interactable = false;
#endif
        }
    }
}
