using Baruah.Service;

namespace Baruah.Core
{
    public class GameStateMachineService : IService
    {
        private GameStateMachine _stateMachine;
        
        public void Initialize()
        {
            _stateMachine = new GameStateMachine();
        }

        public void Update()
        {
            _stateMachine.Update();
        }

        public void OnDestroy()
        {
            _stateMachine = null;
        }

        public GameStateMachine GetStateMachine()
        {
            if (_stateMachine == null)
            {
                Initialize();
            }
            
            return _stateMachine;
        }
    }
}
