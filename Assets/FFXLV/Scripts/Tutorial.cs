using UnityEngine;

namespace FFXLV
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Vector3 firstPosition;
        [SerializeField] private TransformChanger transformChanger;
        public bool IsCompleted { get; private set; }
        private State currentState;
        private enum State
        {
            None,
            Move,
            Game
        }

        public void Initialize()
        {
            currentState = State.Move;
        }

        public void Update()
        {
            switch (currentState)
            {
                case State.Move:
                    var direction = firstPosition - transform.position;
                    if (direction.magnitude > 0.125f)
                    {
                        transform.position += direction * Time.deltaTime;
                    }
                    else
                    {
                        currentState++;
                    }
                    break;
                case State.Game:
                    if (!transformChanger.gameObject.activeSelf)
                    {
                        transformChanger.gameObject.SetActive(true);
                        transformChanger.Initialize(45, 0.2f, 3, 0.4f);
                    }
                    
                    break;
            }
            
        }
    }

}
