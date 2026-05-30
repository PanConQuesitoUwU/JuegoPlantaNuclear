using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temporal : MonoBehaviour
{
    void Update()
    {
        // Presiona E para agregar 50 de energía
        if (Input.GetKeyDown(KeyCode.R))
        {
            AlmacenEnergia.Instance.AgregarEnergia(50);
            AlmacenEnergia.Instance.energiaEntrando = 50;
        }

        // Presiona Q para quitar 30 de energía
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AlmacenEnergia.Instance.ExtraerEnergia(30);
        }
    }
}
