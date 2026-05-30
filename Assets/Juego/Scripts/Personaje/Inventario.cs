using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventario : MonoBehaviour
{
    public static Inventario Instance;

    [Header("El Panel Negro que tiene el Grid Layout Group")]
    public Transform gridInventario;

    [Header("Los 4 Moldes de la carpeta Assets (Cubos Azules)")]
    public GameObject prefabHierro;
    public GameObject prefabCobre;
    public GameObject prefabPlomo;
    public GameObject prefabUranio;

    // Aquí guardamos los totales de cada mineral: <nombre, cantidad>
    public Dictionary<string, int> cantidadMinerales = new Dictionary<string, int>();

    // Aquí guardamos el clon que se está mostrando en pantalla: <nombre, objetoUI>
    public Dictionary<string, GameObject> objetosEnInventario = new Dictionary<string, GameObject>();

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

    // El Update queda totalmente vacío para que no use la tecla E aquí
    void Update() { }

    public void AgregarMineral(string nombreMineral, int cantidad)
    {
        nombreMineral = nombreMineral.ToLower().Trim();

        if (cantidadMinerales.ContainsKey(nombreMineral))
        {
            cantidadMinerales[nombreMineral] += cantidad;
            ActualizarTextoUI(nombreMineral);
            Debug.Log("Sumaste " + cantidad + " de " + nombreMineral + ". Total: " + cantidadMinerales[nombreMineral]);
        }
        else
        {
            cantidadMinerales.Add(nombreMineral, cantidad);
            CrearIconoEnUI(nombreMineral);
            Debug.Log("¡Primerizo! Añadiste " + nombreMineral + " al inventario.");
        }
    }

    void CrearIconoEnUI(string nombreMineral)
    {
        GameObject prefabAClonar = null;

        switch (nombreMineral)
        {
            case "hierro": prefabAClonar = prefabHierro; break;
            case "cobre": prefabAClonar = prefabCobre; break;
            case "plomo": prefabAClonar = prefabPlomo; break;
            case "uranio": prefabAClonar = prefabUranio; break;
        }

        if (prefabAClonar != null && gridInventario != null)
        {
            GameObject nuevoItem = Instantiate(prefabAClonar, gridInventario);
            nuevoItem.name = prefabAClonar.name;

            Arrastre scriptDrag = nuevoItem.GetComponent<Arrastre>();
            if (scriptDrag != null)
            {
                scriptDrag.nombreMineral = nombreMineral;
            }

            objetosEnInventario.Add(nombreMineral, nuevoItem);
            ActualizarTextoUI(nombreMineral);
        }
    }

    void ActualizarTextoUI(string nombreMineral)
    {
        if (objetosEnInventario.ContainsKey(nombreMineral))
        {
            GameObject iconoUI = objetosEnInventario[nombreMineral];
            TextMeshProUGUI textoContador = iconoUI.GetComponentInChildren<TextMeshProUGUI>();

            if (textoContador != null)
            {
                textoContador.text = "x" + cantidadMinerales[nombreMineral];
            }
        }
    }

    public int ObtenerTotalMineral(string nombreMineral)
    {
        nombreMineral = nombreMineral.ToLower().Trim();
        if (cantidadMinerales.ContainsKey(nombreMineral))
        {
            return cantidadMinerales[nombreMineral];
        }
        return 0;
    }

    public void EliminarRegistroMineral(string nombreMineral)
    {
        nombreMineral = nombreMineral.ToLower().Trim();

        // Si el mineral existe en nuestros diccionarios, lo sacamos por completo
        if (cantidadMinerales.ContainsKey(nombreMineral))
        {
            cantidadMinerales.Remove(nombreMineral);
        }

        if (objetosEnInventario.ContainsKey(nombreMineral))
        {
            objetosEnInventario.Remove(nombreMineral);
        }

        Debug.Log("🎒 [Inventario]: Registro de " + nombreMineral + " limpiado. Listo para recibir nuevos ores.");
    }
}