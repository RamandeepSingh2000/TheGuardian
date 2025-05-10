namespace Player
{
    public interface IInjectableInputActions
    {        
        void InjectInputActions(PlayerInputActions inputActions);
        void EnableInputs();
        void DisableInputs();
    }
}