using UnityEngine;

namespace ContradictiveGames.UI
{
    public abstract class GameView : MonoBehaviour
    {
        public CanvasGroup Group;
        public abstract void OnShow();
        public abstract void OnHide();
    }
}