using UnityEngine;
namespace HexagonSystem
{
    /// <summary>
    /// Attached to Hexagon prefab to get information
    /// Destroyed after get information
    /// </summary>
    public class HexagonViewTempInfo : MonoBehaviour
    {
        public TMPro.TextMeshPro debugText;
        public Renderer hexagonRenderer;
        public SpriteRenderer blockSprite;
        public SpriteRenderer highlightSprite;

        /// <summary>
        /// Set all info then self destory component
        /// </summary>
        public HexagonViewTempInfo GetInfo(ref Bounds bounds, string pdebugText = "")
        {
            debugText.text = pdebugText;
            bounds.Encapsulate(hexagonRenderer.bounds);

            Destroy(this,0.25f);
            return this;
        }
    }
}
