using UnityEngine;
using System.Collections;

public class Spawn {
	public Spawn(Vector3 p) {
		position = p;
		lastSpawn = -1;
		spawnInterval = 0;
	}
	public virtual void itemPicked(AIScript player) { 
		GameObject.Destroy(itemMesh);
		itemMesh = null;
		lastSpawn = Time.time;
		
	}
	public virtual void spawnItem() {
		itemMesh.transform.position = position;
		itemMesh.GetComponent<Spawner>().setParent(this);
		lastSpawn = Time.time;
	}
	public Vector3 position;
	public GameObject itemMesh;
	public float lastSpawn;
	public float spawnInterval;
}