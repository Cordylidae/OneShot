using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Minion
{
    [SerializeField] private SpriteRenderer hitInBody;
    
    private int isJump = 0;

    public override void PreStart()
    {
        startPos = startPosAtStart;

        isJump = 0;
        isShaking = true;
    }
    protected override void LateUpdate()
    {
        if (isShaking) Shaking();
    }

    public override float DieAnimation()
    {
        isShaking = false;

        hitInBody.gameObject.SetActive(true);

        return 0.5f;
    }

    public IEnumerator JumpAnimation()
    {
        isJump = 1;

        isShaking = false;
        animation.Play("JumpAnim");
        yield return new WaitForSeconds(animation["JumpAnim"].length);

        isJump = 2;

        isShaking = true;
    }

    public int getIsJump()
    {
        return isJump;
    }
}
