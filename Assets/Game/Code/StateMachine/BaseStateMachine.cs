using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Baruah.StateMachine
{
    public interface IState
    {
        /// <summary>
/// Called when the state becomes active to perform initialization or setup.
/// </summary>
void OnEnter();
        /// <summary>
/// Performs the state's per-update logic; called repeatedly while the state is active (for example, once per frame or tick).
/// </summary>
void OnUpdate();
        /// <summary>
/// Called when the state is exited to perform teardown and release resources.
/// </summary>
void OnExit();
    }
    
    public abstract class BaseStateMachine
    {
        protected List<IState> _currentStates = new();

        /// <summary>
        /// Initializes the state machine and starts the provided initial state.
        /// </summary>
        /// <param name="startingState">The initial state to activate; must not be null. The state will be added to the machine and entered.</param>
        public BaseStateMachine(IState startingState)
        {
            StartState(startingState);
        }

        /// <summary>
        /// Starts and activates the specified state for this state machine.
        /// </summary>
        /// <param name="nextState">The state to start and activate; must not be null.</param>
        /// <exception cref="System.Exception">Thrown if <paramref name="nextState"/> is null.</exception>
        /// <remarks>
        /// If the state is already active, the method returns without starting it again. When started, the state's <c>OnEnter</c> method is invoked.
        /// </remarks>
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

        /// <summary>
        /// Ends the given active state by invoking its exit behavior and removing it from the active states list.
        /// </summary>
        /// <param name="state">The state instance to end; must be non-null and currently managed by the state machine.</param>
        /// <exception cref="System.Exception">Thrown when <paramref name="state"/> is null.</exception>
        public void EndState<TState>(TState state) where TState : IState
        {
            if (state == null)
            {
                throw new Exception("Can't start a null state");
            }
            
            state.OnExit();
            _currentStates.Remove(state);
        }

        /// <summary>
        /// Determines whether the state machine currently contains an active state of the specified type.
        /// </summary>
        /// <returns>True if any active state is of type <typeparamref name="TState"/>, false otherwise.</returns>
        public bool HasState<TState>() where TState : IState
        {
            return _currentStates.Any(s => s is TState);
        }

        /// <summary>
        /// Calls <c>OnUpdate</c> on each active state in the state machine.
        /// </summary>
        /// <remarks>
        /// Iterates over a snapshot of the active states so states can be started or ended during updates without causing enumeration errors.
        /// </remarks>
        public void Update()
        {
            foreach (var state in _currentStates.ToList())
            {
                state.OnUpdate();
            }
        }
    }
}
