using System;
using UnityEngine;

namespace Runtime.Ball
{
    public class BallService : MonoBehaviour
    {
        public event Action<int, int> ChangedAttempts;
        public event Action EndedAttempts;

        public int Score => _score;

        [SerializeField] private InputService inputService;
        
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform startPosition;
        
        [SerializeField] private int totalAttempts;
        [SerializeField] private int score;

        private GameObject _ballGO;
        private IBall _currentBall;

        private int _attemptsCount;
        private int _hitCount;
        private int _score;

        private void Awake() => CreateBall();

        private void OnDestroy()
        {
            _currentBall.Destroyed -= CreateBall;
            Destroy(_ballGO);
        }

        public void UpdateScores()
        {
            _hitCount++;
            _score = score * _hitCount + 3;
            ChangedAttempts?.Invoke(_attemptsCount, _score);
        }

        private void CreateBall()
        {
            if (_attemptsCount >= totalAttempts)
            {
                EndedAttempts?.Invoke();
                return;
            }

            _attemptsCount++;
            ChangedAttempts?.Invoke(_attemptsCount, _score);
            
            _ballGO = Instantiate(ballPrefab, startPosition.position, Quaternion.identity);
            
            _currentBall = _ballGO.GetComponent<IBall>();
            _currentBall.SetInputService(inputService);
            
            _currentBall.Destroyed += CreateBall;
        }
    }
}