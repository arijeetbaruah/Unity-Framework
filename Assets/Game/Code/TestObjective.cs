using Baruah.Inputs;
using Baruah.Mission;
using Baruah.Service;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestObjective : IntObjective
{
    public override void Hookup()
    {
        ServiceManager.Get<InputService>().UI.Click.performed += OnClick;
    }

    public override void UnHookup()
    {
        ServiceManager.Get<InputService>().UI.Click.performed -= OnClick;
    }

    private void OnClick(InputAction.CallbackContext obj)
    {
        Update();
    }

    public override bool Valid()
    {
        return true;
    }
}
