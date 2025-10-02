using Baruah.Service;
using UnityEngine;

namespace Baruah.Inputs
{
    public class InputService : IService
    {
        private InputActions _actions;
        
        public InputActions.PlayerActions Player => _actions.Player;
        public InputActions.UIActions UI => _actions.UI;
        
        public void Initialize()
        {
            _actions = new InputActions();
        }

        public void Update()
        {
        }

        public void OnDestroy()
        {
            _actions.Dispose();
        }
    }
}
