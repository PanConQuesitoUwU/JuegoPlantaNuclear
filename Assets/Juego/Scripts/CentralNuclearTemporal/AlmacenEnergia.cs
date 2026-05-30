using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AlmacenEnergia : MonoBehaviour
{
    public static AlmacenEnergia Instance;

    [Header("Configuración de Almacenamiento")]
    public float energiaMaxima = 1000f;
    private float energiaActual = 0f;

    [Header("UI - Pantalla del Módulo")]
    public TextMeshProUGUI textoEnergia;
    public TextMeshProUGUI textoMaximo;
    public Image barraEnergia; // Barra visual (opcional)

    [Header("Velocidad de entrada")]
    public float energiaEntrando = 0f; // Se actualiza desde afuera

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        // Actualizar UI
        ActualizarUI();
    }

    // Se llama desde el Generador de Energía para meter energía
    public void AgregarEnergia(float cantidad)
    {
        float energiaAntes = energiaActual;
        energiaActual = Mathf.Min(energiaActual + cantidad, energiaMaxima);

        float energiaAgregada = energiaActual - energiaAntes;

        if (energiaAgregada > 0)
        {
            Debug.Log("⚡ [Almacén]: +" + energiaAgregada + " energía. Total: " + energiaActual + "/" + energiaMaxima);
        }
    }

    // Se llama cuando algo necesita energía
    public bool ExtraerEnergia(float cantidad)
    {
        if (energiaActual >= cantidad)
        {
            energiaActual -= cantidad;
            Debug.Log("⚡ [Almacén]: -" + cantidad + " energía. Total: " + energiaActual + "/" + energiaMaxima);
            return true;
        }
        else
        {
            Debug.LogWarning("❌ No hay suficiente energía!");
            return false;
        }
    }

    public float ObtenerEnergiaActual()
    {
        return energiaActual;
    }

    public float ObtenerEnergiaMaxima()
    {
        return energiaMaxima;
    }

    public float ObtenerPorcentaje()
    {
        return (energiaActual / energiaMaxima) * 100f;
    }

    void ActualizarUI()
    {
        if (textoEnergia != null)
        {
            textoEnergia.text = "Entrando: " + energiaEntrando.ToString("F1") + " EU/s\n" +
                               "Actual: " + energiaActual.ToString("F0") + " EU";
        }

        if (textoMaximo != null)
        {
            textoMaximo.text = "Máximo: " + energiaMaxima.ToString("F0") + " EU\n" +
                              ObtenerPorcentaje().ToString("F1") + "%";
        }

        // Actualizar barra (opcional)
        if (barraEnergia != null)
        {
            barraEnergia.fillAmount = ObtenerPorcentaje() / 100f;
        }
    }
}
