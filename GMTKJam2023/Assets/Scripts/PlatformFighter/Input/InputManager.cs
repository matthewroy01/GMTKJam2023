using MHRUtil.Singletons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlatformFighter.Input
{
    public class InputManager : Singleton<InputManager>, PlatformFighterControls.IFightingActions
    {
        public Vector2 InputDirection => _inputDirection;
        private Vector2 _inputDirection;
        
        public void OnMovement(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }
    }
}