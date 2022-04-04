using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateWheels : MonoBehaviour
{
    [SerializeField] private float degrees = 360.0f;

    private Transform[] wheels;

    private void Start()
    {
        wheels = this.GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        for (int i = 1;i<wheels.Length;i++)
        {
            wheels[i].transform.Rotate(0.0f, 0.0f, -degrees * Time.deltaTime);
        }
    }
}
