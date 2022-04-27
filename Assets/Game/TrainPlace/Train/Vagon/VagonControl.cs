using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VagonControl : MonoBehaviour
{
    private Vagon[] vagons;
    
    private float distance = 9.08f;

    private int nowVagonIndex = 2;
    private int swapVagonIndex = 1;

    private bool startChange1 = false;
    private bool startChange2 = false;

    [SerializeField] private float speedOfSwap;
    [SerializeField] private Minion enemy;
    [SerializeField] private Player player;

    void Start()
    {
        speedOfSwap /= 10.0f;
        vagons = this.GetComponentsInChildren<Vagon>();
    }

    void FixedUpdate()
    {
        if (startChange1)
        {
            changeVagon1();
        }
        if (startChange2)
        {
            changeVagon2();
        }

    }

    void startChangeVagon()
    {
        for (int i = 0; i < 3; i++)
        {
            vagons[i].transform.position -= new Vector3(speedOfSwap * Time.fixedDeltaTime, 0.0f, 0.0f);
        }
    }

    void changeVagon1()
    {
        startChangeVagon();

        if (vagons[swapVagonIndex].transform.position.x <= -5.0f)
        {
            startChange1 = false;

            vagons[swapVagonIndex].transform.position += new Vector3(distance * 1.5f, 0.0f, 0.0f);

            startChange2 = true;
        }
    }

    void changeVagon2()
    {
        startChangeVagon();

        if (vagons[nowVagonIndex].transform.position.x <= -2.75f)        
        {
            if (player.getIsJump() == 0) StartCoroutine(player.JumpAnimation()); 
            if (player.getIsJump() == 2) player.transform.position -= new Vector3(speedOfSwap * Time.fixedDeltaTime, 0.0f, 0.0f);
        }

        if (vagons[nowVagonIndex].transform.position.x <= -(distance / 2))
        {
            startChange2 = false;

            float result = -(distance / 2) - (vagons[nowVagonIndex].transform.position.x);

            vagons[nowVagonIndex].transform.position += new Vector3(result, 0.0f, 0.0f);
            vagons[swapVagonIndex].transform.position += new Vector3(result, 0.0f, 0.0f);
            vagons[3 - nowVagonIndex - swapVagonIndex].transform.position += new Vector3(result, 0.0f, 0.0f);

            player.SetVagon(vagons[3 - nowVagonIndex - swapVagonIndex]);
            enemy.SetVagon(vagons[3 - nowVagonIndex - swapVagonIndex]);

            player.PreStart();
            enemy.PreStart();
        }
    }

    public IEnumerator NextVagon()
    {
        yield return new WaitForSeconds(1.5f);
        yield return new WaitForSeconds(player.ShootAnimation());

        yield return new WaitForSeconds(enemy.DieAnimation());
        yield return new WaitForSeconds(1.0f);

        startChange1 = true;

        nowVagonIndex++;
        if (nowVagonIndex > 2) nowVagonIndex = 0;
        swapVagonIndex++;
        if (swapVagonIndex > 2) swapVagonIndex = 0;

        vagons[3 - nowVagonIndex - swapVagonIndex].vagonCenter.shaking._spacing = vagons[nowVagonIndex].vagonCenter.shaking._spacing;
        vagons[nowVagonIndex].vagonCenter.shaking._spacing = vagons[swapVagonIndex].vagonCenter.shaking._spacing;
    }


    public IEnumerator EndVagon()
    {
        yield return new WaitForSeconds(enemy.ShootAnimation());

        yield return new WaitForSeconds(player.DieAnimation());
    }

}
