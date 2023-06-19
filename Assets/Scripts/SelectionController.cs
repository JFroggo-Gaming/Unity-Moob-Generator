using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionController : MonoBehaviour
{
    private Button button;
    private TMP_Text buttonText;

    private void Start()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();
        button.onClick.AddListener(ToggleAgents);
        buttonText.text = AgentManager.Instance.allAgentsSelected ? "Unmark all the moobs" : "Mark all the moobs";
    }

    private void ToggleAgents()
    {
        AgentManager.Instance.ToggleSelectAllAgents();
        buttonText.text = AgentManager.Instance.allAgentsSelected ? "Unmark all the moobs" : "Mark all the moobs";
    }
}
