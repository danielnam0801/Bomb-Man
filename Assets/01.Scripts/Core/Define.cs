using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum StateType
    {
        Normal,
        Attack,
        Jump,
    }
    public class Define {
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
}
