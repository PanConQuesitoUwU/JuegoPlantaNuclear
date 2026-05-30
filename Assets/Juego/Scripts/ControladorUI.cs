using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorUI : MonoBehaviour
{
    [Header("Paneles de Interfaz")]
    public GameObject panelInventario;  // Arrastra aquí tu PanelInventario
    public GameObject panelRefineria;   // Arrastra aquí tu PanelRefineria

    [Header("Configuración de la Vista")]
    public Camera camaraJugador;        // Arrastra la cámara principal
    public float distanciaInteraccion = 3.5f; // Qué tan cerca tienes que estar

    private bool inventarioAbierto = false;

    void Start()
    {
        if (panelInventario != null) panelInventario.SetActive(false);
        if (panelRefineria != null) panelRefineria.SetActive(false);

        inventarioAbierto = false;
        DesbloquearMouse(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventarios();
        }
    }

    void ToggleInventarios()
    {
        inventarioAbierto = !inventarioAbierto;

        // --- SI VAMOS A CERRAR TODO ---
        if (!inventarioAbierto)
        {
            if (panelInventario != null) panelInventario.SetActive(false);
            if (panelRefineria != null) panelRefineria.SetActive(false);
            DesbloquearMouse(false);
            return;
        }

        // --- SI VAMOS A ABRIR, REVISAMOS QUÉ ESTAMOS MIRANDO ---
        if (camaraJugador != null)
        {
            Ray rayo = camaraJugador.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(rayo, out hit, distanciaInteraccion))
            {
                // Buscamos si chocó con la refinería (revisando si el objeto tiene el script Refineria)
                if (hit.collider.GetComponent<Refineria>() != null)
                {
                    if (panelInventario != null) panelInventario.SetActive(true);
                    if (panelRefineria != null) panelRefineria.SetActive(true);
                    DesbloquearMouse(true);
                    return;
                }
            }
        }

        // Si no estamos mirando la refinería, solo abrimos el inventario a secas
        if (panelInventario != null) panelInventario.SetActive(true);
        if (panelRefineria != null) panelRefineria.SetActive(false);
        DesbloquearMouse(true);
    }

    void DesbloquearMouse(bool liberar)
    {
        if (liberar)
        {
            Cursor.lockState = CursorLockMode.None; // Libera la flecha
            Cursor.visible = true;
            Camara.puedeMoverse = false; // 🚨 CONGELA LA CÁMARA
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Esconde la flecha
            Cursor.visible = false;
            Camara.puedeMoverse = true; // 🚨 DESCONGELA LA CÁMARA
        }
    }
}