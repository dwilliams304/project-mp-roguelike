using UnityEngine;

namespace ContradictiveGames.UI
{
    public class FaceUIToCamera : MonoBehaviour
    {
        private Transform cameraTransform;

        private void Start() {
            cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if(cameraTransform == null) return;
            transform.LookAt(transform.position + cameraTransform.forward);
        }
    }
}
