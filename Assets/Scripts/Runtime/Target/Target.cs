using UnityEngine;

namespace Runtime.Target
{
    public class Target : MonoBehaviour, ITarget
    {
        [SerializeField] private MeshRenderer meshRenderer;
        
        public void SetGreenColor() => SetColor(Color.green);

        private void OnDestroy() => SetColor(Color.red);

        private void SetColor(Color color) => meshRenderer.sharedMaterial.color = color;
    }
}