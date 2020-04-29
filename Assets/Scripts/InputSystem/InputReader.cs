using System;
using UnityEngine;
namespace InputSystem
{
    public enum MouseButtonState { Down, Drag, Up }
    /// <summary>
    /// Responsible for processing mouse click events
    /// </summary>
    public class InputReader
    {

        public static Action<RaycastHit, MouseButtonState, bool> OnClickEvent;//bool true no hit

        private Camera eventCameracache;
        public InputReader(Camera camera)
        {
            eventCameracache = camera;
        }
        /// <summary>
        /// subscriped from UnityContext for update call
        /// </summary>
        public void UpdateLoop(float deltaTime)
        {
#if (UNITY_IOS || UNITY_ANDROID ) && !UNITY_EDITOR
            TouchInput();
#else
            MouseInput();
#endif
        }
        private void MouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                InputProcess(MouseButtonState.Down, Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                InputProcess(MouseButtonState.Drag, Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                InputProcess(MouseButtonState.Up, Input.mousePosition);
            }
        }
        private void TouchInput()
        {
            if (Input.touchCount <= 0) return;
            
            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    InputProcess(MouseButtonState.Down, Input.touches[0].position);
                    break;
                case TouchPhase.Moved:
                    InputProcess(MouseButtonState.Drag, Input.touches[0].position);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    InputProcess(MouseButtonState.Up, Input.touches[0].position);
                    break;
            }

        }
        /// <summary>
        /// Ray casting from camera to get hit point on hexagon grid
        /// </summary>
        /// <param name="state"></param>
        private void InputProcess(MouseButtonState state,Vector3 position)
        {
            Ray ray = eventCameracache.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hitinfo, 500))
            {

                if (hitinfo.collider)
                {
                   OnClickEvent(hitinfo, state, false);
                }
                else
                    OnClickEvent(hitinfo, state, true);
            }
            else
                OnClickEvent(hitinfo, state, true);
        }
    }
}
