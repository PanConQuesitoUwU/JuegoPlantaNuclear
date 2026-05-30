using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; // Para leer las cantidades al mover ítems

public class Arrastre : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform padreOriginal;

    [Header("Configuración del Item")]
    [Tooltip("Escribe aquí: cobre, hierro, plomo o uranio")]
    public string nombreMineral; // "cobre", "hierro", o "lingote_cobre", etc.

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        padreOriginal = transform.parent;
        transform.SetParent(transform.root); // Lo mandamos al frente de toda la UI
        canvasGroup.blocksRaycasts = false;  // Transparente al mouse para detectar qué hay abajo
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject objetoAbajo = eventData.pointerCurrentRaycast.gameObject;

        if (objetoAbajo != null)
        {
            // CASO A: Lo soltamos sobre el SlotEntrada de la refinería (y no es un lingote)
            if (objetoAbajo.name == "SlotEntrada" && objetoAbajo.transform.childCount == 0 && !nombreMineral.StartsWith("lingote_"))
            {
                transform.SetParent(objetoAbajo.transform);
                rectTransform.anchoredPosition = Vector2.zero;
                return;
            }

            // CASO B: Lo soltamos de vuelta en el Inventario Principal o en la Grilla
            if (objetoAbajo.name == "PanelInventario" || objetoAbajo.name == "GridInventario")
            {
                Transform grilla = objetoAbajo.name == "GridInventario" ? objetoAbajo.transform : objetoAbajo.transform.Find("GridInventario");
                if (grilla == null) grilla = objetoAbajo.transform;

                // Buscamos si en la mochila ya hay otro item con nuestro mismo nombre para sumarlo
                Transform itemExistente = null;
                foreach (Transform hijo in grilla)
                {
                    Arrastre dragHijo = hijo.GetComponent<Arrastre>();
                    if (dragHijo != null && dragHijo.nombreMineral == this.nombreMineral && hijo.gameObject != this.gameObject)
                    {
                        itemExistente = hijo;
                        break;
                    }
                }

                // Si encontramos un clon del mismo tipo guardado en la mochila, los fusionamos
                if (itemExistente != null)
                {
                    int miCantidad = ObtenerCantidadDeEsteItem();
                    int cantidadEnMochila = 1;

                    TextMeshProUGUI txtMochila = itemExistente.GetComponentInChildren<TextMeshProUGUI>();
                    if (txtMochila != null)
                    {
                        string txtLimpio = txtMochila.text.Replace("x", "").Trim();
                        int.TryParse(txtLimpio, out cantidadEnMochila);
                    }

                    int limiteTope = 64; // Límite por defecto para el inventario
                    if (Refineria.Instance != null) limiteTope = Refineria.Instance.maxStack;

                    if (cantidadEnMochila + miCantidad <= limiteTope)
                    {
                        // Se pueden juntar en un solo cuadradito
                        if (txtMochila != null) txtMochila.text = "x" + (cantidadEnMochila + miCantidad);

                        Destroy(this.gameObject); // Borramos el de la mano porque ya sumó su valor
                        Debug.Log("🎒 [Mochila]: ¡Items acumulados perfectamente en el mismo slot!");
                        return;
                    }
                }

                // Si no hay repetidos o ya se pasa del límite (64), se acomoda normal en un espacio libre
                transform.SetParent(grilla);
                rectTransform.anchoredPosition = Vector2.zero;
                return;
            }
        }

        // Si lo soltamos en cualquier lado incorrecto, vuelve a donde lo tomamos
        transform.SetParent(padreOriginal);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    // Función auxiliar para leer cuánta cantidad tiene este mismo cuadradito puesto encima
    private int ObtenerCantidadDeEsteItem()
    {
        TextMeshProUGUI texto = GetComponentInChildren<TextMeshProUGUI>();
        if (texto != null)
        {
            string limpio = texto.text.Replace("x", "").Trim();
            if (int.TryParse(limpio, out int cant)) return cant;
        }
        return 1;
    }

    
}