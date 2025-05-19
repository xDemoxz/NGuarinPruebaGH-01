/*
 * Copyright (c) 2024 Qualcomm Technologies, Inc. and/or its subsidiaries.
 * All rights reserved.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.XR.OpenXR;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class PermissionsUiController : MonoBehaviour
    {
        private enum PermissionType
        {
            ApplicationCamera,
            RuntimeCamera,
            RuntimeOverlay
        }

        [SerializeField]
        private PermissionType RequiredPermission;

        [SerializeField]
        private List<Button> ManagedButtons;

        [SerializeField]
        private GameObject MessageToDisplay;

        private BaseRuntimeFeature _baseRuntimeFeature;
        private bool _permissionGranted;

        void Start()
        {
            _baseRuntimeFeature = OpenXRSettings.Instance.GetFeature<BaseRuntimeFeature>();
            if (_baseRuntimeFeature == null)
            {
                Debug.LogWarning("Base Runtime Feature isn't available.");
                Destroy(this);
                return;
            }

            switch (RequiredPermission)
            {
                case PermissionType.ApplicationCamera:
                    _permissionGranted = Permission.HasUserAuthorizedPermission(Permission.Camera);
                    break;
                case PermissionType.RuntimeCamera:
                    _permissionGranted = _baseRuntimeFeature.CheckServicesCameraPermissions();
                    break;
                case PermissionType.RuntimeOverlay:
                    _permissionGranted = _baseRuntimeFeature.CheckServicesOverlayPermissions();
                    break;
            }

            UpdateButtons();
        }

        void UpdateButtons()
        {
            foreach (var button in ManagedButtons)
            {
                if (button.interactable)
                {
                    button.interactable = _permissionGranted;
                }
            }
            if (!MessageToDisplay.activeSelf)
            {
                MessageToDisplay.SetActive(!_permissionGranted);
            }
        }
    }
}
