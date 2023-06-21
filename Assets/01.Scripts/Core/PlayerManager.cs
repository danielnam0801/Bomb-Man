using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    AgentController agentController;
    public AgentController AgentController => agentController;
    
    ActionData actionData;
    public ActionData ActionData => actionData;

    Transform unitParent;
    MainUI mainUI;
    Transform boss;
    Transform player;

    private void OnEnable()
    {
        StartCoroutine("InitWait", 0.2f);
    }

    void Init()
    {
        mainUI = GameObject.Find("MainUI").GetComponent<MainUI>();
        unitParent = GameObject.Find("UnitParent").transform;
        unitParent.Find("Player").gameObject.SetActive(true);
        
        boss = unitParent.Find("BossRobot").transform;
        player = unitParent.Find("Player").transform;
        boss.gameObject.SetActive(true);

        if (agentController == null)
            agentController = unitParent.Find("Player").transform.GetComponent<AgentController>();
        if (actionData == null)
            actionData = unitParent.Find("Player").transform.GetComponent<ActionData>();
    }

    public void FindPlayer()
    {
        mainUI.Subscribe(boss.GetComponent<EnemyHealth>(), player.GetComponent<AgentHealth>());
    }

    IEnumerator InitWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Init();
    }
}
