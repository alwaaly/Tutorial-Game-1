using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ButtonSound : MonoBehaviour, IPointerClickHandler {
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    public void OnPointerClick(PointerEventData eventData) {
        source.clip = clip;
        source.Play();
    }
}
