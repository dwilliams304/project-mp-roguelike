using ContradictiveGames.Managers;
using TMPro;
using UnityEngine;

namespace ContradictiveGames
{
    public class StateTextUpdater : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;

        private void Start(){
            if(GameManager.Instance != null){
                GameManager.Instance.GameStateChanged += OnGameStateChanged;
                OnGameStateChanged(GameManager.Instance.CurrentGameStateType.Value);
            }
        }
        private void OnDisable()
        {
            GameManager.Instance.GameStateChanged -= OnGameStateChanged;
            
        }

        private void OnGameStateChanged(GameStateType state)
        {
            tmpText.text = $"Current State: {state}";
        }
    
    }
}
