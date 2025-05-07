using UnityEngine;

namespace ContradictiveGames.Effects
{
    public class SeeThroughShaderSync : MonoBehaviour
    {
        public static int PositionID = Shader.PropertyToID("_PlayerPosition");
        public static int SizeID = Shader.PropertyToID("_Size");
        public static int OpacityID = Shader.PropertyToID("_Opacity");

        [SerializeField] private Material WallMaterial;
        [SerializeField] private LayerMask WallMask;

        [SerializeField] private float transitionDuration = 1f;
        private Camera PlayerCamera;
        private float transitionTimer = 0f;
        private float currentValue = 0f;

        private bool isObstructed = false;
        
        private void OnEnable(){
            PlayerCamera = Camera.main;

            if(PlayerCamera == null) Debug.Log("Player Camera set to null!");
            if(Camera.main == null) Debug.Log("Camera.main is null!");
        }

        private void Update()
        {

            bool hitWall = CheckForObstructions();

            if(isObstructed != hitWall){
                isObstructed = hitWall;
                transitionTimer = 0f;
            }

            if(transitionTimer < transitionDuration){
                transitionTimer += Time.deltaTime;
                float t = Mathf.Clamp01(transitionTimer / transitionDuration);
                float target = isObstructed ? 1.5f : 0f;

                currentValue = Mathf.Lerp(currentValue, target, t);

                WallMaterial.SetFloat(SizeID, currentValue);
                WallMaterial.SetFloat(OpacityID, currentValue);
            }
            

            Vector3 view = PlayerCamera.WorldToViewportPoint(transform.position);  
            WallMaterial.SetVector(PositionID, view);  
        }

        private bool CheckForObstructions(){
            Vector3 dir = PlayerCamera.transform.position.WithY(0.5f) - transform.position;
            Ray ray = new Ray(transform.position, dir.normalized);
            if(Physics.Raycast(ray, 3000, WallMask)) return true;
            else return false;
        }
    }
}
