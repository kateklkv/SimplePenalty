using System;
using Runtime.Ball;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public event Action BallHit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BallMovement ball))
        {
            Destroy(ball.gameObject);
            BallHit?.Invoke();
        }
    }
}
