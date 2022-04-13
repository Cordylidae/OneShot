using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vagon : MonoBehaviour
{
    [Space]
    [SerializeField] private VagonCenter vagonCenter;

    [Space][Header("Wheels")]
    [SerializeField] private float rotateDegrees = 720.0f;

    private IWheel[] wheels;

    void Start()
    {
        atStartVagon();
        atStartWheel();
    }

    // Update is called once per frame
    void Update()
    {
        WheelRotation();
        vagonCenter.Shaking();
    }

    #region [Wheel]

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

    #endregion

    #region [Vagon Center]
    private void atStartVagon()
    {
        vagonCenter = new VagonCenter(this.GetComponentInChildren<IVagonCenter>().transform);
    }

    [System.Serializable]
    public class VagonCenter
    {
        [System.Serializable]
        public struct ShakingParm
        {
            public float _spacing;
            public float _speed;
        }

        [SerializeField]
        public ShakingParm shaking;

        private Vector3 startPosition;
        private Transform transform;

        private float result = 0.0f;
        public float ShakingResult
        {
            get
            {
               return result;
            }
        }
        
        private float definition = 0.0f;

        public VagonCenter(Transform target)
        {
            startPosition = target.position;
            transform = target;

            shaking._spacing = 1.75f;
            shaking._speed = 10.0f;
        }

        public void Shaking()
        {
            definition += Time.deltaTime * shaking._speed;
            result = Mathf.Sin(definition) * shaking._spacing / 100.0f;
            transform.position = startPosition + new Vector3(0.0f, result, 0.0f);
        }
    }

    public float getShaking()
    {
        return vagonCenter.ShakingResult;
    }

    #endregion
}
