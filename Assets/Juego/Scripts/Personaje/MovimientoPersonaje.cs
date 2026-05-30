using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MovimientoPersonaje : MonoBehaviour
{
    public CharacterController controller;
    public float velocidad = 12f;
    public float gravedad = -9.81f;
    public float fuerzaSalto = 3f;

    public Transform verificadorSuelo;
    public float distanciaSuelo = 0.4f;
    public LayerMask mascaraSuelo;

    Vector3 velocidadCaida;
    bool estaEnElSuelo;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
 
        estaEnElSuelo = Physics.CheckSphere(verificadorSuelo.position, distanciaSuelo, mascaraSuelo);

        if (estaEnElSuelo && velocidadCaida.y < 0)
        {
            velocidadCaida.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;

        controller.Move(mover * velocidad * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && estaEnElSuelo)
        {
            velocidadCaida.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
        }

        velocidadCaida.y += gravedad * Time.deltaTime;
        controller.Move(velocidadCaida * Time.deltaTime);
    }
}
