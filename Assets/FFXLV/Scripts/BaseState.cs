using UnityEngine;

namespace FFXLV
{
    public class BaseState : MonoBehaviour
    {
        public bool IsCompleted { get; protected set; }
        private bool isInitialized;

        public virtual void Initialize()
        {
            IsCompleted = false;
            isInitialized = true;
        }

        public virtual void Run(float deltaTime)
        {
            if (!isInitialized)
            {
                Initialize();
            }
        }
    }
}