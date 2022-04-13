using UnityEngine;
using UnityEngine.UI;

using System;

public class SkullScore : MonoBehaviour
{
    private Animation animation;
    private Text score;

    public int Score
    {
        set
        {
            if (value < 0) { throw new Exception(); }
            score.text = value.ToString();
        }
    }

    public string EndText
    {
        set
        {
            score.text = value;
            score.fontSize = 14;
        }
    }


    void Start()
    {
        animation = this.GetComponent<Animation>();
        score = this.GetComponentInChildren<Text>();
    }

    public void skullAnimationOpen()
    {
        score.color = Color.black;

        animation.Play("OpenSkull");
    }
    public void skullAnimationClose()
    {
        score.color = Color.white;

        animation.Play("CloseSkull");

    }
}
