using System;
using MHRUtil.Singletons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlatformFighter.Input
{
    public class InputManager : Singleton<InputManager>, PlatformFighterControls.IFightingActions
    {
        public event Action DashPressed;
        public event Action Ability1Pressed;
        public event Action Ability2Pressed;
        public event Action Ability3Pressed;
        
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

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            
            DashPressed?.Invoke();
        }

        public void OnAbility1(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            
            Ability1Pressed?.Invoke();
        }

        public void OnAbility2(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            
            Ability2Pressed?.Invoke();
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            
            Ability3Pressed?.Invoke();
        }
    }
}