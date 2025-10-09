using UnityEngine;

namespace Baruah.UISystem
{
    public abstract class BasePanel : MonoBehaviour
    {
        public bool IsStackable => _isStackable;
        
        [SerializeField] protected bool _isStackable = true;
        
        [SerializeField]
        protected GameObject panel;
        
        public abstract void Initialize();
        public abstract void OnOpen();
        public abstract void OnClose();
        public virtual void OnBackPressed() => OnClose();
    }
}
