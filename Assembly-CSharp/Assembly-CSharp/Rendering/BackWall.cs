using UnityEngine;

namespace rendering {
    public class BackWall : MonoBehaviour {
        [SerializeField]
        public Texture2DArray array;

        [SerializeField]
        public Material backwallMaterial;

        private void Awake() { backwallMaterial.SetTexture("images", array); }
    }
}