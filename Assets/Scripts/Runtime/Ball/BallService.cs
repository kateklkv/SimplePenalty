using UnityEngine;

namespace Runtime.Ball
{
    public class BallService : MonoBehaviour
    {
        [SerializeField] private InputService inputService;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform startPosition;

        private GameObject _ballGO;
        private IBall _currentBall;

        private void Start() => CreateBall();

        private void OnDestroy()
        {
            _currentBall.Destroyed -= CreateBall;
            Destroy(_ballGO);
        }

        private void CreateBall()
        {
            _ballGO = Instantiate(ballPrefab, startPosition.position, Quaternion.identity);
            
            _currentBall = _ballGO.GetComponent<IBall>();
            _currentBall.SetInputService(inputService);
            
            _currentBall.Destroyed += CreateBall;
        }
    }
}