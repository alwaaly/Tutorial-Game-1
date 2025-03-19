using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : UI_WorldButton {
    public override void InvokeAction() {
        SaveSystem.Save(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
