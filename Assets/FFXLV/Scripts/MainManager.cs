using FFXLV.Enum;
using UnityEngine;

namespace FFXLV
{
    public class MainManager : MonoBehaviour
    {
        public GameState GameState { get; private set; }
        
        private void Start()
        {
        }
        
        private void Update()
        {
            switch (GameState)
            {
                case GameState.Title:
                    break;
                case GameState.Tutorial:
                    break;
                case GameState.MainGame:
                    break;
                case GameState.Result:
                    break;
            }
        }
    }

}
