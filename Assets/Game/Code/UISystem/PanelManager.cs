using System;
using System.Collections.Generic;
using System.Linq;
using Baruah.Service;
using UnityEngine;

namespace Baruah.UISystem
{
    public class PanelManager : IService
    {
        private Dictionary<System.Type, BasePanel> _panels = new ();
        
        private Stack<BasePanel> _panelStack = new();
        
        /// <summary>
        /// Discovers all concrete BasePanel subclasses in loaded assemblies and caches their scene instances.
        /// </summary>
        /// <remarks>
        /// Searches all loaded assemblies for non-abstract types deriving from BasePanel, locates a corresponding scene object (including inactive), and stores each found instance in the `_panels` dictionary keyed by its concrete type.
        /// </remarks>
        public void Initialize()
        {
            var panelsType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(BasePanel)) && !type.IsAbstract);

            foreach (var panelType in panelsType)
            {
                BasePanel panel = (BasePanel) GameObject.FindFirstObjectByType(panelType, FindObjectsInactive.Include);
                _panels.Add(panelType, panel);
            }
        }

        /// <summary>
        /// Performs the per-frame update for the PanelManager service. Currently performs no action.
        /// </summary>
        public void Update()
        {
        }

        /// <summary>
        /// Lifecycle hook invoked when the service is destroyed.
        /// </summary>
        /// <remarks>
        /// Called when the PanelManager is being destroyed; currently no cleanup is performed.
        /// </remarks>
        public void OnDestroy()
        {
        }

        /// <summary>
        /// Opens the panel of type <typeparamref name="TPanel"/> if it is registered in the manager.
        /// </summary>
        /// <typeparam name="TPanel">The concrete <see cref="BasePanel"/> type to open.</typeparam>
        /// <remarks>
        /// If the panel's <c>IsStackable</c> property is true, the panel's <c>OnOpen</c> is invoked and the panel is pushed onto the open-panel stack.
        /// If <c>IsStackable</c> is false, all currently stacked panels are closed before invoking the panel's <c>OnOpen</c>.
        /// If no panel instance of the requested type is registered, the method does nothing.
        /// </remarks>
        public void OpenPanel<TPanel>() where TPanel : BasePanel
        {
            if (TryGetPanel<TPanel>(out TPanel panel))
            {
                if (panel.IsStackable)
                {
                    panel.OnOpen();
                    _panelStack.Push(panel);
                }
                else
                {
                    CloseAllPanels();
                    panel.OnOpen();
                }
            }
        }
        
        /// <summary>
        /// Closes the cached panel of the specified type by invoking its close lifecycle and removes it from the open panel stack if present.
        /// </summary>
        /// <remarks>
        /// If no panel instance of the specified type is cached, the method has no effect.
        /// </remarks>
        public void ClosePanel<TPanel>() where TPanel : BasePanel
        {
            if (TryGetPanel<TPanel>(out TPanel panel))
            {
                panel.OnClose();
                
                if (_panelStack.Contains(panel))
                    _panelStack = new Stack<BasePanel>(_panelStack.Where(p => p != panel));
            }
        }

        /// <summary>
        /// Closes every panel currently stored on the internal panel stack.
        /// </summary>
        /// <remarks>
        /// Calls <c>OnClose()</c> for each stacked panel and clears the internal stack so no stacked panels remain.
        /// </remarks>
        public void CloseAllPanels()
        {
            while (_panelStack.Count > 0)
            {
                BasePanel panel = _panelStack.Pop();
                panel.OnClose();
            }
            
            _panelStack.Clear();
        }

        /// <summary>
        /// Attempts to retrieve a cached panel instance for the specified panel type.
        /// </summary>
        /// <typeparam name="TPanel">The concrete panel type to look up; must derive from <see cref="BasePanel"/>.</typeparam>
        /// <param name="panel">When this method returns, contains the panel instance if found, or null otherwise.</param>
        /// <returns>`true` if a panel of the requested type was found and assigned to <paramref name="panel"/>, `false` otherwise.</returns>
        public bool TryGetPanel<TPanel>(out TPanel panel) where TPanel : BasePanel
        {
            if (_panels.TryGetValue(typeof(TPanel), out var bpanel))
            {
                panel = (TPanel) bpanel;
                return true;
            }

            panel = null;
            return false;
        }
    }
}