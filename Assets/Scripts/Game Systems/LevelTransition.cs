using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game_Systems;
using PixelCrushers.DialogueSystem.UnityGUI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private SceneIndex nextLevel;
    [SerializeField] private Vector3 nextSpawn = Vector3.zero;
    [SerializeField] private Quaternion nextRot = Quaternion.identity;
    [SerializeField] private CanvasGroup fade;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.DisableInput();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return fade.DOFade(1f, 2f).OnComplete(() =>
        {
            GameManager.Instance.currLevel = (int) nextLevel;
            GameManager.Instance.spawnPoint = nextSpawn;
            GameManager.Instance.spawnRot = nextRot;
            GameManager.Instance.LoadLevel();
        });
    }
    private void Update()
    {
        // if (!_decelerate) return;
        // NavMeshAgent agent = GameManager.Instance.PlayerMovement.Agent;
        // if(agent.speed > walkSpeed)
        //     agent.speed = 0.95f*GameManager.Instance.PlayerMovement.Agent.speed;
    }
}
