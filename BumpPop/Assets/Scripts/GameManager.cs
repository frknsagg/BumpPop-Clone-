using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Extensions;
using Signals;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public List<GameObject> playableBalls;
    public bool isShoot;
    [SerializeField] public List<Transform> checkPoints;
    public float lastTempZ;
    [SerializeField] private GameObject playball;
    public int nextCheckPoint;

    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        playableBalls.Add(GameObject.FindWithTag("Playable"));
        playball = FindPlayableBall();
    }

    public GameObject FindPlayableBall()
    {
        foreach (var item in playableBalls)
        {
            var tempZ = Vector3.Distance(checkPoints[0].position, item.transform.position);

            if (tempZ < lastTempZ)
            {
                lastTempZ = tempZ;
                playball = item.gameObject;
            }
        }

        return playball;
    }

    public IEnumerator StopPlayableBalls()
    {
        while (isShoot)
        {
            playball = FindPlayableBall();
            if (playball.GetComponent<Rigidbody>().IsSleeping())
            {
                isShoot = false;
                StopCoroutine(StopPlayableBalls());
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    void RemoveCheckPoint()
    {
    }
}