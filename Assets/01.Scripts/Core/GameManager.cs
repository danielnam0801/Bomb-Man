using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private PoolingListSO _poolingList;
  
    public bool IsIntroScene = true;
    public bool IsLoadingScene = false;
    public bool IsFightScene = false;

    private Transform _playerTrm;
    public Transform PlayerTrm
    {
        get
        {
            if (IsFightScene)
            {
                if (_playerTrm == null)
                {
                    _playerTrm = GameObject.FindGameObjectWithTag("Player").transform;
                }
            }
            return _playerTrm;
        }
    }

    private Transform _playerOriginTrm;
    public Transform PlayerOriginTrm
    {
        get
        {
            if (IsFightScene)
            {
                if(_playerOriginTrm == null)
                {
                    _playerOriginTrm = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerPoint").transform;
                }
            }
            return _playerOriginTrm;
        }
    }

    bool orthoState = true;
    public void ChangeCameraModeToOrthographic()
    {
        //if (orthoState == value) return;

        orthoState = !orthoState;
        Camera.main.orthographic = orthoState;
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
            Debug.LogError("Multiple GameManager is running");
        }
        
        Instance = this;

        CreatePool();
        CreateTimeController();
        CreateBombManager();
        CreateUIManagers();
        CreateVFXManager();
        //CreatePlayerManager();

        DontDestroyOnLoad(this);
    }


    public void FightSceneInit()
    {
        if(PlayerManager.Instance != null)
            Destroy(PlayerManager.Instance);
       
        CreatePlayerManager();
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

    private void CreateUIManagers()
    {
        Transform a = transform.Find("UIManager");
        UIManager.Instance = a.gameObject.AddComponent<UIManager>();
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

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public Vector3 ReturnVector3PosXZ(Vector3 pos)
    {
        return new Vector3(pos.x, 0, pos.z);
    }

    public Vector3 TargetGroundPos()
    {
        RaycastHit checkGround;
        if(Physics.Raycast(PlayerOriginTrm.position, Vector3.down, out checkGround, 100f, _whatIsGround))
        {
            return checkGround.point;
        }
        else
        {
            return PlayerOriginTrm.position;
        }
        
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    #region 디버그 모드

    [SerializeField]
    private LayerMask _whatIsGround;
    public LayerMask whatIsGround => _whatIsGround;
    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.Q))
    //    {
    //        ChangeCameraModeToOrthographic(false);
    //    }
    //    if(Input.GetKeyUp(KeyCode.Q))
    //        ChangeCameraModeToOrthographic(true);
    //}
    #endregion
}
