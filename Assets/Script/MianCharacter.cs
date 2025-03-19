using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MianCharacter : MonoBehaviour {
    [SerializeField] float movmentSpeed;
    [SerializeField] Transform pivot;
    public Transform hook;
    [SerializeField] float defoaltHookSpeed = 10;
    [SerializeField] Transform mainPlaneBody;
    float hookSpeed;
    // x => left corrner , Y => Right corrner
    [SerializeField] Vector2 leftandRightCorrner;
    [SerializeField] float downCorrner;
    int movmentDirection;
    int hookRotationDirection;
    float currentAngle;
    [SerializeField] GameObject closeHook;
    [SerializeField] GameObject openHook;
    enum HookState { Returned , Throwing , Returing };
    [SerializeField] HookState hookState;
    Item collectedItem;
    UI_WorldButton worldButton;

    [SerializeField] ParticleSystem hookParticle;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip CatchClip;
    [SerializeField] AudioClip pullingRopelip;

    float hookDefaultYPos;
    public event System.Action OnCollect;

    public static MianCharacter Instance;
    private void Awake() {
        Instance = this;

        hookRotationDirection = 1;
        hookState = HookState.Returned;

        hookDefaultYPos = hook.localPosition.y;
        hookSpeed = defoaltHookSpeed;
        closeHook.SetActive(false);
        openHook.SetActive(true);

        GameManager.Instance.OnTimeEnd += Instance_OnTimeEnd;
    }
    bool isTimeEnd;
    private void Instance_OnTimeEnd() {
        isTimeEnd = true;
    }

    private void Update() {
        if (isTimeEnd) return;
        HandelPostion();
        HandelPivotRotation();
        HandelHookState();
        mainPlaneBody.Rotate(Vector3.up, 90 * Time.deltaTime);
    }

    bool HandelHookState_isHookPassCorrner;
    private void HandelHookState() {
        if (Input.GetKey(KeyCode.Space) && hookState == HookState.Returned) {
            hookState = HookState.Throwing;
        }
        if (hookState == HookState.Throwing) {
            hook.position += hook.up * -1 * Time.deltaTime * hookSpeed;
        }
        HandelHookState_isHookPassCorrner = hook.position.x > leftandRightCorrner.y || hook.position.x < leftandRightCorrner.x || hook.position.y < downCorrner;
        if (hookState == HookState.Throwing && HandelHookState_isHookPassCorrner) {
            hookState = HookState.Returing;
        }
        if (hookState == HookState.Returing) {
            hook.position += hook.up * Time.deltaTime * hookSpeed;
        }
        if (hookState == HookState.Returing && hook.localPosition.y > hookDefaultYPos) {
            hook.localPosition = new Vector3(hook.localPosition.x, hookDefaultYPos, hook.localPosition.z);
            hookState = HookState.Returned;
            if (collectedItem != null) {
                hook.GetChild(0).gameObject.SetActive(false);
                collectedItem.OnCollect();
                collectedItem.OnItemDestroy -= CollectedItem_OnItemDestroy;
                collectedItem = null;
                hookSpeed = defoaltHookSpeed;
                source.Pause();
                OpenHook();
            }
            else if(worldButton != null) {
                worldButton.OnCollect();
                worldButton.InvokeAction();
                worldButton = null;
            }
        }
    }

    bool HandelPivotRotation_isHookPassCorrner;
    private void HandelPivotRotation() {
        if (hookState != HookState.Returned) return;
        currentAngle = pivot.rotation.eulerAngles.z;
        if (currentAngle > 180) {
            currentAngle -= 360;
        }
        HandelPivotRotation_isHookPassCorrner = hook.position.x > leftandRightCorrner.y || hook.position.x < leftandRightCorrner.x;
        if (HandelPivotRotation_isHookPassCorrner == false) {
            if (currentAngle > 60) {
                hookRotationDirection = -1;
            }
            else if (currentAngle < -60) {
                hookRotationDirection = 1;
            }
        }
        else {
            if(currentAngle < 0) {
                hookRotationDirection = 1;
            }
            else {
                hookRotationDirection = -1;
            }
        }
        pivot.Rotate(new Vector3(0, 0, 90 * Time.deltaTime * hookRotationDirection));
    }

    private void HandelPostion() {
        if (hookState != HookState.Returned) return;
        movmentDirection = 0;
        if (Input.GetKey(KeyCode.A)) {
            movmentDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D)) {
            movmentDirection = 1;
        }
        transform.position += Vector3.right * Time.deltaTime * movmentSpeed * movmentDirection;
        if (transform.position.x > leftandRightCorrner.y) {
            transform.position = new Vector3(leftandRightCorrner.y, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < leftandRightCorrner.x) {
            transform.position = new Vector3(leftandRightCorrner.x, transform.position.y, transform.position.z);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 5) {
            hookParticle.transform.position = hook.position;
            source.clip = CatchClip;
            source.Play();

            hookState = HookState.Returing;
            worldButton = other.gameObject.GetComponent<UI_WorldButton>();

            worldButton.IsCatch = true;

            return;
        }

        if (!other.TryGetComponent(out Item component)) {
            hookState = HookState.Returing;
            return; 
        }
        if (collectedItem != null) return;
        collectedItem = component;
        collectedItem.OnItemDestroy += CollectedItem_OnItemDestroy;
        hookSpeed /= collectedItem.BaseWeight;
        hookState = HookState.Returing;
        other.transform.parent = hook;
        other.transform.localPosition = Vector3.zero;
        collectedItem.OnCatch();
        CloseHook();
        hookParticle.transform.position = hook.position;
        source.clip = CatchClip;
        source.Play();
        playPullingRopelip();
        hookParticle.Play();
    }
    async void playPullingRopelip() {
        await Awaitable.WaitForSecondsAsync(CatchClip.length);
        if (source == null) return;
        source.clip = pullingRopelip;
        source.Play();
    }

    private void CollectedItem_OnItemDestroy() {
        hookState = HookState.Returned;
        hookSpeed = defoaltHookSpeed;
        OpenHook();
        collectedItem.OnItemDestroy -= CollectedItem_OnItemDestroy;
    }

    private void OpenHook() {
        openHook.SetActive(true);
        closeHook.SetActive(false);
    }
    private void CloseHook() {
        closeHook.SetActive(true);
        openHook.SetActive(false);
    }
}
