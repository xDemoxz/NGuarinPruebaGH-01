/*
 * Copyright (c) 2023-2024 Qualcomm Technologies, Inc. and/or its subsidiaries.
 * All rights reserved.
 */

#if QCHT_UNITY_CORE
using QCHT.Interactions.Core;
#endif

using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID && !UNITY_EDITOR
using System.Linq;
using UnityEngine.Android;
using UnityEngine.XR.OpenXR;
#endif

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    [RequireComponent(typeof(Button))]
    public class HandTrackingSampleChecker : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        private void OnEnable()
        {
            _button.interactable = CheckRuntimeCameraPermissions();
        }

        private void OnValidate()
        {
            _button = _button ? _button : GetComponent<Button>();
        }

        private void Start()
        {
#if !QCHT_UNITY_CORE
            _button.interactable = false;
#elif QCHT_UNITY_CORE && UNITY_EDITOR
            if (HandTrackingSimulationSettings.Instance.enabled)
            {
                return;
            }

            Debug.LogWarning("To use Editor hand tracking simulation, enable it at Project Settings > XR Plug-in Management > Hand Tracking Simulation");
            _button.interactable = false;
#endif

        }

        private bool CheckRuntimeCameraPermissions()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            var runtimeChecker = new AndroidJavaClass("com.qualcomm.snapdragon.spaces.serviceshelper.RuntimeChecker");

            if ( !runtimeChecker.CallStatic<bool>("CheckCameraPermissions", new object[] { activity }) )
            {
                Debug.LogError("The OpenXR runtime has no camera permissions! Hand Tracking feature disabled.");
                return false;
            }
#endif
            return true;
        }
    }
}
