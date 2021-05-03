using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Game_Systems
{
    internal enum SceneIndex
    {
        Intro = 0, 
        Shrine = 1,
        Forest = 2,
        Mountains = 3,
        Entrance = 4,
        PuzzleButtons = 5,
        PuzzleLevers = 6,
        Boss = 7,
        Loading = 8
} 
    public class GameManager : Singleton<GameManager>
    {
        public PlayerClickMove PlayerMovement { get; private set; }
        public static bool AllowInput { get; private set; } = true;
        private GameObject _player;
        public int currLevel = 0;
        public Vector3 spawnPoint = Vector3.zero;
        public Quaternion spawnRot = Quaternion.identity;
        
        private void Awake()
        {
            Init();
        }
        
        // called first
        private void OnEnable()
        {
            Debug.Log("OnEnable called");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // called second
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Init();
        }


        public void DisableInput()
        {
            AllowInput = false;
        }

        private void Init()
        {
            _player = GameObject.FindWithTag("Player");
            if (!_player) return;
            currLevel = SceneManager.GetActiveScene().buildIndex;
            
            PlayerMovement = _player.GetComponent<PlayerClickMove>();
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene((int) SceneIndex.Loading);
        }

        public void EnableInput()
        {
            AllowInput = true;
        }
    }
}
