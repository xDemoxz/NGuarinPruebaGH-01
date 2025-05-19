/*
 * Copyright (c) 2022-2024 Qualcomm Technologies, Inc. and/or its subsidiaries.
 * All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class SpatialMeshingSampleController : SampleController
    {
        public Toggle CalculateCpuNormalsToggle;
        public Text MeshOpacityValueText;
        public Scrollbar OpacityScrollBar;

        public MeshFilter CustomShaderPrefab;
        public MeshFilter CpuNormalsPrefab;
        private ARMeshManager _meshManager;

        public void Awake()
        {
            _meshManager = FindFirstObjectByType<ARMeshManager>();

            if (_meshManager == null)
            {
                Debug.LogError("Could not find mesh manager. Sample will not work correctly.");
            }
        }

        public override void Start()
        {
            base.Start();

            CalculateCpuNormalsToggle.isOn = _meshManager.normals;
            SwitchPrefab(CalculateCpuNormalsToggle.isOn);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (!GetSubsystemCheck())
            {
                return;
            }

            _meshManager.meshesChanged += OnMeshesChanged;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _meshManager.meshesChanged -= OnMeshesChanged;
        }

        private void OnMeshesChanged(ARMeshesChangedEventArgs args)
        {
            foreach (var meshFilter in args.added)
            {
                Debug.Log("Added meshFilter: " + meshFilter.name);
            }

            foreach (var meshFilter in args.updated)
            {
                Debug.Log("Updated meshFilter: " + meshFilter.name);
            }

            foreach (var meshFilter in args.removed)
            {
                Debug.Log("Removed meshFilter: " + meshFilter.name);
            }

            UpdateMeshOpacity(OpacityScrollBar.value);
        }

        protected override bool CheckSubsystem()
        {
#if !UNITY_EDITOR
            if (!_baseRuntimeFeature.CheckServicesCameraPermissions())
            {
                Debug.LogError("The OpenXR runtime has no camera permissions! ");
                return false;
            }
#endif

            return _meshManager.subsystem?.running ?? false;
        }

        public void ToggleCalculateCpuNormals(bool cpuNormalsEnabled)
        {
            if (_meshManager != null)
            {
                _meshManager.normals = cpuNormalsEnabled;
                var meshes = _meshManager.meshes;
                // Need to destroy the MeshFilters because we want the MeshManager to regenerate them with a new prefab.
                foreach (var mesh in meshes)
                {
                    Destroy(mesh);
                }

                SwitchPrefab(cpuNormalsEnabled);
            }
        }

        public void OnScrollValueChanged(float value)
        {
            SendHapticImpulse(duration: 0.1f);
            UpdateMeshOpacity(value);
        }

        private void UpdateMeshOpacity(float value)
        {
            // Get the meshes from the Mesh Manager
            var meshes = _meshManager.meshes;
            if (meshes == null)
            {
                Debug.LogWarning("No meshes generated yet to change the color.");
                return;
            }
            // Change the alpha in the meshes materials.
            foreach (var mesh in meshes)
            {
                if (mesh != null)
                {
                    var materialColor = mesh.gameObject.GetComponent<Renderer>().material.color;
                    var newAlpha = Math.Clamp(value, 0.1f, 1f);
                    materialColor.a = newAlpha;
                    MeshOpacityValueText.text = newAlpha.ToString("#0.00");
                    mesh.gameObject.GetComponent<Renderer>().material.color = materialColor;
                }
            }
        }

        private void SwitchPrefab(bool cpuNormalsEnabled)
        {
            if (cpuNormalsEnabled && _meshManager.normals)
            {
                // Don't switch to cpu normals prefab unless the normals are enabled on the MeshManager
                _meshManager.meshPrefab = CpuNormalsPrefab;
            }
            else
            {
                _meshManager.meshPrefab = CustomShaderPrefab;
            }
        }
    }
}
