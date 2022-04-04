using UnityEngine;
using UnityEngine.UI;
public class ShotControl : MonoBehaviour
{
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject finishGameButton;
    [Space]
    [SerializeField] private RopeTimeLine ropeTimeLine;
    [Space]
    [SerializeField] private GeneratorOfButtons generatorOfButtons;
    [SerializeField] private Image backgroundOfShot;
    [Space]
                     public Trigger.TriggerEvent Win = new Trigger.TriggerEvent();
                     public Trigger.TriggerEvent Lose = new Trigger.TriggerEvent();
    [Space]
    [SerializeField] private int levelOfHard = 1;
    public int LevelOfHard {
        set
        {
            levelOfHard = value;
        }
    }


    void Start()
    {
        generatorOfButtons.SetRopeTimeLine(ropeTimeLine);

        IButtonTrigger   bts = startGameButton.GetComponent<IButtonTrigger>();
                        bts.OnTrigger.AddListener(() =>
                        {
                            StartShot();
                        });

        IButtonTrigger   btf = finishGameButton.GetComponent<IButtonTrigger>();
                        btf.OnTrigger.AddListener(() =>
                        {
                            FinishShot();
                        });

                        ropeTimeLine.EndGameTime.AddListener(() =>
                        {
                            LoseShot();
                        });

                        generatorOfButtons.EndGame.AddListener(() =>
                        {
                            PreFinishShot();
                        });
    }
    
    public void PreStartShot()
    {
        SetUIDefault();
        AlgoritmOfShot();
    }
    #region [Pre Start Shot]
    private void SetUIDefault()
    {

        startGameButton.SetActive(true);

        backgroundOfShot.color = new Color(0.0f, 0.0f, 0.0f, 0.7f);

        ropeTimeLine.gameObject.SetActive(true);
    }

    private void AlgoritmOfShot()
    {
        generatorOfButtons.PreStartGenerate(levelOfHard); // rope reset inside
    }

    #endregion
    
    public void StartShot()
    {
        startGameButton.SetActive(false);

        generatorOfButtons.StartGenerate();

        ropeTimeLine.StartRopeTime();
    }
    #region [Start Shot]
    #endregion

    
    public void PreFinishShot()
    {
        finishGameButton.SetActive(true);
    }
    #region [Pre Finish Shot]
    #endregion

    
    private void FinishShot()
    {
        ropeTimeLine.StopRopeTime();

        AfterEnd();
        Win.Invoke();
    }
    
    public void LoseShot()
    {
        AfterEnd();
        generatorOfButtons.ClearAllShots();
        Lose.Invoke();
    }

    #region [Finish/Lose Shot]
    private void AfterEnd()
    {
        backgroundOfShot.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        ropeTimeLine.gameObject.SetActive(false);

        finishGameButton.SetActive(false);
    }

    #endregion

}
