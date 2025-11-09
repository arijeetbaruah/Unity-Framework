using Baruah.Database;
using Baruah.Inputs;
using Baruah.Service;
using Baruah.StateMachine;
using Baruah.UISystem;
using UnityEngine;

namespace Baruah.Core
{
    public class GlobalState : IState
    {
        public void OnEnter()
        {
            ServiceManager.AddService(new InputService());
            ServiceManager.AddService(new DatabaseService());
            ServiceManager.AddService(new GameService());
            ServiceManager.AddService(new PanelManager());
        }

        public void OnUpdate()
        {
        }

        public void OnExit()
        {
            ServiceManager.RemoveService<PanelManager>();
            ServiceManager.RemoveService<GameService>();
            ServiceManager.RemoveService<DatabaseService>();
            ServiceManager.RemoveService<InputService>();
        }
    }
}
