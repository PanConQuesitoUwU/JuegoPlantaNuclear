using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonFisico : MonoBehaviour
{
    [Header("Lo que se va a mover")]
    public Transform barrasUranio;

    [Header("Configuración")]
    public float alturaAlzada = 3f;
    public float velocidadMovimiento = 2f;

    [Header("Raycast")]
    public Camera camaraJugador;
    public float rangoDeteccion = 10f;

    private Vector3 posicionOriginal;
    private Vector3 posicionAlzada;
    private bool estanArriba = false;
    private bool seEstaMoviendo = false;

    void Start()
    {
        if (barrasUranio == null)
        {
            Debug.LogError("❌ No asignaste las barras de uranio");
            return;
        }

        if (camaraJugador == null)
            camaraJugador = Camera.main;

        posicionOriginal = barrasUranio.position;
        posicionAlzada = posicionOriginal + Vector3.up * alturaAlzada;
    }

    void Update()
    {
        // Detectamos si miramos al botón
        if (Input.GetKeyDown(KeyCode.P))
        {
            DetectarYMoverBarras();
        }
    }

    void DetectarYMoverBarras()
    {
        Ray ray = camaraJugador.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Si hacemos raycast y golpeamos este botón
        if (Physics.Raycast(ray, out hit, rangoDeteccion))
        {
            // Verificamos que sea ESTE botón el que golpeamos
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("✅ ¡Mirás el botón y presionaste P!");

                // Si no se está moviendo, iniciamos el movimiento
                if (!seEstaMoviendo)
                {
                    if (estanArriba)
                    {
                        StartCoroutine(MoverBarras(posicionAlzada, posicionOriginal));
                        estanArriba = false;
                        Debug.Log("⬇️ Bajando barras...");
                    }
                    else
                    {
                        StartCoroutine(MoverBarras(posicionOriginal, posicionAlzada));
                        estanArriba = true;
                        Debug.Log("⬆️ Subiendo barras...");
                    }
                }
            }
        }
    }

    System.Collections.IEnumerator MoverBarras(Vector3 desde, Vector3 hasta)
    {
        seEstaMoviendo = true;
        float tiempoTranscurrido = 0f;
        float duracion = 1f / velocidadMovimiento;

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float progreso = tiempoTranscurrido / duracion;

            barrasUranio.position = Vector3.Lerp(desde, hasta, progreso);

            yield return null;
        }

        barrasUranio.position = hasta;
        seEstaMoviendo = false;
    }
}
