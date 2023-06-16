using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;


public class RBBossAIBrain : MonoBehaviour
{
    public enum AttackType
    {
        Dash,
        Shoot,
        Jump
    }

    public class EnemyAttackData
    {
        public RbBossAttack atk;
        public Action action;
        public UnityEvent animAction;
        public float coolTime;
        public AttackType attackName;
    }
    Dictionary<AttackType, float> _attackCoolList;
    Dictionary<AttackType, EnemyAttackData> _attackDictionary;
    Dictionary<AttackType, bool> _canAttackList;

    NavAgentMovement _movement;
    RobotBossPhaseData bossInfo;
    AIActionData actionData;

    [Header("CoolValue")]
    [SerializeField] private float dashCool = 5f;    
    [SerializeField] private float JumpCool = 3f;    
    [SerializeField] private float shootCool = 5f;    

    [Tooltip("공격사이에 공격간격두기")]
    [Header("AttackTerm")]
    public bool isUseAttackTerm = false;
    [SerializeField] private float attackTerm = 4f;

    Queue<EnemyAttackData> attackQueue;

    private void Awake()
    {
        Init();
        MakeAttackTypeAction();
    }

    private void Init()
    {
        _movement = GetComponent<NavAgentMovement>();
        bossInfo = transform.Find("AI").GetComponent<RobotBossPhaseData>();
        actionData = transform.Find("AI").GetComponent<AIActionData>();
        _attackCoolList = new Dictionary<AttackType, float>();
        _canAttackList = new Dictionary<AttackType, bool>();
        _attackDictionary = new Dictionary<AttackType, EnemyAttackData>();
        attackQueue = new Queue<EnemyAttackData>();
    }

    private void MakeAttackTypeAction()
    {
        Transform atkTrm = transform.Find("AttackType");

        EnemyAttackData dashAttack = new EnemyAttackData()
        {
            atk = atkTrm.GetComponent<RbBossDashAttack>(),
            attackName = AttackType.Dash,
            action = () =>
            {
                actionData.IsAttacking = false;
                bossInfo.IsDashing = false;
                _canAttackList[AttackType.Dash] = true;
                SetCoolDown(AttackType.Dash, dashCool);
            },
            coolTime = dashCool,
        };
        attackQueue.Enqueue(dashAttack);
        _attackDictionary.Add(dashAttack.attackName, dashAttack);
       
        EnemyAttackData jumpAttack = new EnemyAttackData()
        {
            atk = atkTrm.GetComponent<RbBossJumpAttack>(),
            attackName = AttackType.Jump,
            action = () =>
            {
                actionData.IsAttacking = false;
                bossInfo.IsJumping = false;
                _canAttackList[AttackType.Jump] = true;
                SetCoolDown(AttackType.Jump, JumpCool);
            },
            coolTime = JumpCool,
        };
        attackQueue.Enqueue(jumpAttack);
        _attackDictionary.Add(jumpAttack.attackName, jumpAttack);
       
        EnemyAttackData shootAttack = new EnemyAttackData()
        {
            atk = atkTrm.GetComponent<RbBossShootAttack>(),
            attackName = AttackType.Shoot,
            action = () =>
            {
                actionData.IsAttacking = false;
                bossInfo.IsShooting = false;
                _canAttackList[AttackType.Shoot] = true;
                SetCoolDown(AttackType.Shoot, shootCool);
            },
            coolTime = shootCool,
        };

        attackQueue.Enqueue(shootAttack);
        _attackDictionary.Add(shootAttack.attackName, shootAttack);
      
        foreach (var skill in _attackDictionary.Values)
        {
            _attackCoolList.Add(skill.attackName, skill.coolTime);
            _canAttackList.Add(skill.attackName, true);
        }
    }

    public virtual void Attack(AttackType skillname)
    {
        if (isCoolDown(skillname) == false) return;

        Debug.Log("ThisAttack : " + skillname);
        EnemyAttackData atkData = null;
        if(_attackDictionary.TryGetValue(skillname, out atkData)){
            _movement.StopImmediately();
            SetAttackValue(skillname);
            _canAttackList[skillname] = false;
            atkData.atk.Attack(atkData.action);
            GotoEndQueue(); // 공격이 실행되었으면 우선순위 마지막으로 미룸
        }
    }

    public void SetAttackValue(AttackType key)
    {
        switch (key)
        {
            case AttackType.Shoot:
                bossInfo.IsShooting = true;
                break;
            case AttackType.Jump:
                bossInfo.IsJumping = true;
                break;
            case AttackType.Dash:
                bossInfo.IsDashing = true;
                break;
        }
    }

    public bool isCoolDown(AttackType key)
    {
        float coolDown;
        if (_attackCoolList.TryGetValue(key, out coolDown))
        {
            return Time.time > coolDown;
        }
        else //들어온 key에 해당하는 value가 없음
        {
            return false;
        }
    }

    public void SetCoolDown(AttackType key, float duration)
    {
        float coolDown = Time.time + duration;
        if (_attackCoolList.ContainsKey(key))
        {
            _attackCoolList[key] = coolDown;
        }
        else
        {
            _attackCoolList.Add(key, coolDown);
        }
    }

    public RbBossAttack GetAttack(AttackType key)
    {
        foreach(var a in _attackDictionary)
        {
            Debug.Log($"keyName : {key}, {a.Value.attackName}");
        }
        Debug.Log($"Key : {key}, {_attackDictionary[key]}");
        return _attackDictionary[key].atk;
    }

    public bool CheckAttack(AttackType key)
    {
        return bossInfo.CanAttack == true && _canAttackList[key] == true
            && isCoolDown(key) == true;
    }

    public void CallEndAct(AttackType key)
    {
        _attackDictionary[key].action?.Invoke();
    }

    void GotoEndQueue()
    {
        EnemyAttackData temp = attackQueue.Peek(); //뒷순위로 미룬다
        attackQueue.Dequeue();
        attackQueue.Enqueue(temp); // 
    }

}
