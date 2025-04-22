using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerControls;

namespace ContradictiveGames.Input
{
    public interface IInputReader {
        Vector2 MoveDirection { get; }
        Vector2 LookDirection { get; }
        void EnablePlayerActions();
    }

    [CreateAssetMenu(fileName = "InputReader", menuName = "Custom/Input/Input Reader")]
    public class InputReader : ScriptableObject, IInputReader, IGameActions
    {

#region Input Events

        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2> Look = delegate { };

        public event UnityAction Interact = delegate { };

        public event UnityAction MainAttack = delegate { };
        public event UnityAction SecondaryAttack = delegate { };
        public event UnityAction Ability1 = delegate { };
        public event UnityAction Ability2 = delegate { };
        public event UnityAction Ability3 = delegate { };

#endregion

        public PlayerControls playerActions;

        public Vector2 MoveDirection => playerActions.Game.Move.ReadValue<Vector2>();
        public Vector2 LookDirection => playerActions.Game.Look.ReadValue<Vector2>();

        public void EnablePlayerActions()
        {
            if(playerActions == null){
                playerActions = new PlayerControls();
                playerActions.Game.SetCallbacks(this);
            }
            playerActions.Enable();
        }

#region Movement Controls

        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>());
        }

#endregion

#region Combat Controls

        public void OnMainAttack(InputAction.CallbackContext context)
        {
            if(context.performed) MainAttack.Invoke();
        }

        public void OnSecondaryAttack(InputAction.CallbackContext context)
        {
            if(context.performed) SecondaryAttack.Invoke();
        }

        public void OnAbility1(InputAction.CallbackContext context){
            if(context.performed) Ability1.Invoke();
        }
        public void OnAbility2(InputAction.CallbackContext context){
            if(context.performed) Ability2.Invoke();
        }
        public void OnAbility3(InputAction.CallbackContext context){
            if(context.performed) Ability3.Invoke();
        }

#endregion

#region Misc. Controls

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed) Interact.Invoke();
        }

#endregion

    }
}
