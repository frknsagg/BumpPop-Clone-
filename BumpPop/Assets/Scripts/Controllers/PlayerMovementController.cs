using Signals;
using UnityEngine;
using UnityEngine.Assertions.Must;


namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform followTarget;
        [SerializeField] private LineRenderer arrowLineRenderer;
        [SerializeField] private LayerMask layerMask;


        [Range(0, 1000)] public int moveSpeed;
        private Vector3 _reflectedDirection;
        private Quaternion _rotation;
        private float _eulerY;
        private RaycastHit _hit;


        void Start()
        {
            _rotation = transform.rotation;
            // Position(10, 20, new Vector3(0, 0, 1));
            ChangePlayer();
        }

        private void FixedUpdate()
        {
            ChangePlayer();
            CalculateRotation();
        }

        private void DrawPoints(Vector3 direction)
        {
            if (!GameManager.Instance.isShoot)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(-direction), out _hit, 20,
                        layerMask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(-direction) * _hit.distance,
                        Color.yellow);
                    var angle = Vector3.Angle(transform.TransformDirection(-direction), _hit.normal);
                    _reflectedDirection = Vector3.Reflect(transform.TransformDirection(-direction), _hit.normal);
                    var distance = 20 - _hit.distance;
                    Debug.DrawRay(_hit.point, _reflectedDirection * distance, Color.yellow);

                    // Debug.Log(_hit.point.normalized);
                    arrowLineRenderer.positionCount = 3;
                    arrowLineRenderer.SetPosition(0, transform.position);
                    arrowLineRenderer.SetPosition(1, _hit.point);
                    arrowLineRenderer.SetPosition(2, _hit.point + _reflectedDirection * distance);
                }
                else
                {
                    arrowLineRenderer.positionCount = 2;
                    arrowLineRenderer.SetPosition(0, transform.position);
                    arrowLineRenderer.SetPosition(1, transform.position - direction * 20);
                }
            }
        }

        private void ImpulseMovement(Vector3 vec)
        {
            if (!GameManager.Instance.isShoot)
            {
                arrowLineRenderer.positionCount = 0;
                StopCoroutine(GameManager.Instance.StopPlayableBalls());
                var velo = rb.velocity;
                velo.x = -vec.x * moveSpeed;
                velo.z = -vec.z * moveSpeed;
                rb.velocity = velo;
                GameManager.Instance.isShoot = true;
                StartCoroutine(GameManager.Instance.StopPlayableBalls());
            }
        }

        void SubscribeEvent()
        {
            PlayerSignals.Instance.OnPointerUp += ImpulseMovement;
            // CoreGameSignals.Instance.OnChangePlayer += ChangePlayer;
            InputSignals.Instance.OnDrag += DrawPoints;
        }

        private void UnSubscribeEvent()
        {
            PlayerSignals.Instance.OnPointerUp -= ImpulseMovement;
            // CoreGameSignals.Instance.OnChangePlayer -= ChangePlayer;
            InputSignals.Instance.OnDrag -= DrawPoints;
        }

        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void OnDisable()
        {
            UnSubscribeEvent();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void ChangePlayer()
        {
            var obj = GameManager.Instance.FindPlayableBall();
            rb = obj.GetComponent<Rigidbody>();
            Transform transform1;
            (transform1 = transform).SetParent(obj.transform);
            transform1.position = obj.transform.position;
            transform1.rotation = _rotation;
        }


        void CalculateRotation()
        {
            var velo = rb.velocity;
            var angle = Mathf.Atan2(velo.x, velo.z) * Mathf.Rad2Deg;
            _eulerY = Mathf.Lerp(_eulerY, angle, 0.125f);
            var euler = followTarget.eulerAngles;
            euler.y = _eulerY;
            followTarget.eulerAngles = euler;
        }
    }
}