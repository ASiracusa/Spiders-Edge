using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteSetter : MonoBehaviour {

	public GameObject world;
	public Dictionary<string, Color> palette;

	private int currentLevelID;
	private GameObject currentLevel;
	private readonly string[] LEVEL_NAMES = {"Tutorial", "!Tutorial"};
	private readonly string[] LEVEL_PALETTES = { "night", "night" };

	// Use this for initialization
	void Start () {
		palette = new Dictionary<string, Color> ();
		LoadPalette ("night");
		ApplyPalette ();
	}

	private void ApplyPalette () {
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
			palette.Add ("floor", new Color (86, 86, 86));
			palette.Add ("wall", new Color (141, 141, 141));
			palette.Add ("witness", new Color (174, 00, 16));
			palette.Add ("grappleable", new Color (255, 00, 23));
		}
	}

	public IEnumerator ChangeWorld () {

		// load the black screen
		float transTime = 0f;
		while (transTime < 1.0f) {
			transTime += Time.deltaTime;
			yield return null;
		}

		// get the next level
		currentLevelID += 1;
		GameObject levelModel = Resources.Load<GameObject> ("Levels/" + LEVEL_NAMES [currentLevelID]);

		// creates the new level
		GameObject newLevel = Instantiate (levelModel, new Vector3(0, 0, 0), Quaternion.identity);
		newLevel.transform.parent = world.transform;

		// destroys the old level
		GameObject.Destroy (currentLevel);
		currentLevel = newLevel;

		// colors the new level
		LoadPalette (LEVEL_PALETTES[currentLevelID]);
		ApplyPalette ();

		// unload the black screen
		transTime = 0f;
		while (transTime < 1.0f) {
			transTime += Time.deltaTime;
			yield return null;
		}

	}

}
