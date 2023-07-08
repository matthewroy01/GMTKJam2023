using MHR.StateMachine;

namespace PlatformFighter.Character
{
    public abstract class CharacterState : State
    {
        public Character Character => _character;
        private Character _character;

        public void SetCharacter(Character character)
        {
            _character = character;
        }

        public abstract override void EnterState();

        public abstract override void ExitState();

        public abstract override void ProcessState();

        public abstract override void ProcessStateFixed();
    }
}