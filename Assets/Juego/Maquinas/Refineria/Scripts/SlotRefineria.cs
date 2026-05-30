using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotRefineria : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject objetoSoltado = eventData.pointerDrag;
        if (objetoSoltado == null)
        {
            objetoSoltado = EventSystem.current.currentSelectedGameObject;
        }

        if (objetoSoltado != null)
        {
            // Buscamos tu script Arrastrar (que antes tenías como Arrastre)
            Arrastre scriptMineral = objetoSoltado.GetComponent<Arrastre>();

            if (scriptMineral != null)
            {
                // El mineral se vuelve hijo del slot en la UI
                objetoSoltado.transform.SetParent(transform);
                objetoSoltado.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Debug.Log("¡Mineral puesto en el slot! Mandando a cocinar...");

                // Llama al cerebro de la refinería nueva para que procese con el botón o directo
                if (Refineria.Instance != null)
                {
                    Refineria.Instance.CookMinerals();
                }
            }
        }
    }
}