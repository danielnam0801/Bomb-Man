using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private PoolingListSO _poolingList;
      
    public LayerMask whatIsGround;

    private Transform _playerTrm;
    public Transform PlayerTrm
    {
        get
        {
            if (_playerTrm == null)
            {
                _playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return _playerTrm;
        }
    }

    private Transform _playerOriginTrm;
    public Transform PlayerOriginTrm
    {
        get
        {
            if(_playerOriginTrm == null)
            {
                _playerOriginTrm = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerPoint").transform;
            }
            return _playerOriginTrm;
        }
    }





    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Multiple GameManager is running");
        }
        Instance = this;

        CreatePool();
        CreateTimeController();
        CreateUIManager();
        CreateBombManager();
        CreatePlayerManager();
        CreateVFXManager();
    }

    private void CreateBombManager()
    {
        BombManager.Instance = gameObject.AddComponent<BombManager>();
    } 

    private void CreatePlayerManager()
    {
        PlayerManager.Instance = gameObject.AddComponent<PlayerManager>();
    }

    private void CreateVFXManager()
    {
        VFXManager.Instance = gameObject.AddComponent<VFXManager>();
    }

    private void CreatePool()
    {
        PoolManager.Instance = new PoolManager(transform);
        _poolingList.Pairs.ForEach(pair =>
        {
            PoolManager.Instance.CreatePool(pair.Prefab, pair.Count);
        });
    }

    private void CreateTimeController()
    {
        TimeController.Instance = gameObject.AddComponent<TimeController>();
    }

    private void CreateUIManager()
    {
        //UIDocument uidocument = FindObjectOfType<UIDocument>();
        //UIManager.Instance = uidocument.gameObject.AddComponent<UIManager>();
    }

    public Vector3 ReturnVector3PosXZ(Vector3 pos)
    {
        return new Vector3(pos.x, 0, pos.z);
    }
    #region 디버그 모드

    [SerializeField]
    private LayerMask _whatIsGround;
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);

        //    RaycastHit hit;

        //    bool result = Physics.Raycast(ray, out hit, Define.MainCam.farClipPlane, 
        //                _whatIsGround);

        //    if(result)
        //    {
        //        EnemyController e = PoolManager.Instance.Pop("HammerEnemy") as EnemyController;
        //        e.transform.SetPositionAndRotation(hit.point, Quaternion.identity);
        //    }
        //}
    }
    #endregion
}
