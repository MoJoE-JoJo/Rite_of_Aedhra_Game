using UnityEngine;
using UnityEngine.Assertions;

namespace Game_Systems
{
    public class GameManager : Singleton<GameManager>
    {
        private GameObject _player;
        private PlayerClickMove _playerClickMove;
        public static bool AllowInput { get; private set; } = true;
        
        private void Start()
        {
            InitPlayer();
        }

        public void DisableInput()
        {
            AllowInput = false;
            if(_playerClickMove == null)
                InitPlayer();
            _playerClickMove.StopMoving();
        }

        private void InitPlayer()
        {
            _player = GameObject.FindWithTag("Player");
            _playerClickMove = _player.GetComponent<PlayerClickMove>();
        }

        public void EnableInput()
        {
            AllowInput = true;
        }
    }
}
