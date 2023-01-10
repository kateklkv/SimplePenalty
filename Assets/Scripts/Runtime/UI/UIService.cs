using Runtime.Ball;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.UI
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private BallService ballService;

        [SerializeField] private GameObject topContainer;
        [SerializeField] private GameObject restartScreen;

        [SerializeField] private TextMeshProUGUI attemptsCount;
        [SerializeField] private TextMeshProUGUI scoresCount;
        [SerializeField] private TextMeshProUGUI scoresFromRestartScreen;

        private void OnEnable()
        {
            ballService.ChangedAttempts += UpdateCounts;
            ballService.EndedAttempts += EnableRestartScreen;
        }
        
        private void OnDisable()
        {
            ballService.ChangedAttempts -= UpdateCounts;
            ballService.EndedAttempts -= EnableRestartScreen;
        }

        public void Restart() => SceneManager.LoadScene(0);

        private void UpdateCounts(int attempts, int scores)
        {
            attemptsCount.text = attempts.ToString();
            scoresCount.text = scores.ToString();
        }

        private void EnableRestartScreen()
        {
            topContainer.SetActive(false);
            restartScreen.SetActive(true);
            scoresFromRestartScreen.text = string.Format(scoresFromRestartScreen.text, ballService.Score);
        }
    }
}