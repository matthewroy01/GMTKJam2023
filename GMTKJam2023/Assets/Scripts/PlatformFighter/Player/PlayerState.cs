using MHR.StateMachine;
using UnityEngine;

namespace PlatformFighter.Player
{
    public abstract class PlayerState : State
    {
        public PlayerPlatform PlayerPlatform => _playerPlatform;

        private PlayerPlatform _playerPlatform;

        public void SetPlayerPlatform(PlayerPlatform playerPlatform)
        {
            _playerPlatform = playerPlatform;
        }
    
        public abstract override void EnterState();
        public abstract override void ExitState();
        public abstract override void ProcessState();
        public abstract override void ProcessStateFixed();
    }
}
