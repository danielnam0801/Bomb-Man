using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum StateType
    {
        Normal,
        Jump,
        //OnHit
    }

    public enum ResourceType
    {
        
    }

    public class Define {
        private static int mapXSize = 50;
        private static int mapZSize = 50;

        public static int MAPXSIZE => mapXSize;
        public static int MAPZSIZE => mapZSize;

        private static Camera _mainCam = null;
        public static Camera MainCam
        {
            get
            {
                if (_mainCam == null)
                    _mainCam = Camera.main;
                return _mainCam;
            }
        }
    }

    public class DefineEtc : MonoBehaviour
    {
        private static CinemachineVirtualCamera _cmVCam = null;
        public static CinemachineVirtualCamera VCam
        {
            get
            {
                if (_cmVCam == null)
                    _cmVCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
                return _cmVCam;
            }
        }
    }
}
