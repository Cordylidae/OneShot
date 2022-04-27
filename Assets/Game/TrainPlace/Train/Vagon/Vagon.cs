using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vagon : MonoBehaviour
{
    [HideInInspector]
    public VagonCenter vagonCenter;


    [Space][Header("Wheels")]
    [SerializeField] public float rotateDegrees = 720.0f;

    private IWheel[] wheels;

    void Awake()
    {
        vagonCenter = this.GetComponentInChildren<VagonCenter>();
        atStartWheel();
    }

    void Update()
    {
        WheelRotation();
    }

    private void atStartWheel()
    {
        wheels = this.GetComponentsInChildren<IWheel>();
    }

    private void WheelRotation()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].transform.Rotate(0.0f, 0.0f, -rotateDegrees * Time.deltaTime);
        }
    }

    public float getShaking()
    {
        return vagonCenter.ShakingResult;
    }

}