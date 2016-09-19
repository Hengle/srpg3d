using UnityEngine;
using System.Collections;

public class CameraRig: MonoBehaviour {
	// makes camera follows selector
	public Transform follow;
	Transform _transform;
    public Camera cam;
    float rate = 5f, fRadius = 0.1f;
    Vector2 centreVP = new Vector2(0.5f, 0.5f);

	void Awake() {
		_transform = transform;
	}

    bool isFocused(Vector2 v) {
        return (v-centreVP).magnitude < fRadius;
    }

    float followRatio(Vector2 v, float dT) {
        float dst = ((v-centreVP).magnitude - fRadius)*2f / (1f-fRadius*2f); // gives fraction of target off screen from centre
        return Mathf.Min(dst, 1.0f) * dT * rate; // move at rate * ratio off screen per second
    }

	void Update() {
        if (follow) {
            Vector2 vecVP = cam.WorldToViewportPoint(follow.position);
            if (!isFocused(vecVP)) {
                _transform.position = Vector3.Lerp(_transform.position, follow.position, followRatio(vecVP, Time.deltaTime));
            }
        }
	}
}