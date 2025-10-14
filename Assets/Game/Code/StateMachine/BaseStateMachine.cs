using System.Collections.Generic;

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
            _currentStates.Add(startingState);
        }

        public void StartState<TState>(TState nextState) where TState : IState
        {
            _currentStates.Add(nextState);
            nextState.OnEnter();
        }

        public void EndState<TState>(TState nextState) where TState : IState
        {
            _currentStates.Remove(nextState);
            nextState.OnExit();
        }

        public void Update()
        {
            foreach (var state in _currentStates)
            {
                state.OnUpdate();
            }
        }
    }
}
