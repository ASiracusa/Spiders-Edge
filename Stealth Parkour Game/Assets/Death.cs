using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

	public GameObject player;
	public GameObject world;

	public Vector3 spawnpoint;
	public List<GameObject> eyes;

	private RaycastHit hit;

	private float mapBottom = -20f;

	// Use this for initialization
	void Start () {
		spawnpoint = player.transform.position;

		eyes = new List<GameObject> ();
		FindWitnesses ();

		StartCoroutine (ExitingBounds ());
		StartCoroutine (SeekPlayer ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FindWitnesses () {
		int childCount = world.transform.childCount;
		for (int i = 0; i < childCount; i++) {
			GameObject child = world.transform.GetChild (i).gameObject;
			if (child.name == "witness") {
				eyes.Add (child.transform.GetChild(0).gameObject);
			}
		}
	}

	private void RespawnPlayer () {
		print ("respawning!");
		player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		player.transform.position = spawnpoint;
	}

	private IEnumerator ExitingBounds () {
		while (true) {
			if (player.transform.position.y < mapBottom) {
				RespawnPlayer ();
			}
			yield return null;
		}
	}

	private IEnumerator SeekPlayer	() {
		while (true) {
			foreach (GameObject go in eyes) {
				Vector3 diff = player.transform.GetChild(1).transform.position - go.transform.position;
				Physics.Raycast (go.transform.position, diff, out hit, Mathf.Infinity);
				print ((hit.collider != null) ? hit.collider.gameObject.name : "e");
				print ((Mathf.Abs (diff.y) < Mathf.Abs (diff.x) && Mathf.Abs (diff.z) < Mathf.Abs (diff.x)));
				if ((hit.collider.gameObject.name == "playerTorso" || hit.collider.gameObject.name == "playerHead") && (Mathf.Abs (diff.y) < Mathf.Abs (diff.x) && Mathf.Abs (diff.z) < Mathf.Abs (diff.x))) {
					RespawnPlayer();
				}
			}
			yield return null;
		}
	}

}
