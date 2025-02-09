using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Item : MonoBehaviour {
    public int basePoint = 10;
    [Range(.01f,3f)]
    public float BaseWeight;
    public bool CanRotate = true;
    [SerializeField] BoxCollider box;
    // x => min y=> Max
    public Vector2 minMaxScale;
    public float OverlapingSpace;
    public event Action OnItemDestroy;
    public event Action OnItemCollect;
    private void Awake() {
        basePoint = (int) Mathf.Ceil(basePoint * transform.localScale.x);
        BaseWeight = (int) Mathf.Ceil(BaseWeight * transform.localScale.x);
    }

    private void OnValidate() {
        if (box == null) box = GetComponent<BoxCollider>();
    }
    virtual public void OnCatch() {
        //print("Catch");
    }
    public void OnCollect() {
        OnItemCollect?.Invoke();
        gameObject.SetActive(false);
        transform.position = new Vector3(100, 0, 0);
        transform.parent = null;
        // add more point to the player
        GameManager.Instance.AddPoint(basePoint);
    }
    public bool IsOverlapWithOtherObject() {
        box.enabled = false;
        if (Physics.OverlapBox(transform.TransformPoint(box.center), (box.size * transform.lossyScale.x) / 2, transform.rotation).Length == 0) {
            box.enabled = true;
            return false;
        }
        else {
            box.enabled = true;
            return true;
        }
    }
    private void OnDestroy() {
        OnItemDestroy?.Invoke();
    }
}
