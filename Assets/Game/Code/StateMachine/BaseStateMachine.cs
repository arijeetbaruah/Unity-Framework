using System.Collections.Generic;
using System.Linq;

namespace Baruah.StateMachine
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
    
    public abstract class BaseStateMachine
    {
        protected List<IState> _currentStates = new();

        public BaseStateMachine(IState startingState)
        {
            StartState(startingState);
        }

        public void StartState<TState>(TState nextState) where TState : IState
        {
            _currentStates.Add(nextState);
            nextState.OnEnter();
        }

        public void EndState<TState>(TState nextState) where TState : IState
        {
            nextState.OnExit();
            _currentStates.Remove(nextState);
        }

        public bool HasState<TState>() where TState : IState
        {
            return _currentStates.Any(s => s is TState);
        }

        public void Update()
        {
            foreach (var state in _currentStates.ToList())
            {
                state.OnUpdate();
            }
        }
    }
}
