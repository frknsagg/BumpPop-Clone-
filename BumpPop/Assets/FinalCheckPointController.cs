using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCheckPointController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Playable"))
        {
            Debug.Log("finishe çarptı");
            Vector3 vec = (GameManager.Instance.checkPoints[0].position - other.transform.position).normalized;
           var velo= other.gameObject.GetComponent<Rigidbody>().velocity;
          
           velo.x = vec.x * 50;
           velo.z = vec.z * 50;
           velo.y = 0;
           other.gameObject.GetComponent<Rigidbody>().velocity = velo;
        }
    }
}
