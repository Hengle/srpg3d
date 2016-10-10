using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BattleState: State {
	protected BattleController owner;
	//protected Driver driver;
	public CameraRig cameraRig { get { return owner.cameraRig; } }
	public Board board { get { return owner.board; } }
	public LevelData levelData { get { return owner.levelData; } }
	public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; } }
	public Transform cursor { get { return owner.cursor; } }
	public Vec pos { get { return owner.pos; } set { owner.pos = value; } }
	public Vec selectedTilepos { get { return owner.selectedTilepos; } set { owner.selectedTilepos = value; } }
	protected static readonly Vec posInf = new Vec(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
	/*public Tile currentTile { get { return owner.currentTile; } }
	    public AbilityMenuPanelController abilityMenuPanelController { get { return owner.abilityMenuPanelController; } }
	    public StatPanelController statPanelController { get { return owner.statPanelController; } }
	    public HitSuccessIndicator hitSuccessIndicator { get { return owner.hitSuccessIndicator; } }
	    public Turn turn { get { return owner.turn; } }
	    public List<Unit> units { get { return owner.units; } }
	    */
	protected virtual void Awake() {
		owner = GetComponent<BattleController>();
	}

	protected override void AddListeners() {
		//	if (driver == null || driver.Current == Drivers.Human) {
		InputController.moveEvent += OnMove;
		InputController.fireEvent += OnFire;
		//	}
	}

	protected override void RemoveListeners() {
		InputController.moveEvent -= OnMove;
		InputController.fireEvent -= OnFire;
	}

    /*public override void Enter() {
		driver = (turn.actor != null) ? turn.actor.GetComponent<Driver>() : null;
		base.Enter();
	}*/

    protected Vec rotateDirSnap(Vec v, float r) {
        // rotate vector v clockwise by r*pi radians
        // rotates movement cw by angle r*pi radians
        var rot = Quaternion.AngleAxis(r*180f, Vector3.forward);
        Vec rotated = new Vec(rot * v.vec);
        return rotated;
    }

    protected virtual void OnMove(object sender, InfoEventArgs<Vec> e) {

	}

	protected virtual void OnFire(object sender, InfoEventArgs<int> e) {

	}

	private bool oob(Vec p) {
		return false;
	}

	protected virtual void MoveCursor(Vec p) {
		Vec rounded = Vec.roundToSnap(p);
		Debug.Log("calling move cursor to location: " + p.ToString());
		if (pos == p || oob(p))
			return;
		pos = p;
		SelectTile(rounded);

		float height = board.tiles[selectedTilepos].center.y;

		cursor.localPosition = new Vector3(p.x, height, p.y);
		//Debug.Log(cursor.localPosition.ToString());
	}

	protected virtual void SelectTile(Vec p) {
		if (selectedTilepos == p)
			return;
		if (!board.tiles.ContainsKey(p)) {
			Vec closestPos = posInf;
			float cD = float.PositiveInfinity;
			foreach (Tile t in board.tiles.Values) {
				if ((t.pos-p).vec.magnitude < cD) {
					closestPos = t.pos;
					cD = (t.pos-p).vec.magnitude;
				}
			}
			if (closestPos == posInf) return;
			p = closestPos;
		}
		selectedTilepos = p;

		tileSelectionIndicator.localPosition = board.tiles[p].center;
	}

	/*protected virtual Unit GetUnit(Point p) {
		Tile t = board.GetTile(p);
		GameObject content = t != null ? t.content : null;
		return content != null ? content.GetComponent<Unit>() : null;
	}

	protected virtual void RefreshPrimaryStatPanel(Point p) {
		Unit target = GetUnit(p);
		if (target != null)
			statPanelController.ShowPrimary(target.gameObject);
		else
			statPanelController.HidePrimary();
	}

	protected virtual void RefreshSecondaryStatPanel(Point p) {
		Unit target = GetUnit(p);
		if (target != null)
			statPanelController.ShowSecondary(target.gameObject);
		else
			statPanelController.HideSecondary();
	}

	protected virtual bool DidPlayerWin() {
		return owner.GetComponent<BaseVictoryCondition>().Victor == Alliances.Hero;
	}

	protected virtual bool IsBattleOver() {
		return owner.GetComponent<BaseVictoryCondition>().Victor != Alliances.None;
	}*/
}