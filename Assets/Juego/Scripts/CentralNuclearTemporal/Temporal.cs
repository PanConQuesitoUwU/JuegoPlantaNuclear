using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temporal : MonoBehaviour
{
    void Update()
    {
        // Presiona U para agregar 1 uranio al generador
        if (Input.GetKeyDown(KeyCode.U))
        {
            GeneradorEnergia gen = FindObjectOfType<GeneradorEnergia>();
            if (gen != null)
            {
                gen.RecibirUranio(1);
            }
        }
    }
}
