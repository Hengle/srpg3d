using UnityEngine;
using System.Collections;

public class SelectUnitState : BattleState {

    protected override void OnMove(object sender, InfoEventArgs<Vec> e) {
        if (e.info != new Vec(0, 0, 0)) {
            Debug.Log("pre:" + e.info);
            Vec vec = rotateDirSnap(e.info, -0.25f);
            //Debug.Log(vec);
            MoveCursor(vec + pos);
        }
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e) {
        GameObject content = owner.currentTile.content;
        if (content != null) {
            owner.currentUnit = content.GetComponent<Unit>();
            owner.ChangeState<MoveTargetState>();
        }
    }
}