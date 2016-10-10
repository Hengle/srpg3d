using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTargetState: BattleState {

    List<Tile> tiles;

    public override void Enter() {
        base.Enter();
        Movement mover = owner.currentUnit.GetComponent<Movement>();
        tiles = mover.GetTilesInRange(board);
        board.SelectTiles(tiles);
    }

    public override void Exit() {
        base.Exit();
        board.DeSelectTiles(tiles);
        tiles = null;
    }

	protected override void OnMove(object sender, InfoEventArgs<Vec> e) {
		if (e.info != new Vec(0, 0, 0)) {
			Debug.Log("pre:" + e.info);
			Vec vec = rotateDirSnap(e.info, -0.25f);
			//Debug.Log(vec);
			MoveCursor(vec + pos);
		}
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e) {
        if (tiles.Contains(owner.currentTile))
            owner.ChangeState<MoveSequenceState>();
    }
}