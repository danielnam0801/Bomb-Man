using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonState : MonoBehaviour, IState
{
    public abstract void OnEnterState();
    public abstract void OnExitState();
    public abstract bool OnUpdateState();

    protected AgentAnimator _agentAnimator;
    protected AgentInput _agentInput;
    protected AgentController _agentController;
    protected AgentMovement _agentMovement;
    protected ActionData _actionData;
    public virtual void SetUp(Transform agentRoot)
    {
        _agentAnimator = agentRoot.Find("Visual").GetComponent<AgentAnimator>();
        _agentInput = agentRoot.GetComponent<AgentInput>();
        _agentController = agentRoot.GetComponent<AgentController>();
        _agentMovement = agentRoot.GetComponent<AgentMovement>();
        _actionData = agentRoot.GetComponent<ActionData>();
    }

    public void OnHitHandle(Vector3 position, Vector3 normal)
    {

    }
}
