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
   [SerializeField] private int originalPriority = 0;

   private void Awake()
   {
       originalPriority = _virtualCamera.Priority;
   }

   private void Start()
   {
       _brain = FindObjectOfType<CinemachineBrain>();
       Assert.IsNotNull(_brain);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag("Player")) return;
      if ((CinemachineVirtualCamera) _brain.ActiveVirtualCamera == _virtualCamera) return;
      _virtualCamera.Priority = _brain.ActiveVirtualCamera.Priority + 1;
   }
   
   private void OnTriggerExit(Collider other)
   {
       if (!other.CompareTag("Player")) return;

       _virtualCamera.Priority = originalPriority;
   }
}
