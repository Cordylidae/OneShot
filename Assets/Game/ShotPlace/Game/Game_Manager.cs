using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [SerializeField] private ShotControl shotControl;

    private int vagonOfPlayer = 0;
    
    //private LineOfHard lineOfHard;

    void Start()
    {
        shotControl.Win.AddListener(() =>
        {
            vagonOfPlayer++;
            Debug.Log(vagonOfPlayer);

           StartCoroutine(ChangeVagon());
        });

        shotControl.Lose.AddListener(() =>
        {
            Debug.Log("Game Over");
        });

        StartCoroutine(ChangeVagon());
    }

    IEnumerator ChangeVagon()
    {
        yield return new WaitForSeconds(0.5f);
        shotControl.PreStartShot();
    }
}
