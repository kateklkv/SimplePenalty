using Runtime.Ball;
using UnityEngine;

namespace Runtime.Triggers
{
    public class LoseTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IBall ball))
                Destroy(other.gameObject);
        }
    }
}