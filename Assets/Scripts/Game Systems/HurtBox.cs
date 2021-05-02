using Player;
using UnityEngine;

namespace Game_Systems
{
    public class HurtBox : MonoBehaviour
    {
        public bool isEnabled;

        private void OnTriggerEnter(Collider other)
        {
            if (!isEnabled) return;
            if (!other.gameObject.CompareTag("Player")) return;
            Debug.Log("Just die dude");
            other.gameObject.GetComponent<PlayerController>().KillPlayer();
        }
    }
}
