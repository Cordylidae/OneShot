using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class IButtonTrigger : Trigger
{
    public void OnButtonClick()
    {
        onTrigger.Invoke();
    }
}
