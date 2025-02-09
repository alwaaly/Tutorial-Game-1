using UnityEngine;
using UnityEngine.Splines;

public class Airplane : Item {
    [SerializeField] SplineAnimate animate;
    public override void OnCatch() {
        base.OnCatch();
        animate.Pause();
    }
}
