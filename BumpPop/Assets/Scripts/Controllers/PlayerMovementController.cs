using System.Collections;
using Signals;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private TouchController touchController;
        private Vector3 _direction;
        [SerializeField] private Rigidbody rb;
        [Range(0, 1000)] public int moveSpeed;
        private RaycastHit _hit;
        private Quaternion _rotation;

        [SerializeField] private GameObject ArrowPrefab;
        [SerializeField] private GameObject[] Arrows;
        public int numberOfArrows;
        public GameObject _playableBalls;
        private float _counter;


        void Start()
        {
            _rotation = transform.rotation;
            Arrows = new GameObject[numberOfArrows];

            for (int i = 0; i < numberOfArrows; i++)
            {
                Arrows[i] = Instantiate(ArrowPrefab, transform.position, Quaternion.identity);
            }

            ChangePlayer(GameManager.Instance.FindPlayableBall());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _direction = new Vector3(touchController.direction.x, 0f, touchController.direction.y);
            for (int i = 0; i < Arrows.Length; i++)
            {
                Arrows[i].transform.position = ArrowPosition(i * 0.1f);
                Arrows[i].transform.rotation = new Quaternion(0, -_direction.x, 0, 1);
            }
        }

        private void ImpulseMovement(Vector3 vec)
        {
            if (!GameManager.Instance.isShoot)
            {
                var velo = rb.velocity;
                velo.x = -vec.x * moveSpeed;
                velo.z = -vec.y * moveSpeed;
                rb.velocity = velo;
                GameManager.Instance.isShoot = true;
            }
        }

        void SubscribeEvent()
        {
            PlayerSignals.Instance.OnPointerUp += ImpulseMovement;
            CoreGameSignals.Instance.OnChangePlayer += ChangePlayer;
        }

        private void UnSubscribeEvent()
        {
            PlayerSignals.Instance.OnPointerUp -= ImpulseMovement;
            CoreGameSignals.Instance.OnChangePlayer -= ChangePlayer;
        }

        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void OnDisable()
        {
            UnSubscribeEvent();
        }

        Vector3 ArrowPosition(float t)
        {
            Vector3 currentPointPos = transform.position +
                                      (-_direction == Vector3.zero ? transform.forward : -_direction * (5 * t));
            return currentPointPos;
        }

        void ChangePlayer(GameObject obj)
        {
            rb = obj.GetComponent<Rigidbody>();
            transform.SetParent(obj.transform);
            transform.position = obj.transform.position;
            transform.rotation = _rotation;
        }

        IEnumerator SetPlayer()
        {
            var obj = GameManager.Instance.FindPlayableBall();
            rb = obj.GetComponent<Rigidbody>();
            transform.SetParent(obj.transform);
            transform.position = obj.transform.position;
            transform.rotation = _rotation;
            yield return new WaitForSeconds(0.5f);
        }
    }
}