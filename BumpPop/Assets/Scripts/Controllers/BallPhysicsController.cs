using System;
using System.Collections;
using DG.Tweening;
using Signals;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Controllers
{
    public class BallPhysicsController : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;
        public int ballCount;
        private float _counter;
        private bool chz ;
        private int _clonedBallCount = 0;
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Playable"))
            {
               
                chz = true;
                Debug.Log("çarpıştı");
                StartCoroutine(StartClone());
                CoreGameSignals.Instance.OnChangePlayer?.Invoke(GameManager.Instance.FindPlayableBall());
            }
        }

        // private void FixedUpdate()
        // {
        //     if (chz && _clonedBallCount<ballCount)
        //     {
        //         _counter += Time.deltaTime;
        //         if (_counter>0.05f)
        //         {
        //             SpawnBall();
        //         }
        //         
        //     }
        // }

        private void SpawnBall()
        {
            var position = transform.position;
            Vector3 vec = 2*Random.insideUnitCircle;
            vec.z = Mathf.Abs(vec.y);
            vec.y = 0;
            vec += position;

            var obj = Instantiate(ballPrefab, vec, Quaternion.identity);

            GameManager.Instance.playableBalls.Add(obj);
            var objVelo = obj.gameObject.GetComponent<Rigidbody>().velocity;

            objVelo.x = Random.Range(-1, 1) * 3;
            objVelo.z = Random.Range(20, 30);
            obj.gameObject.GetComponent<Rigidbody>().velocity = objVelo;
            _counter = 0;
            _clonedBallCount++;
            if (_clonedBallCount >= ballCount)
            {
                Destroy(this);
            }
        }

        IEnumerator StartClone()
        {
            for (int j = 0; j < ballCount; j++)
            {
                SpawnBall();
                yield return new WaitForSeconds(0.02f);
            }
            
           
        }
    }
}