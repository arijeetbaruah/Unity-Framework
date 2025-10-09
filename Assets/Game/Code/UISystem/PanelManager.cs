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
        
        public void ClosePanel<TPanel>() where TPanel : BasePanel
        {
            if (TryGetPanel<TPanel>(out TPanel panel))
            {
                panel.OnClose();
                
                if (_panelStack.Contains(panel))
                    _panelStack = new Stack<BasePanel>(_panelStack.Where(p => p != panel));
            }
        }

        public void CloseAllPanels()
        {
            while (_panelStack.Count > 0)
            {
                BasePanel panel = _panelStack.Pop();
                panel.OnClose();
            }
            
            _panelStack.Clear();
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
