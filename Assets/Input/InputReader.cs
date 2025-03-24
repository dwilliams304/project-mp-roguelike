using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerControls;

namespace ContradictiveGames.Input
{
    public interface IInputReader {
        Vector2 MoveDirection { get; }
        Vector2 MousePosition { get; }
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
        public event UnityAction Ability = delegate { };

#endregion

        public PlayerControls playerActions;

        public Vector2 MoveDirection => playerActions.Game.Move.ReadValue<Vector2>();
        public Vector2 MousePosition => playerActions.Game.Look.ReadValue<Vector2>();

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

        public void OnAbility(InputAction.CallbackContext context)
        {
            if(context.performed) Ability.Invoke();
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
