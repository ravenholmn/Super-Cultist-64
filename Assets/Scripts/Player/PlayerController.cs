using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public static PlayerController Instance;

        public SO_PlayerConfig PlayerConfig;

        private void Start()
        {
                Instance = this;
        }
}