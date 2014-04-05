using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {
	public string team;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 90*Time.deltaTime, 0);
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer == 10) {
			// this is a player
			other.gameObject.GetComponent<AIScript>().touchFlag(gameObject);
		}
	}
}
