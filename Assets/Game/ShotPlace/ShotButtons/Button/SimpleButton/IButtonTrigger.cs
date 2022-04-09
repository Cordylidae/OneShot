using UnityEngine;
using UnityEngine.Events;

public class IButtonTrigger : Trigger
{
    public void OnButtonClick()
    {
        onTrigger.Invoke();
    }
}
