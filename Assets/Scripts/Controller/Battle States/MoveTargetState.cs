using UnityEngine;
using System.Collections;

public class MoveTargetState: BattleState {

	private Vec rotateDirSnap(Vec v, float r) {
		// rotate vector v clockwise by r*pi radians
		// rotates movement cw by angle r*pi radians
		var rot = Quaternion.AngleAxis(r*180f, Vector3.forward);
		Vec rotated = new Vec(rot * v.vec);
		return rotated;
	}	

	protected override void OnMove(object sender, InfoEventArgs<Vec> e) {
		if (e.info != new Vec(0, 0, 0)) {
			//Debug.Log("pre:" + e.info);
			Vec vec = rotateDirSnap(e.info, -0.25f);
			//Debug.Log(vec);
			MoveCursor(vec + pos);
		}
	}
}