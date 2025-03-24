using ContradictiveGames.Input;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private InputReader inputReader;
        private Vector2 moveInput;
        private Vector3 lookTarget;

        [SerializeField] private Transform playerModelTransform;


        [Header("Look Rotation Settings")]
        [Range(0, 3)] [SerializeField] private float lookSmoothing = 0.15f;

        [Header("Other Settings")]
        [SerializeField] private float interactionRadius;


        [Header("TEST SETTINGS")]
        [SerializeField] private float playerMovementSpeed = 3f;


        private void Awake()
        {
            inputReader.Move += MoveDirection => moveInput = MoveDirection;
            inputReader.Look += RotateCharacter;
            inputReader.EnablePlayerActions();
        }



        private void Update()
        {
            MoveCharacter(CalculateMoveDirection());
        }

        private void MoveCharacter(Vector3 moveDirection)
        {
            transform.Translate(moveDirection * playerMovementSpeed * Time.deltaTime, Space.World);
        }
        private Vector3 CalculateMoveDirection(){
            return Quaternion.Euler(0, 45, 0) * new Vector3(moveInput.x, 0f, moveInput.y);
        }

        private void RotateCharacter(Vector2 mousePosition){
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit)){
                lookTarget = hit.point;
            }
            var lookPositon = lookTarget - transform.position;
            lookPositon.y = 0;
            var rotation = Quaternion.LookRotation(lookPositon);

            if(new Vector3(lookTarget.x, 0f, lookTarget.z) != Vector3.zero){
                playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, rotation, lookSmoothing);
            }
        }
    }
}
