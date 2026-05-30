using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocaMinable : MonoBehaviour
{
    [Header("Configuración del Mineral")]
    public string tipoMineral = "cobre";
    public int cantidadAlRomper = 5;

    [HideInInspector] // Esto oculta la vida en el inspector si quieres que la maneje el script Minar
    public int vidaRoca = 3;

    // Esta es la función que va a llamar el script Minar cuando la roca muera
    public void AgregarAlInventarioYAvisar()
    {
        // Sumamos al inventario
        Inventario.Instance.AgregarMineral(tipoMineral, cantidadAlRomper);
        Debug.Log("¡Roca destruida! Añadidos " + cantidadAlRomper + " de " + tipoMineral);

        // Desaparece la roca del mapa
        Destroy(gameObject);
    }
}
