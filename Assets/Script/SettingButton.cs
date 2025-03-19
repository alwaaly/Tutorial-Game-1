using UnityEngine;

public class SettingButton : UI_WorldButton {
    public override void InvokeAction() {
        UIManager.Instance.OpenWindow();
    }
}
