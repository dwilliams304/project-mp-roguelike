using UnityEngine;

namespace ContradictiveGames
{
    public class SeeThroughShaderSync : MonoBehaviour
    {
        public static int PositionID = Shader.PropertyToID("_PlayerPosition");
        public static int SizeID = Shader.PropertyToID("_Size");

        [SerializeField] private Material WallMaterial;
        [SerializeField] private Camera PlayerCamera;
        [SerializeField] private LayerMask WallMask;

        private Transform cameraTransform;
        
        
        private void Start(){
            PlayerCamera = Camera.main;
            cameraTransform = PlayerCamera.transform;
        }

        private void Update()
        {
            Vector3 dir = cameraTransform.position - transform.position;
            Ray Ray = new Ray(transform.position, dir.normalized);
            if(Physics.Raycast(Ray, 3000, WallMask)){
                WallMaterial.SetFloat(SizeID, 1);
            }
            else WallMaterial.SetFloat(SizeID, 0);

            Vector3 view = PlayerCamera.WorldToViewportPoint(transform.position);  
            WallMaterial.SetVector(PositionID, view);  
        }
    }
}
