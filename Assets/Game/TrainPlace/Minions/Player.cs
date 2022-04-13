using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Minion
{
    [SerializeField] private SpriteRenderer hitInBody;
    protected override void LateUpdate()
    {
        Shaking();
    }

    public override void PreStart()
    {
        startPos = startPosAtStart;

        isShaking = true;
    }

    public override float DieAnimation()
    {
        isShaking = false;

        hitInBody.gameObject.SetActive(true);

        return 0.5f;
    }
}
