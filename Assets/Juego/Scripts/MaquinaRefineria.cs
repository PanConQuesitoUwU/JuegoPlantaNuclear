using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaquinaRefineria : MonoBehaviour
{
    private GameObject panelRefineria;
    private Transform jugador;
    private bool jugadorCerca = false;

    [Header("Configuración de Cercanía")]
    public float distanciaParaInteractuar = 4.5f;

    void Start()
    {
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null) jugador = jugadorObj.transform;

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            Transform t = canvas.transform.Find("PanelRefineria");
            if (t != null) panelRefineria = t.gameObject;
        }
    }

    void Update()
    {
        if (jugador == null || panelRefineria == null) return;

        // Calculamos la distancia entre el jugador y la máquina en el suelo
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaParaInteractuar)
        {
            if (!jugadorCerca)
            {
                jugadorCerca = true;
                Debug.Log("🟢 [Refinería]: Estás al lado de la máquina.");
            }

            // 🚨 SOLUCIÓN AL ERROR ROJO:
            // Si el inventario existe, le preguntamos si su panel principal está activo en la escena.
            if (Inventario.Instance != null)
            {
                // Buscamos si la interfaz de tu mochila está abierta. 
                // Si tu variable era con mayúscula, probamos InventarioActivo.
                // Como descarte definitivo que no falla, usamos el estado de su propio componente.
                bool inventarioAbierto = false;

                // Intentamos leer la variable pública de tu script antiguo
                // Si te vuelve a salir error aquí, me avisas para usar el truco del GameObject directo.
                inventarioAbierto = Inventario.Instance.gameObject.activeInHierarchy;

                // Si la mochila está abierta, la refinería se abre. Si se cierra, esta también.
                if (panelRefineria.activeSelf != inventarioAbierto)
                {
                    panelRefineria.SetActive(inventarioAbierto);
                    Debug.Log("🏭 [Sincronización]: Refinería copió el estado del inventario -> " + inventarioAbierto);
                }
            }
        }
        else
        {
            // Si te alejas de la máquina, te cerramos la refinería a la fuerza por seguridad
            if (jugadorCerca)
            {
                jugadorCerca = false;
                panelRefineria.SetActive(false);
                Debug.Log("🔴 [Refinería]: Te alejaste, panel cerrado.");
            }
        }
    }
}