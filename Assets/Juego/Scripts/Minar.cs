using Unity.VisualScripting;
using UnityEngine;

public class Minar : MonoBehaviour
{
    [Header("Cámara del Juego")]
    public Camera camaraPersonaje;

    [Header("Configuracion de Minado")]
    public float rangoMinado = 3.5f;
    public int dañoPorGolpe = 1;

    // SEGURO ANTIBUG: Para que no pegue ráfagas de golpes en un solo frame
    private bool canMine = true;

    void Update()
    {
        // Usamos GetMouseButtonDown que detecta solo el momento exacto de bajar el dedo
        if (Input.GetMouseButtonDown(0) && canMine)
        {
            RealizarPicotazo();
        }
    }

    void RealizarPicotazo()
    {
        Camera cam = camaraPersonaje != null ? camaraPersonaje : Camera.main;

        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rangoMinado))
        {
            RocaMinable roca = hit.collider.GetComponent<RocaMinable>();

            if (roca != null)
            {
                // Bloqueamos el minado un milisegundo para asegurar un solo golpe limpio
                canMine = false;

                // Le restamos la vida limpiamente usando el daño del Inspector
                roca.vidaRoca -= dañoPorGolpe;
                Debug.Log("¡Picotazo limpio! Vida restante de la roca: " + roca.vidaRoca);

                if (roca.vidaRoca <= 0)
                {
                    roca.AgregarAlInventarioYAvisar();
                }

                // Desbloqueamos el mouse al tiro para el siguiente clic real
                Invoke("ResetMine", 0.1f);
            }
        }
    }

    void ResetMine()
    {
        canMine = true;
    }
}