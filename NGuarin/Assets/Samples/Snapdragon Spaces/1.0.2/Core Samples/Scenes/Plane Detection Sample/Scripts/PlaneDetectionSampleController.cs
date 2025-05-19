/*
 * Copyright (c) 2022-2024 Qualcomm Technologies, Inc. and/or its subsidiaries.
 * All rights reserved.
 */

using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.OpenXR;

namespace Qualcomm.Snapdragon.Spaces.Samples
{
    public class PlaneDetectionSampleController : SampleController
    {
        public Toggle EnableConvexHullToggle;
        private ARPlaneManager _planeManager;
        private SpacesARPlaneManagerConfig _planeManagerConfig;

        public override void Start()
        {
            base.Start();
            if (_planeManagerConfig != null)
            {
                EnableConvexHullToggle.isOn = _planeManagerConfig.ConvexHullEnabled;
            }
        }

        public void OnToggleConvexHull(bool inValue)
        {
            if (_planeManagerConfig != null)
            {
                _planeManagerConfig.ConvexHullEnabled = inValue;
            }
        }

        public void Awake()
        {
            _planeManager = FindFirstObjectByType<ARPlaneManager>();
            _planeManagerConfig = FindFirstObjectByType<SpacesARPlaneManagerConfig>();
        }

        protected override bool CheckSubsystem()
        {
            return _planeManager.subsystem?.running ?? false;
        }
    }
}
