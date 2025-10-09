using Baruah.Database;
using Baruah.Inputs;
using Baruah.Mission;
using Baruah.Service;
using Baruah.UISystem;
using UnityEngine;

namespace Baruah.SceneManager
{
    public class GlobalManager : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
            ServiceManager.AddService(new InputService());
            ServiceManager.AddService(new DatabaseService());
            ServiceManager.AddService(new GameService());
            ServiceManager.AddService(new PanelManager());
            ServiceManager.AddService(new MissionService());
        }

        private void Update()
        {
            ServiceManager.Update();
        }

        private void OnDestroy()
        {
            ServiceManager.RemoveService<MissionService>();
            ServiceManager.RemoveService<PanelManager>();
            ServiceManager.RemoveService<GameService>();
            ServiceManager.RemoveService<DatabaseService>();
            ServiceManager.RemoveService<InputService>();
        }
    }
}
