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
        [SerializeField] private GameObject[] arrows;
        [SerializeField] private ObjectPool _objectPool;
        public int ballCount;

        private bool chz;
        private int _clonedBallCount = 0;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Playable") && gameObject.CompareTag("Clonable"))
            {
                gameObject.tag = "Playable";
                chz = true;
                StartCoroutine(StartClone());
                GameManager.Instance.isShoot = true;
                for (int i = 0; i < arrows.Length; i++)
                {
                    arrows[i].SetActive(false);
                }
                
            }
        }
        private void SpawnBall()
        {
            var position = transform.position;

            Vector3 vec = 2 * Random.insideUnitCircle;
            vec.z = vec.y;
            vec.y = 0;
            vec += position;

            var obj = _objectPool.GetPooledObject(0);
            obj.GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
            obj.transform.position = vec;
            GameManager.Instance.playableBalls.Add(obj);
            var objVelo = obj.gameObject.GetComponent<Rigidbody>().velocity;
            vec = (GameManager.Instance.checkPoints[0].position - obj.transform.position).normalized;
            objVelo.x = vec.x * 30;
            objVelo.z = vec.z * 30;

            obj.gameObject.GetComponent<Rigidbody>().velocity = objVelo;

            _clonedBallCount++;
            if (_clonedBallCount >= ballCount)
            {
                StopCoroutine(StartClone());
                Destroy(this);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator StartClone()
        {
            for (int j = 0; j < ballCount; j++)
            {
                SpawnBall();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}