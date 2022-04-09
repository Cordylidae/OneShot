using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GeneratorOfButtons : MonoBehaviour
{
    struct SpawnHardPropities
    {
        public int count;

        public int minAtView;
        public int maxAtView;

        public int radiusOfSpawn;
        public int intervalOfSpawn;

        public int pauseSpawnAtView;
        public int partSpawnAtView;

        public bool orderChaos;
    };

    [SerializeField] private ButtonEntities buttonModificator;
    [SerializeField] private GameObject ButtonSimple;
    [SerializeField] private int Count = 12;

    private int levelOfHard;

    private SpawnHardPropities hardPropities = new SpawnHardPropities();

    // For control game
    private int lastIndex;
    private RopeTimeLine rope = null;

    void Start()
    {
            
    }

    public void SetRopeTimeLine(RopeTimeLine ropeTimeLine)
    {
        rope = ropeTimeLine;
    }

    public void PreStartGenerate(int level)
    {
        lastIndex = -1;

        levelOfHard = level;

        hardPropities.count = Count;
        hardPropities.minAtView = 1;
        hardPropities.minAtView = 4;
    }

    List<GameObject> Buttons = new List<GameObject>();
    List<GameObject> CurrentButtons = new List<GameObject>();


    public void StartGenerate()
    {
        for (int i = hardPropities.count - 1; i >= 0; i--)
        {
            GameObject go = Instantiate(ButtonSimple, new Vector3(0.0f,0.0f,0.0f), ButtonSimple.transform.rotation);
            
            go.transform.SetParent(this.transform);
            go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            int index = go.GetComponent<IButtonIndex>().Index = i;

            RectTransform rect = go.GetComponentsInChildren<RectTransform>()[1];

            go.transform.localPosition = new Vector3(
                            Random.RandomRange(-540 + rect.rect.width / 2, 540 - rect.rect.width / 2),
                            Random.RandomRange(-960 + rect.rect.height / 2, 960 - rect.rect.height / 2),
                            0.0f);

            
            go.GetComponentInChildren<IButtonTrigger>().OnTrigger.AddListener(() =>
            {
                 OnShotButtonClick(index);
            });

            Buttons.Add(go);
        }

        OnNextShot();


        float time = (0.7f * Count) + 10.0f;
        rope.ReStartRopeTime(true, time);
    }
    #region [Start Generate]
    #endregion
    
    private void OnNextShot()
    {
        int CountAtNextView = Random.RandomRange(hardPropities.minAtView, hardPropities.maxAtView);

        for (int i = 0; i < CountAtNextView; i++)
        {

            SetButtonType(Buttons[Buttons.Count - 1 - i]);

            CurrentButtons.Add(Buttons[Buttons.Count - 1 - i]);

            StartCoroutine(CreateWithAnim(Buttons[Buttons.Count - 1 - i], (float)(i + 1) * 0.05f));
        }
    }
    #region [On Next Shot]
    private void SetButtonType(GameObject go)
    {
        int index = Random.RandomRange(0, 100);

        if (index < 70)
            go.GetComponent<IButtonType>().buttonType = ButtonType.Simple;
        else
        {
            go.GetComponent<IButtonType>().buttonType = ButtonType.Frost;

            GameObject goFrost = Instantiate(buttonModificator.buttonEntities[0], new Vector3(0.0f, 0.0f, 0.0f), buttonModificator.buttonEntities[0].transform.rotation);

            goFrost.SetActive(true);
            goFrost.transform.SetParent(go.transform);

            goFrost.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            goFrost.transform.localPosition = new Vector3(0.0f, 5.0f, 0.0f);

            goFrost.name = "Button_Type_Frost";

            IFrostButtonTrigger frostTrigger = goFrost.GetComponentInChildren<IFrostButtonTrigger>();

            frostTrigger.OnTrigger.AddListener(() =>
            {
                Destroy(goFrost);
            });

            frostTrigger.Durability = Random.Range(1, 5);
        }
    }
    #endregion
    private void OnShotButtonClick(int indexButton)
    {
        if (indexButton - lastIndex == 1)
        {
            Debug.Log("Click norlmal");

            lastIndex = indexButton;

            GameObject go = Buttons[Buttons.Count-1];
            Buttons.RemoveAt(Buttons.Count - 1);
            CurrentButtons.RemoveAt(CurrentButtons.Count - 1);
            StartCoroutine(DestroyWithAnim(go));

        }
        else if (indexButton - lastIndex == 2)
        {

            float seconds = 2.0f;
            Debug.Log("Click no norlmal - add " + seconds + " sec.");

            rope.addTimeRope(seconds);

            lastIndex = indexButton;


            for (int i = 0; i < 2; i++)
            {
                GameObject go = Buttons[Buttons.Count - 1];
                Buttons.RemoveAt(Buttons.Count - 1);
                CurrentButtons.RemoveAt(CurrentButtons.Count - 1);
                StartCoroutine(DestroyWithAnim(go));
            }

        }
        else if (indexButton - lastIndex > 2)
        {

            float seconds = 1.0f;
            Debug.Log("Miss click - add " + seconds + " sec.");

            rope.addTimeRope(seconds);

        }

        if (Buttons.Count == 0) EndGameShot();
        else if (CurrentButtons.Count == 0) OnNextShot();
    }

    IEnumerator CreateWithAnim(GameObject go, float time)
    {
        Animation anim = go.GetComponent<Animation>();

        yield return new WaitForSeconds(time);

        go.SetActive(true);
        anim.Play("OpenShot");
    }

    IEnumerator DestroyWithAnim(GameObject go)
    {
        Animation anim = go.GetComponent<Animation>();
        anim.Play("CloseShot");

        yield return new WaitForSeconds(anim["CloseShot"].length);

        Destroy(go);
    }

    public void ClearAllShots()
    {
        while(Buttons.Count!=0)
        {
            GameObject go = Buttons[0];
            Buttons.RemoveAt(0);
            Destroy(go);
        }

        CurrentButtons.Clear();
    }

    public Trigger.TriggerEvent EndGame = new Trigger.TriggerEvent();
    public void EndGameShot()
    {
        ClearAllShots();
        EndGame.Invoke();
    }
}
