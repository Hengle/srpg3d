﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController: StateMachine {
	public CameraRig cameraRig;
	public Board board;
	public LevelData levelData;
	public Transform tileSelectionIndicator;
    public Transform cursor;
    public Vec pos;
    public Vec selectedTilepos;
    public GameObject heroPrefab;
    public Unit currentUnit;
    public Tile currentTile { get { return board.GetTile(selectedTilepos); } }
    /*
	public AbilityMenuPanelController abilityMenuPanelController;
	public StatPanelController statPanelController;
	public HitSuccessIndicator hitSuccessIndicator;
	public BattleMessageController battleMessageController;
	public FacingIndicator facingIndicator;
	public Turn turn = new Turn();
	public List<Unit> units = new List<Unit>();
	public IEnumerator round;
	public ComputerPlayer cpu;*/

    void Start() {
		ChangeState<InitBattleState>();
	}
}