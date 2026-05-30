using UnityEngine;

public class Camara : MonoBehaviour
{
    public float sensibilidadMouse = 100f;
    public Transform cuerpoJugador;

    float rotacionX = 0f;

    // Candado para congelar la cámara desde el ControladorUI
    public static bool puedeMoverse = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        puedeMoverse = true;
    }

    // 🚨 CAMBIO: Usamos LateUpdate para que la cámara se mueva JUSTO DESPUÉS 
    // de que el personaje calcule su física y movimiento. Esto quita el 99% de los tirones.
    void LateUpdate()
    {
        // Si la UI está abierta, se queda tiesa
        if (!puedeMoverse) return;

        // 🚨 CAMBIO: Para corregir los saltos rápidos en First Person, le quitamos el "* Time.deltaTime"
        // ya que Unity maneja internamente el GetAxis raw del mouse de forma fluida.
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * 0.1f;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * 0.1f;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        cuerpoJugador.Rotate(Vector3.up * mouseX);
    }
}