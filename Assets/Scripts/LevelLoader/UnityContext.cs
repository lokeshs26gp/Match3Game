using System;
using UnityEngine;
namespace LevelLoaderSystem
{
    /// <summary>
    /// Responsible for looping/updating actions from different game systems 
    /// </summary>
    public class UnityContext : MonoBehaviour
    {
        private event Action<float> updateLoopEvent;

        public void AddListener(Action<float> listener)
        {
            updateLoopEvent += listener;
        }
        public void RemoveAllListeners()
        {
            updateLoopEvent = null;
        }
        void Update()
        {
            updateLoopEvent.Invoke(Time.deltaTime);
        }

        public void RemoveListener(Action<float> listener)
        {
            updateLoopEvent -= listener;
        }
        private void OnDestroy()
        {
            updateLoopEvent = null;
        }
    }
}
