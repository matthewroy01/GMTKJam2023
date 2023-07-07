using MHRUtil.Singletons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlatformFighter.Input
{
    public class InputManager : Singleton<InputManager>, PlatformFighterControls.IFightingActions
    {
        public Vector2 InputDirection => _inputDirection;
        
        private Vector2 _inputDirection;
        private PlatformFighterControls _platformFighterControls;

        protected override void Awake()
        {
            base.Awake();
            
            _platformFighterControls = new PlatformFighterControls();
            _platformFighterControls.Fighting.SetCallbacks(this);
            _platformFighterControls.Enable();
        }
        
        public void OnMovement(InputAction.CallbackContext context)
        {
            _inputDirection = context.ReadValue<Vector2>();
        }
    }
}