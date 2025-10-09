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
        
        private List<BasePanel> _activePanels = new();
        
        public void Initialize()
        {
            var panelsType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(BasePanel)) && !type.IsAbstract);

            foreach (var panelType in panelsType)
            {
                BasePanel panel = (BasePanel) GameObject.FindFirstObjectByType(panelType, FindObjectsInactive.Include);
                if (panel == null)
                {
                    Debug.LogWarning($"Panel type {panelType.Name} not found in scene.");
                    continue;
                }
                
                panel.Initialize();
                _panels.Add(panelType, panel);
            }
        }

        public void Update()
        {
        }

        public void OnDestroy()
        {
        }

        public void OpenPanel<TPanel>() where TPanel : BasePanel
        {
            if (TryGetPanel<TPanel>(out TPanel panel))
            {
                if (panel.IsStackable)
                {
                    if (!_activePanels.Contains(panel))
                    {
                        panel.OnOpen();
                        _activePanels.Add(panel);
                    }
                }
                else
                {
                    CloseAllPanels();
                    panel.OnOpen();
                    _activePanels.Add(panel);
                }
            }
        }
        
        public void ClosePanel<TPanel>() where TPanel : BasePanel
        {
            if (TryGetPanel<TPanel>(out TPanel panel))
            {
                panel.OnClose();
                
                if (_activePanels.Contains(panel))
                    _activePanels.Remove(panel);
            }
        }

        public void CloseAllPanels()
        {
            foreach (var panel in _activePanels.ToArray())
            {
                panel.OnClose();
            }
            
            _activePanels.Clear();
        }

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
