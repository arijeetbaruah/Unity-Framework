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
        /// <summary>
        /// Initializes the global manager, prevents its destruction on scene load, and registers core runtime services.
        /// </summary>
        /// <remarks>
        /// Registers the following services with ServiceManager: InputService, DatabaseService, GameService, PanelManager, and MissionService.
        /// </remarks>
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

        /// <summary>
        /// Unregisters the scene-level services managed by this instance when it is destroyed.
        /// </summary>
        /// <remarks>Removes services from ServiceManager in the following order: MissionService, PanelManager, GameService, DatabaseService, InputService.</remarks>
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