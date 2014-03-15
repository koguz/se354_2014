using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	Spawn parent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setParent(Spawn p) {
		parent = p;
	}
	void OnTriggerEnter(Collider other) {
		parent.itemPicked(other.gameObject.GetComponent<AIScript>());
	}
}
