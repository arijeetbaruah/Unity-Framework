using System.Collections.Generic;

namespace Baruah.StateMachine
{
    public interface IState
    {
        /// <summary>
/// Invoked when the state becomes active; perform initialization or enter-time logic.
/// </summary>
void OnEnter();
        /// <summary>
/// Performs the state's periodic update logic.
/// </summary>
/// <remarks>
/// Invoked by the state machine during its update cycle while the state is active.
/// </remarks>
void OnUpdate();
        /// <summary>
/// Performs teardown when the state is deactivated.
/// </summary>
/// <remarks>
/// Implementations should undo any setup performed in <c>OnEnter</c>, stop ongoing behaviors, and release resources held by the state.
/// </remarks>
void OnExit();
    }
    
    public abstract class BaseStateMachine
    {
        protected List<IState> _currentStates = new();

        /// <summary>
        /// Initializes the state machine and registers the provided state as the initial active state.
        /// </summary>
        /// <param name="startingState">The state to add to the machine as the starting active state.</param>
        public BaseStateMachine(IState startingState)
        {
            _currentStates.Add(startingState);
        }

        /// <summary>
        /// Activates and begins tracking the specified state.
        /// </summary>
        /// <typeparam name="TState">The concrete IState type to start.</typeparam>
        /// <param name="nextState">The state instance to add to the active states; its <c>OnEnter</c> method will be invoked.</param>
        public void StartState<TState>(TState nextState) where TState : IState
        {
            _currentStates.Add(nextState);
            nextState.OnEnter();
        }

        /// <summary>
        /// Deactivates the specified state by removing it from the active states and invoking its exit lifecycle.
        /// </summary>
        /// <typeparam name="TState">The concrete state type being ended.</typeparam>
        /// <param name="nextState">The active state instance to remove and exit.</param>
        public void EndState<TState>(TState nextState) where TState : IState
        {
            _currentStates.Remove(nextState);
            nextState.OnExit();
        }

        /// <summary>
        /// Invokes <see cref="IState.OnUpdate"/> on every active state in the state machine.
        /// </summary>
        public void Update()
        {
            foreach (var state in _currentStates)
            {
                state.OnUpdate();
            }
        }
    }
}