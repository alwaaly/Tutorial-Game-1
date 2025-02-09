using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneText : MonoBehaviour {
    public TextMeshProUGUI text;
    public void RePlay() {
        SaveSystem.Save(0);
        SceneManager.LoadScene(0);
    }
    public static EndSceneText Instance;
    private void Awake() {
        Instance = this;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
