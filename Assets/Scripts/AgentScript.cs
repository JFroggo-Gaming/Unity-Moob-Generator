using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentScript : MonoBehaviour
{
    public string agentName; // Nazwa agenta
    public int healthPoints = 3; // Punkty życia agenta
    public float agentSpeed = 5f; // Szybkość poruszania się agenta
    public Slider healthBar; // Pasek zdrowia
    public TMP_Text nameText; // Tekst z nazwą agenta
    public GameObject agentCanvas; // Canvas zawierający UI agenta
    private float lastCollisionTime = -1f; // Czas ostatniej kolizji
    private float collisionDelay = 0.5f; // Opóźnienie między kolizjami
    public ParticleSystem explosionEffect; // Efekt eksplozji

    private bool isSelected = false; // Flaga określająca, czy agent jest zaznaczony
    public Vector3 moveDirection; // Kierunek poruszania się agenta
    private Bounds playAreaBounds; // Granice obszaru, w którym może się poruszać agent

    private void Start()
    {
        healthBar.maxValue = 3;
        healthBar.minValue = 0;
        moveDirection = GetRandomDirection();
        playAreaBounds = AgentGeneratorScript.Instance.playAreaBounds;
        nameText.text = agentName;
        agentCanvas.SetActive(false);

    }

    private void Update()
    {
        MoveAgent();
        healthBar.value = healthPoints;
        healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(Color.white, Color.red, healthPoints / 3f);
        agentCanvas.SetActive(isSelected);
        agentCanvas.transform.LookAt(Camera.main.transform);

        if (healthPoints <= 0)
        {
            Die();
        }
    }

    public void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;
        agentCanvas.SetActive(isSelected);
    }

    public void ChangeDirection(Vector3 newDirection)
    {
        moveDirection = newDirection;
    }

    private void MoveAgent()
    {
        transform.Translate(moveDirection * agentSpeed * Time.deltaTime);

        if (!IsWithinPlayArea())
        {
            moveDirection = GetRandomDirection();
        }

        bool isColliding = false;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, moveDirection, out hit, 0.5f))
        {
            if (hit.collider.CompareTag("Agent") || hit.collider.CompareTag("Wall"))
            {
                isColliding = true;
            }
        }

        if (isColliding)
        {
            bool isValidDirection = false;

            while (!isValidDirection)
            {
                moveDirection = GetRandomDirection();

                if (Physics.Raycast(transform.position, moveDirection, out hit, 0.5f))
                {
                    if (!hit.collider.CompareTag("Agent") && !hit.collider.CompareTag("Wall"))
                    {
                        isValidDirection = true;
                    }
                }
                else
                {
                    isValidDirection = true;
                }
            }
        }
    }

    private Vector3 GetRandomDirection()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        return new Vector3(x, 0, z).normalized;
    }

    private bool IsWithinPlayArea()
    {
        return playAreaBounds.Contains(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Agent") && Time.time - lastCollisionTime >= collisionDelay)
        {
            healthPoints = Mathf.Max(0, healthPoints - 1);
            lastCollisionTime = Time.time;

            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // Odtwarzanie efektu wybuchu
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Odtwarzanie dźwięku
        AudioSource audioSource = GetComponent<AudioSource>();
        GameObject tempAudioSource = new GameObject("TempAudio");
        AudioSource tempAudio = tempAudioSource.AddComponent<AudioSource>();

        // Kopiujemy ustawienia z naszego źródła dźwięku
        tempAudio.clip = audioSource.clip;
        tempAudio.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
        tempAudio.volume = audioSource.volume;
        tempAudio.pitch = audioSource.pitch;
        tempAudio.loop = false;

        // Odtwarzamy dźwięk
        tempAudio.Play();

        // Zniszcz tymczasowy obiekt po odtworzeniu dźwięku
        Destroy(tempAudioSource, audioSource.clip.length);

        AgentGeneratorScript.Instance.RemoveAgent(this);
        Destroy(gameObject);
    }


    private void OnMouseDown()
    {
        isSelected = !isSelected;
    }
}
