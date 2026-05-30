using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneradorEnergia : MonoBehaviour
{
    [Header("Configuración de Procesamiento")]
    public string mineralRequerido = "uranio";
    public float energiaPorUnidad = 50f;
    public float tiempoProcesamiento = 3f;

    [Header("UI - Pantalla del Generador")]
    public TextMeshProUGUI textoEstado;
    public TextMeshProUGUI textoProgreso;
    public Image barraProgreso;

    [Header("Punto de salida")]
    public Transform salidaEnergia;

    private int uranioActual = 0;
    private float tiempoTranscurrido = 0f;
    private bool estaProcesando = false;
    private List<CableEnergia> cablesConectados = new List<CableEnergia>();

    void Update()
    {
        ActualizarUI();

        // Si hay uranio y no está procesando, inicia
        if (uranioActual > 0 && !estaProcesando && HayBateriaConEspacio())
        {
            StartCoroutine(ProcesarUranio());
        }
    }

    // Verifica si hay alguna batería con espacio
    bool HayBateriaConEspacio()
    {
        foreach (CableEnergia cable in cablesConectados)
        {
            if (cable.EstaConectado())
            {
                AlmacenEnergia bat = cable.ObtenerBateriaConectada();
                if (bat.ObtenerEnergiaActual() < bat.ObtenerEnergiaMaxima())
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Conecta un cable al generador
    public void ConectarCable(CableEnergia cable)
    {
        if (!cablesConectados.Contains(cable))
        {
            cablesConectados.Add(cable);
            cable.Conectar(salidaEnergia, cable.puntoEntrada, this, cable.bateria);
            Debug.Log("✅ Cable conectado al generador");
        }
    }

    public void DesconectarCable(CableEnergia cable)
    {
        cablesConectados.Remove(cable);
        cable.Desconectar();
        Debug.Log("❌ Cable desconectado del generador");
    }

    public void RecibirUranio(int cantidad)
    {
        uranioActual += cantidad;
        Debug.Log("⚛️ [Generador]: Recibido " + cantidad + " uranio. Total: " + uranioActual);
    }

    System.Collections.IEnumerator ProcesarUranio()
    {
        estaProcesando = true;
        tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoProcesamiento)
        {
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Procesamiento completo - distribuye a baterías conectadas
        float energiaGenerada = energiaPorUnidad;
        float energiaDistribuida = 0f;

        foreach (CableEnergia cable in cablesConectados)
        {
            if (cable.EstaConectado())
            {
                AlmacenEnergia bat = cable.ObtenerBateriaConectada();
                bat.AgregarEnergia(energiaGenerada);
                energiaDistribuida += energiaGenerada;
            }
        }

        if (energiaDistribuida > 0)
        {
            AlmacenEnergia.Instance.energiaEntrando = energiaDistribuida / tiempoProcesamiento;
        }

        uranioActual--;
        estaProcesando = false;

        Debug.Log("⚛️ [Generador]: Distribuida " + energiaDistribuida + " energía");
    }

    void ActualizarUI()
    {
        if (textoEstado != null)
        {
            if (estaProcesando)
            {
                textoEstado.text = "Estado: PROCESANDO\nUranio: " + uranioActual + "\nCables: " + cablesConectados.Count;
            }
            else if (uranioActual > 0 && HayBateriaConEspacio())
            {
                textoEstado.text = "Estado: EN ESPERA\nUranio: " + uranioActual + "\nCables: " + cablesConectados.Count;
            }
            else
            {
                textoEstado.text = "Estado: DETENIDO\nUranio: " + uranioActual + "\nCables: " + cablesConectados.Count;
            }
        }

        if (estaProcesando && textoProgreso != null)
        {
            float porcentaje = (tiempoTranscurrido / tiempoProcesamiento) * 100f;
            textoProgreso.text = "Progreso: " + porcentaje.ToString("F1") + "%";

            if (barraProgreso != null)
            {
                barraProgreso.fillAmount = tiempoTranscurrido / tiempoProcesamiento;
            }
        }
        else if (textoProgreso != null)
        {
            textoProgreso.text = "Progreso: 0%";
            if (barraProgreso != null) barraProgreso.fillAmount = 0f;
        }
    }
}