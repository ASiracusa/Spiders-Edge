﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteSetter : MonoBehaviour {

	public GameObject world;
	public Dictionary<string, Color> palette;

	// Use this for initialization
	void Start () {
		palette = new Dictionary<string, Color> ();
		palette.Add ("floor", new Color (255, 0, 0));
		palette.Add ("wall", new Color (0, 255, 0));
		ChangeTheWorld ();
	}

	private void ChangeTheWorld () {
		// my final message
		int childCount = world.transform.childCount;
		for (int i = 0; i < childCount; i++) {
			GameObject child = world.transform.GetChild (i).gameObject;
			if (palette.ContainsKey (child.name)) {
				child.GetComponent<Renderer> ().material.color = palette [child.name];
			}
		}
		// goodb ye
	}

	public void LoadPalette (string key) {
		if (key == "night") {

		}
	}

}