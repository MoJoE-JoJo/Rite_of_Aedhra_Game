using System.Collections;
using Cinemachine;
using Game_Systems;
using UnityEngine;

    public class CameraPanningViewTrigger : MonoBehaviour
    {

        private bool _enabled = true;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private int changeDuration = 2;

        private void OnTriggerEnter(Collider other)
        {
            if (_enabled)
            {
                StartCoroutine(PanningSequence());
            }
        }

        private IEnumerator PanningSequence()
        {
            CinemachineBrain brain = CinemachineCore.Instance.GetActiveBrain(0);
            int prevPriority = virtualCamera.Priority;
            // make sure to set same priority
            virtualCamera.Priority = brain.ActiveVirtualCamera.Priority;
            GameManager.Instance.DisableInput();
            virtualCamera.gameObject.SetActive(true);
            yield return new WaitForSeconds(changeDuration + 2);
            virtualCamera.gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            GameManager.Instance.EnableInput();
            virtualCamera.Priority = prevPriority;
            _enabled = false;
            yield return null;
        }
    }
