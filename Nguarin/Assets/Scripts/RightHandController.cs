using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandController : MonoBehaviour
{
    //grab
    //public XRController xrController_grab; 
    public ActionBasedController ActionBasedController_grab;
    public XRRayInteractor xrRayInteractor_grab;
    public LineRenderer lineRenderer_grab;
    public XRInteractorLineVisual xrInteractorLineVisual_grab;

    //teleport
    //public XRController xrController_teleport;
    public ActionBasedController ActionBasedController_teleport;
    public XRRayInteractor xrRayInteractor_teleport;
    public LineRenderer lineRenderer_teleport;
    public XRInteractorLineVisual xrInteractorLineVisual_teleport;

    public InputActionReference Joystick_North_Ref; //estado

    [ContextMenu("JoystickArribaPresionado")]

    //metodos propios
    private void JoystickArribaPresionado(InputAction.CallbackContext context)
    {
        xrRayInteractor_grab.enabled = false;

        xrRayInteractor_teleport.enabled = true;
        xrInteractorLineVisual_teleport.enabled = true;
    }
    private void JoystickArribaLiberado(InputAction.CallbackContext context) 
    {
        Invoke("JoystickArribaLiberado_Invoke", 0.005f); // espera de 0.005
    }

    private void JoystickArribaLiberado_Invoke()
    {
        xrRayInteractor_grab.enabled = true;

        xrRayInteractor_teleport.enabled = false;
        xrInteractorLineVisual_teleport.enabled = false;
    }

    //metodos heredados
    private void OnEnable()
    {
        Joystick_North_Ref.action.performed += JoystickArribaPresionado;
        Joystick_North_Ref.action.canceled += JoystickArribaLiberado;
    }
    private void OnDisable()
    {
        Joystick_North_Ref.action.performed -= JoystickArribaPresionado;
        Joystick_North_Ref.action.canceled -= JoystickArribaLiberado;
    }

}