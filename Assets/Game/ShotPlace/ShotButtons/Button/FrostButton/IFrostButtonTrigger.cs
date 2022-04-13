using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class IFrostButtonTrigger : Trigger
{
    [SerializeField] private Image[] cracks;
    [SerializeField] private int durability = 4;
    public int Durability
    {
        set
        {
            durability = value;
            if (durability == 0) { Debug.Log("Broke Frost Button"); OnButtonBroken(); }
            else if (durability < 0) { throw new Exception(); }

            ChangeSprites();
        }
        get
        {
            return durability;
        }
    }

    public void OnButtonClick()
    {
        Durability--;
    }

    public void OnButtonBroken()
    {
        onTrigger.Invoke();
    }

    private void ChangeSprites()
    {
        switch(durability)
        {
            case 1:
                {
                    cracks[0].gameObject.SetActive(true);
                    cracks[1].gameObject.SetActive(true);
                    cracks[2].gameObject.SetActive(true);
                }break;
            case 2:
                {
                    cracks[0].gameObject.SetActive(true);
                    cracks[1].gameObject.SetActive(true);
                    cracks[2].gameObject.SetActive(false);
                }
                break;
            case 3:
                {
                    cracks[0].gameObject.SetActive(true);
                    cracks[1].gameObject.SetActive(false);
                    cracks[2].gameObject.SetActive(false);
                }
                break;
            default:
                {
                    cracks[0].gameObject.SetActive(false);
                    cracks[1].gameObject.SetActive(false);
                    cracks[2].gameObject.SetActive(false);
                }
                break;
        }
    }
}
