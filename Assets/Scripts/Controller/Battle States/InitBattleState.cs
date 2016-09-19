using UnityEngine;
using System.Collections;

public class InitBattleState: BattleState {
	public override void Enter() {
		base.Enter();
		StartCoroutine(Init());
	}

	IEnumerator Init() {
		if (true) Debug.Log("levelData: " + levelData);
		board.Load(levelData);
		Vec p = new Vec(levelData.tiles[0].x, levelData.tiles[0].z);
        MoveCursor(p);
		//SelectTile(p);
		yield return null;
		owner.ChangeState<MoveTargetState>();
	}
}