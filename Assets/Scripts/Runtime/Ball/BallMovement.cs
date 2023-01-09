using System;
using UnityEngine;

namespace Runtime.Ball
{
    public class BallMovement : MonoBehaviour, IBall
    {
        public event Action Destroyed;
        
        [SerializeField] private Rigidbody rigidbody;

        [SerializeField] private float throwForceInXY = 1f;
        [SerializeField] private float throwForceInZ = 50f;
        [SerializeField] private float destroyTime;
        
        private InputService _inputService;

        public void SetInputService(InputService inputService)
        {
            _inputService = inputService;
            _inputService.LongTouch += ThrowBall;
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
            _inputService.LongTouch -= ThrowBall;
        }

        private void ThrowBall(Vector2 direction, float timeInterval)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(
                - direction.x * throwForceInXY, 
                - direction.y * throwForceInXY, 
                throwForceInZ / timeInterval);
            
            Destroy(gameObject, destroyTime);
        }
    }
}