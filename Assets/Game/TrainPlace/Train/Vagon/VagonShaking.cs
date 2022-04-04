using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VagonShaking : MonoBehaviour
{
    [SerializeField] private float spacing = 0.0f;
    [SerializeField] private float speed = 0.1f;
    private float def = 0.0f;

    private Vector3 startPosition;

    private float result = 0.0f;

    private void Start()
    {
        startPosition = this.transform.position;
    }

    void Update()
    {
        def += Time.deltaTime * speed;
        result = Mathf.Sin(def) * spacing;
        this.transform.position = startPosition + new Vector3(0.0f, result, 0.0f);
        
        //Debug.Log(result);
        //Debug.Log(Mathf.Sin(Time.deltaTime * speed));
    }
}
