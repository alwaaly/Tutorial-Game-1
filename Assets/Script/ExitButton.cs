using UnityEngine;

public class ExitButton : UI_WorldButton {
    public override void InvokeAction() { 
        UIManager.Instance.Exit();
    }
}
