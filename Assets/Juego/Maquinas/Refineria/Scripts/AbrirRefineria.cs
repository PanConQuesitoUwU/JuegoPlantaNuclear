using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirRefineria : MonoBehaviour
{
    [Header("UI de la Refinería")]
    public GameObject panelRefineriaUI;

    [Header("Configuración")]
    public float distanciaParaAbrir = 6f; // Te lo dejé en 6 para asegurar

    private Transform jugador;
    private bool uiActiva = false;

    void Start()
    {
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
            Debug.Log("🟢 [Refinería]: ¡Jugador detectado de pana en el mapa!");
        }
        else
        {
            Debug.LogError("🔴 [Refinería ERROR]: ¡No encontré a ningún objeto con el Tag 'Player'!");
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Calculamos distancia
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // Si apretas la E, que nos avise a cuántos metros estás de la máquina
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("⌨️ [Refinería]: Apretaste la E. Distancia real a la máquina: " + distancia + " metros. (Necesitas estar a menos de " + distanciaParaAbrir + ")");

            if (distancia <= distanciaParaAbrir)
            {
                ToggleRefineria();
            }
        }

        if (distancia > distanciaParaAbrir && uiActiva)
        {
            CerrarUI();
        }
    }

    void ToggleRefineria()
    {
        uiActiva = !uiActiva;
        if (panelRefineriaUI != null)
        {
            panelRefineriaUI.SetActive(uiActiva);
            Debug.Log("🏭 [Refinería]: Cambiando estado de la UI a: " + uiActiva);

            if (uiActiva)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            Debug.LogError("🔴 [Refinería ERROR]: El panel de UI no está conectado en el Inspector.");
        }
    }

    void CerrarUI()
    {
        uiActiva = false;
        if (panelRefineriaUI != null) panelRefineriaUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("🏭 [Refinería]: Jugador se alejó, UI cerrada automáticamente.");
    }
}
