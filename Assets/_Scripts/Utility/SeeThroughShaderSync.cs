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

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(cameraTransform.position, transform.position);
            
        }

        private void Update()
        {
            Vector3 origin = cameraTransform.position;
            Vector3 dir = cameraTransform.position - transform.position;
            float distance = Vector3.Distance(origin, transform.position);

            if(Physics.Raycast(origin, dir, out RaycastHit hit, distance, WallMask)){
                
                    WallMaterial.SetFloat(SizeID, 1);

            }
            else WallMaterial.SetFloat(SizeID, 0);

            Vector3 view = PlayerCamera.WorldToViewportPoint(transform.position);  
            WallMaterial.SetVector(PositionID, view);  
        }
    }
}
