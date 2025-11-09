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
            
        }

        public void OnDestroy()
        {
            _stateMachine = null;
        }
        
        public GameStateMachine GetStateMachine() => _stateMachine;
    }
}
