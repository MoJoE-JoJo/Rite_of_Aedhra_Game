using System.Collections;
using System.Collections.Generic;
using Game_Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class NextLevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingText;
    [SerializeField] private GameObject continueButton;
    private bool _goNext = false;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(LoadLevel());
    }

    public void Continue()
    {
        _goNext = true;
    }

    private IEnumerator LoadLevel()
    {
        yield return null;

        AsyncOperation asyncLoading = SceneManager.LoadSceneAsync(GameManager.Instance.currLevel+1);
        asyncLoading.allowSceneActivation = false;
        while (!asyncLoading.isDone)
        {
            if (asyncLoading.progress >= 0.9f)
            {
                if (loadingText)
                    loadingText.SetActive(false);
                if(continueButton)
                    continueButton.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space) || _goNext)
                    asyncLoading.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
