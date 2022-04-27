using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [SerializeField] private bool SetStartGame;
    [SerializeField] private ShotControl shotControl;
    [SerializeField] private SkullScore skullScore;
    [SerializeField] private VagonControl vagonControl;

   
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
            StartCoroutine(PlayerDeath());
        });

        StartCoroutine(ChangeVagon());
    }

    IEnumerator ChangeVagon()
    {
        skullScore.Score = vagonOfPlayer;
        skullScore.skullAnimationOpen();

        yield return StartCoroutine(vagonControl.NextVagon());
        yield return new WaitForSeconds(5.0f);


        skullScore.skullAnimationClose();


        if (SetStartGame)
        {
            shotControl.PreStartShot();
        }
    }

    IEnumerator PlayerDeath()
    {
        skullScore.EndText = "Game Over. Your Score: " + vagonOfPlayer.ToString();
        skullScore.skullAnimationOpen();

        yield return new WaitForSeconds(1.5f);
      
        yield return StartCoroutine(vagonControl.EndVagon());
    }
}


