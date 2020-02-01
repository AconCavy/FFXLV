using UnityEngine;

namespace FFXLV
{
    public class LayerBehaviour : BaseState
    {
        [SerializeField] private GameObject normalStateObject;
        [SerializeField] private GameObject clearStateObject;
        [SerializeField] private GameObject failedStateObject;
        [SerializeField] private HandBehaviour handBehaviour;

        public bool IsFailed { get; private set; }
        public int Count { get; private set; }
        private State currentState;

        private enum State
        {
            None,
            Normal,
            Clear,
            Failed
        }

        private void ChangeState(State state)
        {
            if (state.Equals(currentState)) return;
            currentState = state;
            switch (state)
            {
                case State.Normal:
                    normalStateObject.SetActive(true);
                    clearStateObject.SetActive(false);
                    failedStateObject.SetActive(false);
                    break;
                case State.Clear:
                    normalStateObject.SetActive(false);
                    clearStateObject.SetActive(true);
                    failedStateObject.SetActive(false);
                    break;
                case State.Failed:
                    normalStateObject.SetActive(false);
                    clearStateObject.SetActive(false);
                    failedStateObject.SetActive(true);
                    break;
            }
        }

        public int GetScore()
        {
            return Mathf.Clamp(100 - 10 * Count, 0, 100);
        }

        public void Initialize(float bestAngle, float angularMagnitude, float bestDistance, float distanceMagnitude)
        {
            Initialize();
            handBehaviour.Initialize(bestAngle, angularMagnitude, bestDistance, distanceMagnitude);
        }

        public override void Initialize()
        {
            base.Initialize();
            ChangeState(State.Normal);
            transform.position = Vector3.zero;
        }

        public override void Run(float deltaTime)
        {
            base.Run(deltaTime);
            if (!handBehaviour.IsCompleted) return;
            IsCompleted = true;
            Count = handBehaviour.Count;
            IsFailed = handBehaviour.IsFailed;
        }
    }
}