using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        public static GameManager Instance;
        public static Camera Cam;

        private void Awake()
        {
                if (Instance != default)
                {
                        Destroy(Instance.gameObject);
                }

                Instance = this;

                Cam = Camera.main;

                InitializeGame();
        }

        private void InitializeGame()
        {
                
        }
}