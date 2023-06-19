using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public static AgentManager Instance { get; private set; }
    private List<AgentScript> agents; // Lista agentów
    public bool allAgentsSelected; // Flaga określająca, czy wszyscy agenci są zaznaczeni

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            agents = new List<AgentScript>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddAgent(AgentScript agent)
    {
        agents.Add(agent);
        agent.SetSelected(allAgentsSelected); // Dodajemy zaznaczenie nowego agenta
    }

    public void RemoveAgent(AgentScript agent)
    {
        agents.Remove(agent);
    }

    public void ToggleSelectAllAgents()
    {
        allAgentsSelected = !allAgentsSelected;

        foreach (AgentScript agent in agents)
        {
            if (agent != null)
            {
                agent.SetSelected(allAgentsSelected);
            }
        }
    }
}
