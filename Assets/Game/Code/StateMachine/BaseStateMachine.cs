using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            if (nextState == null)
            {
                throw new Exception("Can't start a null state");
            }

            if (_currentStates.Contains(nextState))
            {
                Debug.LogWarning("Trying to start a state that has already been started");
                return;
            }
            
            _currentStates.Add(nextState);
            nextState.OnEnter();
        }

        public void EndState<TState>(TState state) where TState : IState
        {
            if (state == null)
            {
                throw new Exception("Can't start a null state");
            }
            
            state.OnExit();
            _currentStates.Remove(state);
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
