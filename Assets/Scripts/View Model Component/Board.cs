using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileComparer : IEqualityComparer<Vec> {
    // ignore z axis
    public bool Equals(Vec x, Vec y) {
        return x.x == y.x && x.y == y.y;
    }

    public int GetHashCode(Vec obj) {
        return new Vec(obj.x, obj.y).GetHashCode();
    }
}

public class Board: MonoBehaviour {
	// actual game board with dictionary of tiles
	[SerializeField]
	GameObject tilePrefab;
	public Dictionary<Vec, Tile> tiles = new Dictionary<Vec, Tile>(new TileComparer());
	public Vec min { get { return _min; } }
	public Vec max { get { return _max; } }
	Vec _min;
	Vec _max;
	Vec[] dirs = new Vec[4] { new Vec(0f, 1f), new Vec(0f, -1f), new Vec(1f, 0f), new Vec(-1f, 0f) };
	Color highlightTileColor = new Color(0, 1, 1, 1);
	Color defaultTileColor = new Color(1, 1, 1, 1);


	public void Load(LevelData data) {
		for (int i = 0; i < data.tiles.Count; ++i) {
			GameObject instance = Instantiate(tilePrefab) as GameObject;
			Tile t = instance.GetComponent<Tile>();
			t.Load(data.tiles[i]);
			tiles.Add(t.pos, t);
             if (CONSTS.VERBOSE > 0) Debug.Log(String.Format("Tile loaded at: ({0}, {1}, {2})", data.tiles[i].x, data.tiles[i].y, data.tiles[i].z));
		}
	}

	public Tile GetTile(Vec p) {
		return tiles.ContainsKey(p) ? tiles[p] : null;
	}


	public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile) {
		List<Tile> retValue = new List<Tile>();
		retValue.Add(start);

		ClearSearch();
		Queue<Tile> queue = new Queue<Tile>();

		start.distance = 0;
		queue.Enqueue(start);

		while (queue.Count > 0) {
			Tile t = queue.Dequeue();
			for (int i = 0; i<4; i++) {
				Tile next = GetTile(t.pos + dirs[i]);
				if (next == null || next.distance < t.distance + 1f - Mathf.Epsilon)
					continue;

				if (addTile(t, next)) {
					next.distance = t.distance + 1f;
					next.prev = t;
					queue.Enqueue(next);
					retValue.Add(next);
				}
			}
		}

		return retValue;
	}

	public void SelectTiles(List<Tile> tiles) {
        // change this to elevate, in addition rest of tiles should be dimmed
		for (int i = tiles.Count - 1; i >= 0; --i)
			tiles[i].GetComponent<Renderer>().material.SetColor("_Color", highlightTileColor);
	}

	public void DeSelectTiles(List<Tile> tiles) {
		for (int i = tiles.Count - 1; i >= 0; --i)
			tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
	}

	void ClearSearch() {
		foreach (Tile t in tiles.Values) {
			t.prev = null;
			t.distance = float.PositiveInfinity;
		}
	}
}