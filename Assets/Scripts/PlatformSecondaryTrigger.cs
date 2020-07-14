using UnityEngine;

// Un script minimo, lo asignamos al segundo trigger de las plataformas grandes y es el encargado de decir al GM si el player se va 
// a atascar dentro. El GM impide que el player cambie las dimensiones si este script le da el chivatazo

public class PlatformSecondaryTrigger : MonoBehaviour
{

    GameManager GM;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GM.IsPlayerInsidePlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GM.IsPlayerInsidePlatform = false;
        }
    }
}