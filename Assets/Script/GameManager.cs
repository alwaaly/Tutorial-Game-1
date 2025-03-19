using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] int Point;
    [SerializeField] bool canGenerate = true;
    [SerializeField] Vector2 topRight;
    [SerializeField] Vector2 downLeft;
    //[SerializeField] int ItemsCount;
    public static GameManager Instance;
    [SerializeField] TextMeshProUGUI pointTextMesh;
    [SerializeField] TextMeshProUGUI timeTextMesh;
    [SerializeField] float levelTime = 120;
    [SerializeField] int levelTargetPoint = 500;
    [Header("ItemRefrence")]
    [SerializeField] Item[] gold;
    [SerializeField] int goldCount;
    [SerializeField] Item[] stone;
    [SerializeField] int stoneCount;
    [SerializeField] Item[] jewels;
    [SerializeField] int jewelsCount;
    [SerializeField] Item[] star;
    [SerializeField] int starCount;
    float remainingTime;

    float lastTimeUpdate;

    public event System.Action OnTimeEnd;

    [SerializeField] int ItemsPoint;
    private void Awake() {
        if (Instance == null) Instance = this;
        else Debug.LogError("There is more then one GameManager");
        if(canGenerate) GenerateItem();
        remainingTime = levelTime;
        //pointTextMesh.BestResult = Point + "/" + levelTargetPoint;
        lastTimeUpdate = -10;
        levelTargetPoint = ItemsPoint;
        pointTextMesh.text = Point + "/" + levelTargetPoint;
    }
    bool isTimeEnd;
    private void Update() {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0) {
            if (!isTimeEnd) {
                OnTimeEnd?.Invoke();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                isTimeEnd = true;
            }
            return;
        }
        if (Time.time >= lastTimeUpdate + 1) {
            timeTextMesh.text = TimeFormat(remainingTime);
            lastTimeUpdate = Time.time;
        }
    }
    private string TimeFormat(float time) {
        int m = Mathf.FloorToInt(time / 60);
        int s = Mathf.FloorToInt(time - (m * 60));
        if(s >= 10) return m + ":" + s;
        else return m + ":0" + s;
    }
    public void AddPoint(int Amount) {
        Point += Amount;
        pointTextMesh.text = Point + "/" + levelTargetPoint;
        if (Point >= levelTargetPoint) {
            TimeCollector.Instance.SceneCompleted(Mathf.Abs(remainingTime - levelTime));
            SaveSystem.Save(0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    List<Item> GeneratedItems;
    [ContextMenu("GenerateItem")]
    private void GenerateItem() {
        GeneratedItems = new();
        for (int i = 0; i < goldCount; i++) {
            Test(gold);
        }
        for (int i = 0; i < stoneCount; i++) {
            Test(stone);
        }
        for (int i = 0; i < jewelsCount; i++) {
            Test(jewels);
        }
        for (int i = 0; i < starCount; i++) {
            Test(star);
        }
    }

    private void Test(Item[] items) {
        Item item = Instantiate(items[Random.Range(0, items.Length)]);
        if (item.CanRotate) item.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        item.transform.localScale = Vector3.one * Random.Range(item.minMaxScale.x, item.minMaxScale.y);
        item.transform.position = new Vector3(Random.Range(downLeft.x, topRight.x), Random.Range(downLeft.y, topRight.y), 0);
        bool IsOverlapWithOtherObject = false;
        for (int j = 0; j < GeneratedItems.Count; j++) {
            if ((GeneratedItems[j].transform.position - item.transform.position).magnitude < 1f) {
                IsOverlapWithOtherObject = true;
                break;
            }
        }
        while (IsOverlapWithOtherObject) {
            IsOverlapWithOtherObject = false;
            item.transform.position = new Vector3(Random.Range(downLeft.x, topRight.x), Random.Range(downLeft.y, topRight.y), 0);
            for (int j = 0; j < GeneratedItems.Count; j++) {
                if ((GeneratedItems[j].transform.position - item.transform.position).magnitude < GeneratedItems[j].OverlapingSpace * GeneratedItems[j].transform.lossyScale.x) {
                    IsOverlapWithOtherObject = true;
                    break;
                }
            }
        }
        GeneratedItems.Add(item);
        ItemsPoint += item.basePoint;
        //do {
        //    item.transform.position = new Vector3(Random.Range(downLeft.x, topRight.x), Random.Range(downLeft.y, topRight.y), 0);
        //} while (item.IsOverlapWithOtherObject());
    }

    [ContextMenu("CollectItemsPoint")]
    private void CollectItemsPoint() {
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        ItemsPoint = 0;
        for (int i = 0; i < items.Length; i++) {
            ItemsPoint += items[i].basePoint;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube((topRight + downLeft) / 2, new Vector3(Mathf.Abs(topRight.x - downLeft.x), Mathf.Abs(topRight.y - downLeft.y), 0));
    }
    private void OnApplicationQuit() {
        SaveSystem.Save(0);
    }
}
