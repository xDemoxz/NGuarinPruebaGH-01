using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit; //namespace

public class RightHandController_ : MonoBehaviour //class--> declaracion / Monobehabiour--> herencia
{
    //estado
    public InputActionReference JoyStick_North_Reference;

    //reight controller grab
    public XRRayInteractor xrRayInteractor_grab;
    public LineRenderer Linerendered_grab;
    public ActionBasedController actionBaseController_grab;
    public XRInteractorLineVisual xrInteractorLineVisual_grab;

    //right controller tp
    public XRRayInteractor xrRayInteractor_tp;
    public LineRenderer Linerendered_tp;
    public ActionBasedController actionBaseController_tp;
    public XRInteractorLineVisual xrInteractorLineVisual_tp;

    //metodos propios


    private void JoyStickArribaPresionado(InputAction.CallbackContext context)
    {



        xrRayInteractor_grab.enabled = false;

        xrRayInteractor_tp.enabled = true;

        xrInteractorLineVisual_tp.enabled = true;

    }


    private void JoyStickArribaLiberado(InputAction.CallbackContext context)
    {
        xrRayInteractor_grab.enabled = true;

        xrRayInteractor_tp.enabled = false;

        xrInteractorLineVisual_tp.enabled = false;

    }
    private void OnEnable()
    {
        JoyStick_North_Reference.action.performed += JoyStickArribaPresionado;
        JoyStick_North_Reference.action.canceled += JoyStickArribaLiberado;
    }

    private void OnDisable()
    {
        JoyStick_North_Reference.action.performed -= JoyStickArribaPresionado;
        JoyStick_North_Reference.action.canceled -= JoyStickArribaLiberado;
    }



}