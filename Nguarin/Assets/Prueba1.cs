using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Prueba1 : MonoBehaviour
{

    private void Awake()
    {
        MensajeInvoke();
    }



    public void MensajeInvoke()
    {
        Invoke("Mensaje", 5f);
    }
    

    public void Mensaje()
    {
        print("Hola mundo");
    }

    

}
