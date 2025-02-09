using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_GetBigAnimation : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler {
    [SerializeField] float toSize = 1.2f;
    [SerializeField] RectTransform rect;
    [SerializeField] float animationSpeed = 4;
    [SerializeField] AnimationCurve curve;
    Vector2 defaultSize;
    private void OnValidate() {
        if (rect == null) rect = transform.GetComponent<RectTransform>();
    }
    private void Awake() {
        defaultSize = rect.sizeDelta;
        tokenSource = new();
    }
    public void OnPointerEnter(PointerEventData eventData) {
        tokenSource.Cancel();
        tokenSource = new();
        GetBiggerAnimation(tokenSource.Token);
    }

    public void OnPointerExit(PointerEventData eventData) {
        tokenSource.Cancel();
        tokenSource = new();
        ToDefaultAnimation(tokenSource.Token);
    }
    float progress;
    CancellationTokenSource tokenSource;
    async void GetBiggerAnimation(CancellationToken token) {
        try {
            while (progress <= 1) {
                progress += Time.deltaTime * animationSpeed;
                rect.sizeDelta = Vector2.Lerp(defaultSize, defaultSize * toSize, curve.Evaluate(progress));
                await Awaitable.NextFrameAsync(token);
            }
        }
        catch (OperationCanceledException) {
        }
    }
    async void ToDefaultAnimation(CancellationToken token) {
        try {
            while (progress >= 0) {
                progress -= Time.deltaTime * animationSpeed;
                rect.sizeDelta = Vector2.Lerp(defaultSize, defaultSize * toSize, curve.Evaluate(progress));
                await Awaitable.NextFrameAsync(token);
            }
        }
        catch (OperationCanceledException) {
        }
    }

}
