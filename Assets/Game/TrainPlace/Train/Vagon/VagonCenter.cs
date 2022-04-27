using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VagonCenter : MonoBehaviour
{
    [System.Serializable]
    public class ShakingParm
    {
        public float _spacing;
        public float _speed;
    }

    public ShakingParm shaking;

    private Vector3 startPosition;

    private float result = 0.0f;
    private float definition = 0.0f;

    private Transform center;
    public float ShakingResult
    {
        get
        {
            return result;
        }
    }

    void Start()
    {
        center = this.GetComponentInChildren<IVagonCenter>().transform;
        startPosition = center.position;
    }

    private void Update()
    {
        Shaking();
    }

    public void Shaking()
    {
        definition += Time.deltaTime * shaking._speed;
        result = Mathf.Sin(definition) * shaking._spacing / 100.0f;
        center.position = new Vector3(center.position.x, 0.0f, center.position.z) + new Vector3(0.0f, startPosition.y + result, 0.0f);
    }
}
