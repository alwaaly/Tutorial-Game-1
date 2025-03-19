using UnityEngine;

public abstract class UI_WorldButton : MonoBehaviour {
    Vector3 defaultPos;
    private void Awake() {
        defaultPos = transform.position;
    }
    public void OnCollect() {
        IsCatch = false;
        transform.position = defaultPos;
    }
    public bool IsCatch;
    private void Update() {
        if (!IsCatch) return;
        transform.position = MianCharacter.Instance.hook.transform.position;
    }
    public abstract void InvokeAction();
}
