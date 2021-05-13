using System;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Game_Systems
{
    public enum SceneIndex
    {
        Menu = 0,
        Intro = 1, 
        Shrine = 2,
        Forest = 3,
        Mountains = 4,
        Entrance = 5,
        PuzzleButtons = 6,
        PuzzleLevers = 7,
        Boss = 8,
        Loading = 9,
        Outro = 10
    }

    public class GameManager : Singleton<GameManager>
    {
        public PlayerClickMove PlayerMovement { get; private set; }
        public static bool AllowInput { get; private set; } = true;
        private GameObject _player;
        public int currLevel = 0;
        public Vector3 spawnPoint = Vector3.zero;
        public Quaternion spawnRot = Quaternion.identity;

        // puzzle completion status
        private bool leverPuzzleDone = false;
        private bool buttonPuzzleDone = false;
        
        private void Awake()
        {
            Init();
        }
        
        // called first
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Door.DoorChangedEvent += UpdatePuzzleStatus;
        }

        private void OnDisable()
        {
            Door.DoorChangedEvent -= UpdatePuzzleStatus;
        }

        private void UpdatePuzzleStatus(string puzzleName, bool isOn)
        {
            if (puzzleName == "lever")
            {
                leverPuzzleDone = isOn;
            } else if (puzzleName == "button")
            {
                buttonPuzzleDone = isOn;
            }
        }

        public bool GetPuzzleStatus(string puzzleName)
        {
            if (puzzleName == "lever")
            {
                return leverPuzzleDone;
            }
            else if (puzzleName == "button")
            {
                return buttonPuzzleDone;
            }
            Debug.LogError("Tried to get puzzle status of puzzle name that doesn't exist.");
            return false;
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
