using UnityEngine;

namespace ContradictiveGames.UI
{
    public class FaceUIToCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            if(Camera.main == null) return;
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
}
