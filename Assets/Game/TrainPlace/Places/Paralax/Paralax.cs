using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    private GameObject[] paralaxObjects;

    private int lastIndex = 0;

    [Range(-1,1)][SerializeField] private int direction = 1;
    [SerializeField] private float speed = 25.0f;
    [SerializeField] private float deltaSpeed = 100.0f;


    [SerializeField] private Transform posA, posB;
    void Awake()
    {
        IParalax[] temp = this.GetComponentsInChildren<IParalax>();

        paralaxObjects = new GameObject[temp.Length];

        for (int i = 0; i < temp.Length; i++)
        {
            paralaxObjects[i] = temp[i].gameObject;
        }

        speed /= 10.0f;
    }
    
    void Update()
    {
        for (float j = 0; j < speed; j += speed / deltaSpeed)
        {

            for (int i = 0; i < this.transform.childCount; i++)
            {
                paralaxObjects[i].transform.position += new Vector3(direction * (speed / deltaSpeed) * Time.deltaTime, 0.0f, 0.0f);

            }

            if (paralaxObjects[lastIndex].transform.position.x < posB.position.x)
            {
                paralaxObjects[lastIndex].transform.position = new Vector3(posA.position.x,
                                                                           paralaxObjects[lastIndex].transform.position.y, 
                                                                           paralaxObjects[lastIndex].transform.position.z);

                if (++lastIndex >= this.transform.childCount) lastIndex = 0;
            }

        }
    }
}
