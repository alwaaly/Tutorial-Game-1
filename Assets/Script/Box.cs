using UnityEngine;

public class Box : MonoBehaviour {
    [SerializeField] Transform boxLid;
    [SerializeField] float closeDuration = 1;
    [SerializeField] float openDuration = 2;
    [SerializeField] BoxCollider coll;
    [SerializeField] Item[] items;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip closeClip;
    [SerializeField] AudioClip openClip;

    float progress;
    float updateDuration;
    enum BoxState { Close , Open }
    BoxState state;
    Item selectedItem;
    private void Awake() {
        state = BoxState.Open;
        updateDuration = openDuration;
        coll.enabled = false;
        CustomUpdate();
        selectedItem =  items[Random.Range(0, items.Length)];
        selectedItem.basePoint *= 2;
        selectedItem.gameObject.SetActive(true);
        selectedItem.OnItemCollect += SelectedItem_OnItemCollect;
        selectedItem.OnItemDestroy += SelectedItem_OnItemDestroy;
        isActive = true;
    }

    private void SelectedItem_OnItemDestroy() {
        selectedItem.OnItemDestroy -= SelectedItem_OnItemDestroy;
        gameObject.SetActive(false);
        isActive = false;
    }

    private void SelectedItem_OnItemCollect() {
        selectedItem.OnItemCollect -= SelectedItem_OnItemCollect;
        gameObject.SetActive(false);
        isActive = false;
    }
    bool isActive;
    private async void CustomUpdate() {
        while (isActive) {
            await Awaitable.WaitForSecondsAsync(updateDuration);
            if (state == BoxState.Open) {
                Close();
            }
            else {
                Open();
            }
        }
    }
    private async void Close() {
        state = BoxState.Close;
        updateDuration = closeDuration;
        if (coll == null) return;
        coll.enabled = true;
        progress = 0;
        if (!source.isPlaying) {
            source.clip = closeClip;
            source.Play();
        }
        while (progress <= 1) {
            progress += Time.deltaTime * 2;
            boxLid.localRotation = Quaternion.Slerp(Quaternion.Euler(-120, 60, -90), Quaternion.Euler(-90, 60, -90), progress);
            await Awaitable.NextFrameAsync();
        }
    }
    private async void Open() {
        state = BoxState.Open;
        updateDuration = openDuration;
        if (coll == null) return;
        coll.enabled = false;
        progress = 0;
        if (!source.isPlaying) {
            source.clip = openClip;
            source.Play();
        }
        while (progress <= 1) {
            progress += Time.deltaTime * 2;
            boxLid.localRotation = Quaternion.Slerp(Quaternion.Euler(-90, 60, -90), Quaternion.Euler(-120, 60, -90), progress);
            await Awaitable.NextFrameAsync();
        }
    }
    private void OnDestroy() {
        isActive = false;
    }
}
