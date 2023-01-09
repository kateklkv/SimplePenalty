using System;
using UnityEngine;

namespace Runtime.Ball
{
    public class BallMovement : MonoBehaviour
    {
        [SerializeField] private InputService inputService;
        [SerializeField] private Rigidbody rigidbody;

        [SerializeField] private float throwForceInXY = 1f;
        [SerializeField] private float throwForceInZ = 50f;

        private void Awake()
        {
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable() => inputService.LongTouch += ThrowBall;

        private void OnDestroy() => inputService.LongTouch -= ThrowBall;

        private void ThrowBall(Vector2 direction, float timeInterval)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(
                - direction.x * throwForceInXY, 
                - direction.y * throwForceInXY, 
                throwForceInZ / timeInterval);
        }
    }
}