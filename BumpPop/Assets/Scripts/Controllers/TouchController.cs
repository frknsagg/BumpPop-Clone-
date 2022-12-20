using Signals;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class TouchController : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
    {
        private Vector2 _touchPosition;
        public Vector2 direction;
        public Vector2 rotation;
        private bool _isDragged;
        private float _counter;


        public void OnPointerDown(PointerEventData eventData)
        {
            _touchPosition = eventData.position;
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            _counter += Time.deltaTime;
            if (_counter>0.1f)
            {
                _isDragged = true;
            }
            var delta = eventData.position - _touchPosition;
            direction = delta.normalized;
        
        }
    
        public void OnPointerUp(PointerEventData eventData)
        {
            
            
            if (_isDragged)
            {
                PlayerSignals.Instance.OnPointerUp?.Invoke(direction);
            }
            _counter = 0;
            direction = Vector2.zero;
        }
    
    }
}
