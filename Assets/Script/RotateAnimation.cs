using UnityEngine;

public class RotateAnimation : MonoBehaviour {
    enum RotatingAxis { X, Y ,Z }
    [SerializeField] RotatingAxis axis;
    void Update() {
        switch (axis) {
            case RotatingAxis.X:
                transform.Rotate(Vector3.right, 90 * Time.deltaTime);
                break;
            case RotatingAxis.Y:
                transform.Rotate(Vector3.up, 90 * Time.deltaTime);
                break;
            case RotatingAxis.Z:
                transform.Rotate(Vector3.forward, 90 * Time.deltaTime);
                break;
        }
    }
}
