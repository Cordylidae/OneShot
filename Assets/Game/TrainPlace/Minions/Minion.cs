using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    [SerializeField] protected Vagon vagonNow;

    protected Vector3 startPos;
    protected Vector3 startPosAtStart;

    protected Animation animation;


    protected bool isShaking = false;

    void Awake()
    {
        startPosAtStart = this.transform.position;

        animation = this.GetComponent<Animation>();

        PreStart();
    }

    public virtual void PreStart()
    {
        startPos = startPosAtStart;

        StartCoroutine(EmergnceAnimation());
    }

    protected virtual void LateUpdate()
    {
        if(isShaking)Shaking();
    }

    public void SetVagon(Vagon vagon)
    {
        vagonNow = vagon;
    }

    public void Shaking()
    {
        transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z) + new Vector3(0.0f, startPos.y + vagonNow.getShaking(), 0.0f);
    }

    public float ShootAnimation()
    {
        animation.Play("ShootAnim");

        return animation["ShootAnim"].length;
    }

    public virtual float DieAnimation()
    {
        animation.Play("DieAnim");

        isShaking = false;

        return animation["DieAnim"].length;
    }

    IEnumerator EmergnceAnimation()
    {
        animation.Play("EmergAnim");
        yield return new WaitForSeconds(animation["EmergAnim"].length);

        isShaking = true;
    }

    public float AnimationTime(string s)
    {
        return animation[s].length;
    }
}
