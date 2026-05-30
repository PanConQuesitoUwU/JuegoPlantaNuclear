using UnityEngine;
using UnityEngine.EventSystems;

public class Minerales : MonoBehaviour
{
    public enum TipoMineral { Hierro, Cobre, Plomo, Uranio }

    [Header("Configuracion del Ore")]
    public TipoMineral tipo;
    public int cantidadDisponible = 5;

    [Header("Efecto Especial")]
    public bool esRadioactivo = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tipo == TipoMineral.Uranio)
        {
            esRadioactivo = true;
        }
    }

    public void RomperMineral(int daño)
    {
        if (cantidadDisponible <= 0) return;

        cantidadDisponible -= daño;
        Debug.Log("Le pegaste a un yacimiento de: " + tipo + ". Quedan: " + cantidadDisponible);

        if (cantidadDisponible <= 0)
        {
            Debug.Log("El ore de " + tipo + " se agoto.");

            Inventario mochila = FindAnyObjectByType<Inventario>();

            if (mochila != null)
            {

                mochila.AgregarMineral(tipo.ToString(), 1);
            }

            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
