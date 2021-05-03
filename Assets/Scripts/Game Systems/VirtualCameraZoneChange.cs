using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

public class VirtualCameraZoneChange : MonoBehaviour
{
   [SerializeField] private CinemachineVirtualCamera _virtualCamera;
   private CinemachineBrain _brain;

   private void Start()
   {
       _brain = FindObjectOfType<CinemachineBrain>();
       Assert.IsNotNull(_brain);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag("Player")) return;
      
      _virtualCamera.Priority = _brain.ActiveVirtualCamera.Priority + 1;
   }
   
   private void OnTriggerExit(Collider other)
   {
       if (!other.CompareTag("Player")) return;
      
       _virtualCamera.Priority = _brain.ActiveVirtualCamera.Priority - 2;
   }
}
