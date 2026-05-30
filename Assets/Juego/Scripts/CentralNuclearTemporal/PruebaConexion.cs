using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaConexion : MonoBehaviour
{
    void Update()
    {
        // Presiona C mirando el cable para conectar
        if (Input.GetKeyDown(KeyCode.C))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10f))
            {
                CableEnergia cable = hit.collider.GetComponent<CableEnergia>();
                if (cable != null)
                {
                    GeneradorEnergia gen = FindObjectOfType<GeneradorEnergia>();
                    if (gen != null)
                    {
                        gen.ConectarCable(cable);
                        Debug.Log("✅ Cable conectado!");
                    }
                }
            }
        }

        // Presiona U para agregar uranio
        if (Input.GetKeyDown(KeyCode.U))
        {
            GeneradorEnergia gen = FindObjectOfType<GeneradorEnergia>();
            if (gen != null) gen.RecibirUranio(1);
        }
    }
}
