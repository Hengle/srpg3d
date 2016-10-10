using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Movement: MonoBehaviour {
	public int range;
	public int jumpHeight;
	protected Unit unit;
	protected Transform jumper;

	public abstract IEnumerator Traverse(Tile tile); // handles path tranversal animation

	protected virtual void Awake() {
		unit = GetComponent<Unit>();
		jumper = transform.FindChild("Jumper");
	}

	public virtual List<Tile> GetTilesInRange(Board board) {
		List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
		Filter(retValue);
		return retValue;
	}

	protected virtual bool ExpandSearch(Tile from, Tile to) { // filters viable intermediate + end point nodes
		return (from.distance + 1) <= range;
	}

	protected virtual void Filter(List<Tile> tiles) { // filters out end point nodes i.e. where space is occupied but can be passed through
		for (int i = tiles.Count - 1; i >= 0; --i)
			if (tiles[i].content != null)
				tiles.RemoveAt(i);
	}

	protected virtual IEnumerator Turn(Vec dir) {
		TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);

		// When rotating between North and West, we must make an exception so it looks like the unit
		// rotates the most efficient way (since 0 and 360 are treated the same)
		if (t.endValue.y-t.startValue.y >= 180f) { // rotate cw more than 180 degrees
			t.startValue = new Vector3(t.startValue.x, t.startValue.y+360f, t.startValue.z);
		}
		if (t.startValue.y-t.endValue.y >= 180f) { // rotate ccw more than 180 degrees
			t.endValue = new Vector3(t.endValue.x, t.endValue.y+360f, t.endValue.z);
		}
		/*if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
			t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
		else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0))
			t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);*/

		unit.dir = dir;

		while (t != null)
			yield return null;
	}

	/*#region Properties
	public int range { get { return stats[StatTypes.MOV]; } }
	public int jumpHeight { get { return stats[StatTypes.JMP]; } }
	protected Unit unit;
	protected Transform jumper;
	protected Stats stats;
	#endregion

	#region MonoBehaviour

	protected virtual void Start() {
		stats = GetComponent<Stats>();
	}
	#endregion
	
	#endregion

	#region Protected


	#endregion*/
}