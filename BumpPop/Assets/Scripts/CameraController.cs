using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Signals;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
   private RaycastHit _hit;
  [SerializeField] private LayerMask _layerMask;

  // private void Start()
  // {
  //     StartCoroutine(FindLookAt());
  // }

  // IEnumerator FindLookAt()
  //  {
  //      while (GameManager.Instance.isShoot)
  //      {
  //          if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),out _hit,Mathf.Infinity,_layerMask))
  //          {
  //              Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * _hit.distance, Color.yellow);
  //              Debug.Log(_hit.collider);
  //          }
  //          yield return new WaitForSeconds(1f);
  //      }
  //      
  //  }

    protected  void OnEnable()
    {
        InputSignals.Instance.OnDrag += OnDrag;
        Debug.Log("camera on enable");
    }
    
    private void OnDisable()
    {
        InputSignals.Instance.OnDrag -= OnDrag;
    }
    
    void OnDrag(Vector3 direction)
    {
        var _rot = transform.rotation;
        _rot.y = -direction.x / Time.deltaTime;
        transform.rotation = _rot;
    }
}