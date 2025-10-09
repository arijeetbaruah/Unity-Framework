using UnityEngine;

namespace Baruah.UISystem
{
    public abstract class BasePanel : MonoBehaviour
    {
        public bool IsStackable => _isStackable;
        
        [SerializeField] protected bool _isStackable = true;
        
        [SerializeField]
        protected GameObject panel;
        
        /// <summary>
/// Performs panel-specific initialization logic.
/// </summary>
/// <remarks>
/// Implementations should set up the panel's initial state and prepare any required resources before the panel is used.
/// </remarks>
public abstract void Initialize();
        /// <summary>
/// Perform panel-specific logic when the panel is opened.
/// </summary>
/// <remarks>
/// Implementations should prepare the panel's visible state (for example: update content, start opening animations, and enable interactive elements).
/// </remarks>
public abstract void OnOpen();
        /// <summary>
/// Performs panel-specific actions when the panel is closed.
/// </summary>
public abstract void OnClose();
        /// <summary>
/// Handles a back-button press for the panel.
/// </summary>
/// <remarks>
/// The default implementation closes the panel by invoking <see cref="OnClose"/>. Override to customize back-press behavior.
/// </remarks>
public virtual void OnBackPressed() => OnClose();
    }
}