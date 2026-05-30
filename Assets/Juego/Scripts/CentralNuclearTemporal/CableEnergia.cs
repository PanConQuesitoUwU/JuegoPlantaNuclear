using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableEnergia : MonoBehaviour
{
    [Header("Conexiones")]
    public Transform puntoSalida; // Del generador
    public Transform puntoEntrada; // De la batería
    public GeneradorEnergia generador;
    public AlmacenEnergia bateria;

    [Header("Visualización")]
    public LineRenderer lineaVisual;
    public Material materialCable;
    public float grosorCable = 0.05f;

    private bool estaConectado = false;

    void Update()
    {
        // Si está conectado, dibuja la línea
        if (estaConectado && lineaVisual != null)
        {
            DibujarCable();
        }
    }

    // Se llama para conectar el cable
    public void Conectar(Transform salida, Transform entrada, GeneradorEnergia gen, AlmacenEnergia bat)
    {
        puntoSalida = salida;
        puntoEntrada = entrada;
        generador = gen;
        bateria = bat;
        estaConectado = true;

        // Crear LineRenderer si no existe
        if (lineaVisual == null)
        {
            lineaVisual = gameObject.AddComponent<LineRenderer>();
            lineaVisual.material = materialCable != null ? materialCable : new Material(Shader.Find("Standard"));
            lineaVisual.startWidth = grosorCable;
            lineaVisual.endWidth = grosorCable;
            lineaVisual.positionCount = 2;
        }

        Debug.Log("🔌 ¡Cable conectado!");
    }

    public void Desconectar()
    {
        estaConectado = false;
        if (lineaVisual != null)
        {
            lineaVisual.positionCount = 0;
        }
        Debug.Log("🔌 Cable desconectado");
    }

    void DibujarCable()
    {
        if (puntoSalida != null && puntoEntrada != null)
        {
            lineaVisual.SetPosition(0, puntoSalida.position);
            lineaVisual.SetPosition(1, puntoEntrada.position);
        }
    }

    public bool EstaConectado()
    {
        return estaConectado;
    }

    public AlmacenEnergia ObtenerBateriaConectada()
    {
        return bateria;
    }
}
