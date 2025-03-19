using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] AudioSource[] soundEffectSource;
    [SerializeField] AudioSource backgroundMusicSource;

    [SerializeField] Slider soundEffectSlider;
    [SerializeField] Slider backgroundMusicSlider;

    [SerializeField] GameObject settingPanel;
    public static UIManager Instance;
    private void Awake() {
        if (Instance == null) Instance = this;
        else Debug.LogError("There is more then one UIManager");

        if(SceneManager.GetActiveScene().buildIndex != 4) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Data data = SaveSystem.Load();
        if(data != null) {
            soundEffectSlider.value = data.SoundEffectVolum;
            backgroundMusicSlider.value = data.MusicVolum;
        }

        OnSoundEffectVolumeChange();
        OnBackgroundMusicVolumeChange();
    }

    private void Update() {
        if (SceneManager.GetActiveScene().buildIndex == 4) return;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            switch (settingPanel.activeSelf) {
                case true:
                    CloseWindow();
                    break;
                case false:
                    settingPanel.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }
        }
    }

    public void CloseWindow() {
        settingPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OpenWindow() {
        settingPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void OnSoundEffectVolumeChange() {
        for (int i = 0; i < soundEffectSource.Length; i++) {
            soundEffectSource[i].volume = soundEffectSlider.value;
        }
    }
    public void OnBackgroundMusicVolumeChange() {
        backgroundMusicSource.volume = backgroundMusicSlider.value;
    }
    public float GetMusicVolum() {
        return backgroundMusicSlider.value;
    }
    public float GetSoundEffectVolum() {
        return soundEffectSlider.value;
    }
    public void Exit() {
        Application.Quit();
    }
}
