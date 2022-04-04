using UnityEngine;
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


    [SerializeField] private GameObject ButtonEntity;

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

        hardPropities.count = 12;

        

        float time = 30.0f;
        rope.ReStartRopeTime(true, time);
    }

    List<GameObject> Buttons = new List<GameObject>();

    public void StartGenerate()
    {
        for (int i = hardPropities.count - 1; i >= 0; i--)
        {
            GameObject go = Instantiate(ButtonEntity, new Vector3(0.0f,0.0f,0.0f), ButtonEntity.transform.rotation);
            
            go.transform.SetParent(this.transform);
            go.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            int index = go.GetComponent<IButtonIndex>().Index = i;

            RectTransform rect = go.GetComponent<RectTransform>();
            go.transform.localPosition = new Vector3(
                            Random.RandomRange(-540 + rect.rect.width / 2, 540 - rect.rect.width / 2),
                            Random.RandomRange(-960 + rect.rect.height / 2, 960 - rect.rect.height / 2),
                            0.0f);

            go.GetComponent<IButtonTrigger>().OnTrigger.AddListener(() =>
            {
                 OnShotButtonClick(index);
            });
            
            Buttons.Add(go);
        }
    }

    private void OnShotButtonClick(int indexButton)
    {
        if (indexButton - lastIndex == 1)
        {
            Debug.Log("Click norlmal");

            lastIndex = indexButton;

            GameObject go = Buttons[Buttons.Count-1];
            Buttons.RemoveAt(Buttons.Count - 1);
            Destroy(go);
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
                Destroy(go);
            }

        }
        else if (indexButton - lastIndex > 2)
        {

            float seconds = 1.0f;
            Debug.Log("Miss click - add " + seconds + " sec.");

            rope.addTimeRope(seconds);

        }

        if (Buttons.Count == 0) EndGameShot();
    }

    public void ClearAllShots()
    {
        while(Buttons.Count!=0)
        {
            GameObject go = Buttons[0];
            Buttons.RemoveAt(0);
            Destroy(go);
        }
    }

    public Trigger.TriggerEvent EndGame = new Trigger.TriggerEvent();
    public void EndGameShot()
    {
        EndGame.Invoke();
    }
}
