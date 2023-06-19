using UnityEngine;

// skrypt, który sprawia,że Agenci nie "odbijają się" od collision boxu podłoża
[RequireComponent(typeof(BoxCollider))]
public class NoCollisionFloor : MonoBehaviour
{
    private BoxCollider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Agent"))
        {
            AgentScript agent = collision.gameObject.GetComponent<AgentScript>();
            if (agent != null)
            {
                // Sprawdź, czy agent porusza się w dół
                if (agent.moveDirection.y < 0)
                {
                    // Jeżeli tak, tymczasowo wyłącz collider
                    _collider.enabled = false;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Agent"))
        {
            // Po zakończeniu kolizji, włącz collider z powrotem
            _collider.enabled = true;
        }
    }
}
