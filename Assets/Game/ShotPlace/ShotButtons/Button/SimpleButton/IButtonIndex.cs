using UnityEngine;
using UnityEngine.UI;
using System;

public class IButtonIndex : MonoBehaviour
{
    [SerializeField] private Numbers numbers;

    [SerializeField] private int index = 0;

    [SerializeField] private Image[] numeric;

    
    private void Start()
    {
        ChangeSprites();
    }

    public int Index
    {
        set
        {
            index = value;
            if (index < 0) throw new Exception();

            ChangeSprites();
        }
        get
        {
            return index;
        }
    }

    private void ChangeSprites()
    {
        if (index >= 0 && index <= 9)
        {
            numeric[0].gameObject.SetActive(true);
            numeric[1].gameObject.SetActive(false);
            numeric[2].gameObject.SetActive(false);

            numeric[0].sprite = numbers.numbers[index];
            numeric[0].SetNativeSize();

        }
        if (index >= 10 && index <= 99)
        {
            numeric[0].gameObject.SetActive(false);
            numeric[1].gameObject.SetActive(true);
            numeric[2].gameObject.SetActive(true);

            numeric[1].sprite = numbers.numbers[index/10];
            numeric[2].sprite = numbers.numbers[index%10];
            
            numeric[1].SetNativeSize();
            numeric[2].SetNativeSize();
        }
    }



}
