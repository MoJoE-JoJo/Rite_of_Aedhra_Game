using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Game_Systems
{
    public class GameManager : Singleton<GameManager>
    {
        public PlayerClickMove PlayerMovement { get; private set; }
        public static bool AllowInput { get; private set; } = true;
        [SerializeField] private int loadingScreenSceneIndex;
        private GameObject _player;
        public int currLevel = 0;
        
        private void Start()
        {
            Init();
            loadingScreenSceneIndex = 3;
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
            if (_player)
            {
                PlayerMovement = _player.GetComponent<PlayerClickMove>();
                currLevel = SceneManager.GetActiveScene().buildIndex;
            }
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(loadingScreenSceneIndex);
        }

        public void EnableInput()
        {
            AllowInput = true;
        }
    }
}
