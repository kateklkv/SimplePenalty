using System.Collections;
using UnityEngine;

namespace Runtime.Target
{
    public class TargetService : MonoBehaviour
    {
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private float timeDestroyTarget;

        private GameObject _targetGO;
        private TargetTrigger _targetTrigger;
        private ITarget _target;

        private WaitForSeconds _waitForSeconds;

        private void Start()
        {
            _waitForSeconds = new WaitForSeconds(timeDestroyTarget);
            CreateTarget();
        }

        private void OnDestroy()
        {
            _targetTrigger.BallHit -= ReplaceTarget;
            Destroy(_targetGO);
        }

        private void CreateTarget()
        {
            _targetGO = Instantiate(targetPrefab, transform);

            float randomX = Random.Range(-2.2f, 2.2f);
            float randomY = Random.Range(0.5f, 1.55f);
            _targetGO.transform.localPosition = new Vector3(randomX, randomY, 0f);
            
            _targetTrigger = _targetGO.GetComponent<TargetTrigger>();
            _target = _targetGO.GetComponent<ITarget>();

            _targetTrigger.BallHit += ReplaceTarget;
        }

        private void ReplaceTarget() => StartCoroutine(ReplaceTargetCoroutine());

        private IEnumerator ReplaceTargetCoroutine()
        {
            _target.SetGreenColor();
            yield return _waitForSeconds;
            Destroy(_targetGO);
            CreateTarget();
        }
    }
}