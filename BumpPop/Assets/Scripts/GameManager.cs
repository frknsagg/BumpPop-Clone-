using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Signals;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
  public List<GameObject> playableBalls;
  public bool isShoot;

  protected override void Awake()
  {
    Application.targetFrameRate = 60;
    playableBalls.Add(GameObject.FindWithTag("Playable"));
    StartCoroutine(BallSleepingCheck());
  }
  
  public GameObject FindPlayableBall( )
  {
    GameObject obj = null;
    float tempZ = -50;

    foreach (var item in playableBalls)
    {
      if (tempZ < item.transform.position.z)
      {
        if (item.transform.position.z>500 && item.transform.position.z<-50)
        {
          playableBalls.Remove(item.gameObject);
          Destroy(item.gameObject);
        }
        tempZ = item.transform.position.z;
        obj = item.gameObject;
      }
    }

    return obj;
  }

 public IEnumerator BallSleepingCheck()
  {
    yield return new WaitForSeconds(0.5f);

    foreach (var item in playableBalls)
    {
      while (true)
      {
        var asl = item.GetComponent<Rigidbody>().IsSleeping();
        Debug.Log(asl);
      }

    }
    CoreGameSignals.Instance.OnChangePlayer?.Invoke(FindPlayableBall());
    
  }
}
