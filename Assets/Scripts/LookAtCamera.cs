using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject textObject; // Obiekt tekstu do obrócenia

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
        
        
    }
}
