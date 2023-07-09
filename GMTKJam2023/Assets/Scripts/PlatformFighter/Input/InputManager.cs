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
        public event Action Ability1Released;
        public event Action Ability2Pressed;
        public event Action Ability2Released;
        public event Action Ability3Pressed;
        
        public Vector2 InputDirection => _inputDirection;
        public float InputDirectionHorizontal => _inputDirectionHorizontal;
        
        private Vector2 _inputDirection;
        private float _inputDirectionHorizontal;
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
            
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            
            DashPressed?.Invoke();
        }

        public void OnAbility1(InputAction.CallbackContext context)
        {
            if (context.performed)
                Ability1Pressed?.Invoke();
            
            if (context.canceled)
                Ability1Released?.Invoke();
        }

        public void OnAbility2(InputAction.CallbackContext context)
        {
            if (context.performed)
                Ability2Pressed?.Invoke();
            
            if (context.canceled)
                Ability2Released?.Invoke();
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            
            Ability3Pressed?.Invoke();
        }

        public void OnMovementHorizontal(InputAction.CallbackContext context)
        {
            _inputDirectionHorizontal = context.ReadValue<float>();
        }
    }
}