using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    AgentController agentController;
    public AgentController AgentController => agentController;
    
    ActionData actionData;
    public ActionData ActionData => actionData;

    private void OnEnable()
    {
        Init();
    }

    void Init()
    {
        if(agentController == null)
            agentController = GameManager.Instance.PlayerTrm.GetComponent<AgentController>();
        if(actionData == null)
            actionData = GameManager.Instance.PlayerTrm.GetComponent<ActionData>();
    }
}
