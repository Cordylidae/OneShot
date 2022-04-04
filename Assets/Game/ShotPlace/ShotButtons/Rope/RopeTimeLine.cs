using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTimeLine : MonoBehaviour
{
    [SerializeField]  private float timeInSeconds = 60.0f; 
    private Animation animationRope;

    private bool isStarted = false;

    void Awake()
    {
        animationRope = this.GetComponent<Animation>();

        animationRope.wrapMode = WrapMode.Once;
        animationRope["TimeLineRope"].speed = (60.0f)/timeInSeconds;
    }

    void Update()
    {
        if (!animationRope.isPlaying && isStarted) EndTime();
    }

    public void ReStartRopeTime(bool setTime = false, float time = 20.0f)
    {

        animationRope.Play("StartRope");

        if (!setTime) time = timeInSeconds;
        animationRope["TimeLineRope"].speed = (60.0f) / time;
    }

    public void StartRopeTime()
    {
        animationRope.Play("TimeLineRope");

        isStarted = true;
    }

    public void StopRopeTime()
    {
        isStarted = false;
        animationRope.Stop();
    }


    public void addTimeRope(float seconds)
    {
        animationRope["TimeLineRope"].time += seconds * animationRope["TimeLineRope"].speed;
    }

    public Trigger.TriggerEvent EndGameTime = new Trigger.TriggerEvent();
    public void EndTime()
    {

        isStarted = false;

        EndGameTime.Invoke();
    }
}
