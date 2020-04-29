using UnityEngine;

namespace InputSystem
{
    /// <summary>
    /// Responsible for generating collider around hexagon grid
    /// </summary>
    public class ColliderGenerator
    {
        private GameObject colliderObject;

        public ColliderGenerator(Bounds bounds)
        {
            colliderObject = new GameObject("ColliderObject");

            colliderObject.transform.position = bounds.center;

            BoxCollider collider = colliderObject.AddComponent<BoxCollider>();

            collider.size = bounds.size;


        }
    }
}
