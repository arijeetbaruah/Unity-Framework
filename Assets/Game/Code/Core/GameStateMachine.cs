using Baruah.Service;
using Baruah.StateMachine;
using UnityEngine;

namespace Baruah.Core
{
    public class GameStateMachine : BaseStateMachine
    {
        public GameStateMachine() : base(new GlobalState())
        {
            StartState(new MainMenuState());
        }
    }
}
