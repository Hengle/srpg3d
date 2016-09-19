using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Vec {
	public Vector3 vec;
	public float x { get { return vec.x; } }
	public float y { get { return vec.y; } }
	public float z { get { return vec.z; } }
	public float angle;

	public Vec(float _x, float _y, float _z = 0f) {
		vec = new Vector3((float)Round(_x), (float)Round(_y), (float)Round(_z));
		angle = Mathf.Atan2(vec.x, vec.y);
		if (angle < 0) angle += 2*Mathf.PI;
	}

	public Vec(Vector3 v) {
		vec = v;
		angle = Mathf.Atan2(vec.x, vec.y);
		if (angle < 0) angle += 2*Mathf.PI;
	}

    public Vector3 ToEuler() {
        return new Vector3(0, angle/Mathf.PI*180, 0);
    }

	private static float Round(float f, int significance = 100) {
		if (significance == 1) return Round(Mathf.Round(f));
		return Mathf.Round(f*significance)/significance;
	}

	public static Vec roundToSnap(Vec v) {
		return new Vec(Round(v.x, 1), Round(v.y, 1), Round(v.z, 1));
	}

	public static Vec roundToSnap(Vec v, float ManDst) {
		// modify magnitude so manhatten distance = ManDst
		float scaler = ManDst/v.ManhattanDist();
		return Vec.roundToSnap(v*scaler);
	}

	public float ManhattanDist() {
		return Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z);
	}

	public static Vec operator +(Vec a, Vec b) {
		return new Vec(a.x + b.x, a.y + b.y, a.z+b.z);
	}

	public static Vec operator -(Vec p1, Vec p2) {
		return p1 + (p2*-1f);
	}

	public static Vec operator *(Vec v, float f) {
		return new Vec(v.x*f, v.y*f, v.z*f);
	}

	public static Vec operator *(float f, Vec v) {
		return v*f;
	}

	public static Vec operator /(Vec v, float f) {
		return v*(1f/f);
	}

	public static Vec operator /(float f, Vec v) {
		return v/f;
	}

	public static bool operator ==(Vec a, Vec b) {
		return (Round(a.x) == Round(b.x)) && (Round(a.y) == Round(b.y));

	}

	public static bool operator !=(Vec a, Vec b) {
		return !(a == b);
	}

	public override bool Equals(object obj) {
		return (obj is Vec) ? this == (Vec)obj : false;
	}

	public override int GetHashCode() {
		unchecked {
			int hash = 17;
			hash = hash*23 + Round(x).GetHashCode();
			hash = hash*23 + Round(y).GetHashCode();
			return hash;
		}
	}

	public override string ToString() {
		return "{" + vec.ToString() + ", angle: " + angle.ToString() + "}";
	}

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}
