using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeCollector : MonoBehaviour {
    public static TimeCollector Instance;
    float[] sceneCompletTime;
    private void Awake() {
        Instance = this;
        sceneCompletTime = new float[4];
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
        if(arg0.buildIndex == 4) {
            EndSceneText.Instance.text.text = TimeFormat(SaveSystem.Load().CompleteTime);
        }
    }

    public void SceneCompleted(float time) {
        sceneCompletTime[SceneManager.GetActiveScene().buildIndex] = time;
        float totalTime = 0;
        if (SceneManager.GetActiveScene().buildIndex == 3) {
            for (int i = 0; i < sceneCompletTime.Length; i++) {
                totalTime += sceneCompletTime[i];
            }            
            SaveSystem.Save(totalTime);
        }
    }
    private string TimeFormat(float time) {
        int m = Mathf.FloorToInt(time / 60);
        int s = Mathf.FloorToInt(time - (m * 60));
        if (s >= 10) return m + ":" + s;
        else return m + ":0" + s;
    }
}
