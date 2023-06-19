using UnityEngine;
using System.Collections.Generic;

public class AgentGeneratorScript : MonoBehaviour
{
    public static AgentGeneratorScript Instance { get; private set; }

    public GameObject agentPrefab; // Prefab agenta
    public float minSpawnTime = 2f; // Minimalny czas między generowaniem agentów
    public float maxSpawnTime = 6f; // Maksymalny czas między generowaniem agentów
    public int minStartingAgents = 3; // Minimalna liczba początkowych agentów
    public int maxStartingAgents = 5; // Maksymalna liczba początkowych agentów
    public int maxAgents = 30; // Maksymalna liczba agentów na scenie
    public Transform wallContainer; // Kontener zawierający ściany
    public GameObject groundObject; // Obiekt reprezentujący podłoże
    public float agentSpeed = 5f; // Szybkość poruszania się agentów

    [HideInInspector]
    public Bounds playAreaBounds; // Granice obszaru, w którym mogą się poruszać agenci

    private float nextSpawnTime; // Czas następnego generowania agenta
    private List<AgentScript> activeAgents; // Lista aktywnych agentów
    private List<Color> availableColors; // Lista dostępnych kolorów

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            activeAgents = new List<AgentScript>();
            availableColors = new List<Color>(); // Inicjalizacja listy dostępnych kolorów
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CalculatePlayAreaBounds();
        GenerateStartingAgents();
        nextSpawnTime = Time.time + GetRandomSpawnTime();
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            GenerateAgent();
            nextSpawnTime = Time.time + GetRandomSpawnTime();
        }
    }

    private void GenerateStartingAgents()
    {
        int startingAgents = Random.Range(minStartingAgents, maxStartingAgents + 1);
        for (int i = 0; i < startingAgents; i++)
        {
            GenerateAgent();
        }
    }

    private void GenerateAgent()
    {
        if (activeAgents.Count >= maxAgents)
        {
            return; // Osiągnięto maksymalną liczbę agentów
        }

        Vector3 spawnPosition = GetRandomSpawnPositionOnGround();
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, agentPrefab.transform.localScale.y / 2);

        if (colliders.Length > 0)
        {
            return; // Koliduje z innym agentem lub ścianą, nie można wygenerować
        }

        GameObject agentObject = Instantiate(agentPrefab, spawnPosition, Quaternion.identity);
        AgentScript agent = agentObject.GetComponent<AgentScript>();

        agent.agentName = "Moob " + Random.Range(0, 1000);
        agent.healthPoints = 3;
        agent.agentSpeed = agentSpeed;

        // Przypisanie losowego koloru agentowi
        Color agentColor = GetRandomAgentColor();
        agent.GetComponent<Renderer>().material.color = agentColor;

        agentObject.transform.parent = transform;
        activeAgents.Add(agent);

        AgentManager.Instance.AddAgent(agent);
        IncreaseAgentCount(); // Zwiększamy licznik agentów po wygenerowaniu nowego agenta
    }

    public void RemoveAgent(AgentScript agent)
    {
        activeAgents.Remove(agent);
        DecreaseAgentCount(); // Zmniejszamy licznik agentów po usunięciu agenta
    }

    private float GetRandomSpawnTime()
    {
        return Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void CalculatePlayAreaBounds()
    {
        Renderer[] wallRenderers = wallContainer.GetComponentsInChildren<Renderer>();
        Renderer groundRenderer = groundObject.GetComponent<Renderer>();

        if (wallRenderers.Length > 0)
        {
            playAreaBounds = wallRenderers[0].bounds;
            for (int i = 1; i < wallRenderers.Length; i++)
            {
                playAreaBounds.Encapsulate(wallRenderers[i].bounds);
            }
        }

        if (groundRenderer != null)
        {
            playAreaBounds.Encapsulate(groundRenderer.bounds);
        }
    }

    private Vector3 GetRandomSpawnPositionOnGround()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(playAreaBounds.min.x, playAreaBounds.max.x),
            groundObject.transform.position.y + agentPrefab.transform.localScale.y / 2,
            Random.Range(playAreaBounds.min.z, playAreaBounds.max.z)
        );

        return spawnPosition;
    }

    public void IncreaseAgentCount()
    {
        if (activeAgents.Count >= maxAgents)
        {
            nextSpawnTime = float.MaxValue; // Wyłącz generowanie agentów, gdy osiągnięto maksymalną liczbę
        }
    }

    public void DecreaseAgentCount()
    {
        if (activeAgents.Count < maxAgents)
        {
            nextSpawnTime = Time.time + GetRandomSpawnTime(); // Włącz generowanie agentów, gdy liczba spadnie poniżej maksymalnej
        }
    }

    private Color GetRandomAgentColor()
    {
        // Jeśli lista dostępnych kolorów jest pusta, wygeneruj nową listę z losowymi kolorami
        if (availableColors.Count == 0)
        {
            GenerateAvailableColors();
        }

        // Wybierz losowy kolor z listy dostępnych kolorów
        int randomIndex = Random.Range(0, availableColors.Count);
        Color randomColor = availableColors[randomIndex];

        // Usuń wybrany kolor z listy dostępnych kolorów
        availableColors.RemoveAt(randomIndex);

        return randomColor;
    }

    private void GenerateAvailableColors()
    {
        // Dodaj różne kolory do listy dostępnych kolorów
        availableColors.Add(Color.red);
        availableColors.Add(Color.blue);
        availableColors.Add(Color.green);
        availableColors.Add(Color.yellow);
        availableColors.Add(Color.cyan);
        availableColors.Add(Color.magenta);
        availableColors.Add(Color.gray);
        availableColors.Add(new Color(1f, 0.5f, 0f)); // Pomarańczowy
    }
}
