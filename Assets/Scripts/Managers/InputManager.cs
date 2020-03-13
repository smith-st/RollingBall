using UnityEngine;
using Zenject;

namespace Managers
{
    public class InputManager:ITickable
    {
        private ITouchListener _listener;

        public void Tick()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            {
                SendEvent();
            }
#else
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended){
                SendEvent();
            }
        }
#endif
        }

        public void AddListener(ITouchListener listener)
        {
            _listener = listener;
        }

        private void SendEvent()
        {
            Debug.Log("Touch Registered");
            _listener?.TouchRegistered();
        }
    }
}