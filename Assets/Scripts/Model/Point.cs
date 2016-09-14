using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Point {
	public int x, y;

	public Point(int _x, int _y) {
		x=_x;
		y=_y;
	}

	public static Point operator +(Point a, Point b) {
		return new Point(a.x + b.x, a.y + b.y);
	}

	public static Point operator -(Point p1, Point p2) {
		return new Point(p1.x - p2.x, p1.y - p2.y);
	}

	public static bool operator ==(Point a, Point b) {
		return a.x == b.x && a.y == b.y;
	}

	public static bool operator !=(Point a, Point b) {
		return !(a == b);
	}

	public override bool Equals(object obj) {
		return (obj is Point) ? this == (Point)obj : false;
	}

	public override int GetHashCode() {
		return ((x+13) * 7) + y;
	}
	
	public override string ToString() {
		return string.Format("({0},{1})", x, y);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
