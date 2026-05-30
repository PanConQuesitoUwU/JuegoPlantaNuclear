using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Refineria : MonoBehaviour
{
    public static Refineria Instance;

    [Header("Slots de la Refinería (Arrastra los de la Hierarchy)")]
    public Transform slotEntrada;
    public Transform slotSalida;

    [Header("Los Moldes de los Lingotes (Arrastra los de la carpeta Assets)")]
    public GameObject prefabLingoteHierro;
    public GameObject prefabLingoteCobre;
    public GameObject prefabLingotePlomo;
    public GameObject prefabLingoteUranio;

    [Header("Configuración de Almacenamiento")]
    public int maxStack = 64;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void CookMinerals()
    {
        if (slotEntrada.childCount == 0)
        {
            Debug.LogWarning("¡No hay ningún mineral en la entrada, hrmno!");
            return;
        }

        GameObject mineralEnEntrada = slotEntrada.GetChild(0).gameObject;
        Arrastre scriptDragEntrada = mineralEnEntrada.GetComponent<Arrastre>();

        if (scriptDragEntrada == null) return;

        string tipoMineral = scriptDragEntrada.nombreMineral.ToLower().Trim();

        int cantidadACocinar = 1;
        TextMeshProUGUI textoContadorEntrada = mineralEnEntrada.GetComponentInChildren<TextMeshProUGUI>();

        if (textoContadorEntrada != null)
        {
            string textoLimpio = textoContadorEntrada.text.Replace("x", "").Trim();
            if (int.TryParse(textoLimpio, out int resultado))
            {
                cantidadACocinar = resultado;
            }
        }

        GameObject lingoteExistente = null;
        if (slotSalida.childCount > 0)
        {
            GameObject objetoEnSalida = slotSalida.GetChild(0).gameObject;
            Arrastre scriptDragSalida = objetoEnSalida.GetComponent<Arrastre>();

            if (scriptDragSalida != null && scriptDragSalida.nombreMineral == "lingote_" + tipoMineral)
            {
                lingoteExistente = objetoEnSalida;
            }
            else
            {
                Debug.LogWarning("¡El slot de salida está ocupado por otro material, waxo!");
                return;
            }
        }

        // Le avisamos al inventario que limpie el registro del BRUTO para poder volver a minar
        if (Inventario.Instance != null)
        {
            Inventario.Instance.EliminarRegistroMineral(tipoMineral);
        }

        if (lingoteExistente != null)
        {
            TextMeshProUGUI textoContadorSalida = lingoteExistente.GetComponentInChildren<TextMeshProUGUI>();
            if (textoContadorSalida != null)
            {
                string textoLimpioSalida = textoContadorSalida.text.Replace("x", "").Trim();
                int cantidadActual = 1;

                if (int.TryParse(textoLimpioSalida, out int res))
                {
                    cantidadActual = res;
                }

                int nuevaCantidad = cantidadActual + cantidadACocinar;

                if (nuevaCantidad > maxStack)
                {
                    Debug.LogWarning("¡Supera el límite de stack en la máquina!");
                    return;
                }

                textoContadorSalida.text = "x" + nuevaCantidad;
                Destroy(mineralEnEntrada);
                Debug.Log("🏭 [Refinería]: Acumulados " + cantidadACocinar + " lingotes en la máquina.");
            }
        }
        else
        {
            GameObject lingoteAClonar = null;
            switch (tipoMineral)
            {
                case "hierro": lingoteAClonar = prefabLingoteHierro; break;
                case "cobre": lingoteAClonar = prefabLingoteCobre; break;
                case "plomo": lingoteAClonar = prefabLingotePlomo; break;
                case "uranio": lingoteAClonar = prefabLingoteUranio; break;
            }

            if (lingoteAClonar != null)
            {
                Destroy(mineralEnEntrada);

                GameObject nuevoLingote = Instantiate(lingoteAClonar, slotSalida);
                nuevoLingote.name = lingoteAClonar.name;

                Arrastre dragLingote = nuevoLingote.GetComponent<Arrastre>();
                if (dragLingote != null)
                {
                    dragLingote.nombreMineral = "lingote_" + tipoMineral; // Ojo: queda guardado como "lingote_hierro", etc.
                }

                nuevoLingote.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                TextMeshProUGUI textoContadorSalida = nuevoLingote.GetComponentInChildren<TextMeshProUGUI>();
                if (textoContadorSalida != null)
                {
                    textoContadorSalida.text = "x" + cantidadACocinar;
                }
                Debug.Log("🏭 [Refinería]: Creado stack de lingotes en salida.");
            }
        }
    }
}