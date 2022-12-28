using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Signals;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public List<GameObject> playableBalls;
    public bool isShoot;
    [SerializeField] public List<Transform> checkPoints;
    public float lastTempZ;
    [SerializeField] private GameObject playball;
    public int nextCheckPoint;
    private int ballCount;
    [SerializeField] private Slider levelProgressBar;

    public int BallCount
    {
        get => ballCount;
        set => ballCount = value;
    }


    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        playableBalls.Add(GameObject.FindWithTag("Playable"));
        playball = FindPlayableBall();
        var distance = Vector3.Distance(playball.transform.position, checkPoints[checkPoints.Count - 2].position);
        levelProgressBar.maxValue = distance;
        StartCoroutine(CalculateLevelProgress());
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

    IEnumerator CalculateLevelProgress()
    {
        while (true)
        {
            var distance = levelProgressBar.maxValue -
                           Vector3.Distance(playball.transform.position, checkPoints[^2].position);
            if (distance > levelProgressBar.maxValue)
            {
                levelProgressBar.value = levelProgressBar.maxValue;
            }
            else
            {
                levelProgressBar.value = distance;
            }

            yield return new WaitForSeconds(.5f);
        }
    }
}