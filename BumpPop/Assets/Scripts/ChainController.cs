using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    public int needToBreakBall;
    public List<GameObject> ballList;
    [SerializeField] private TextMeshProUGUI ballCountText;
    [SerializeField] private List<GameObject> chainList;

    public HingeJoint hingeJoint;
    // public GameObject[] cube;

    void Start()
    {
        ballCountText.text = "" + needToBreakBall;
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Playable"))
        {
          ImpulseMove(other);
            if (!ballList.Contains(other.gameObject))
            {
                ballList.Add(other.gameObject);
                needToBreakBall--;
                TextChange();
                ChangeColor();
                if (needToBreakBall == 0)
                {
                    hingeJoint.breakForce = 0;
                    // for (int i = 0; i < cube.Length; i++)
                    // {
                    //     Destroy(cube[i]);
                    // }
                    ImpulseMove(other);
                    
                }
                
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Playable"))
        {
           ImpulseMove(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Playable"))
        {
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    void ImpulseMove(Collider other)
    {
        var objVelo = other.GetComponent<Rigidbody>().velocity;
        var vec = (GameManager.Instance.checkPoints[0].position - other.transform.position).normalized;
        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        objVelo.x = vec.x * 20;
        objVelo.z = vec.z * 20;
        objVelo.y = -1f;
        other.GetComponent<Rigidbody>().velocity = objVelo;
    }

    void TextChange()
    {
        if (!(needToBreakBall < 0))
        {
            ballCountText.text = "" + needToBreakBall;
        }
    }

    void ChangeColor()
    {
        for (int i = 0; i < chainList.Count; i++)
        {
            chainList[i].gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.red, 1);
        }
    }
}