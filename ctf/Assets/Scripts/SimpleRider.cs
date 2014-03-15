using UnityEngine;
using System.Collections;

public class SimpleRider : MonoBehaviour {
	Level level;
	int msize;
	public Vector3 target;
	
	enum State { SEARCH, MOVE };
	State state;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<AIScript>().playername = "Simple Rider";
		level = GameObject.Find("Level").GetComponent<Level>();
		state = State.SEARCH;
		msize = (int)Mathf.Sqrt(level.getMap().Length);
		Debug.Log (msize);
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.GetComponent<AIScript>().wasItDead())
			Debug.Log ("I was dead, but now I live!"); // this only works once
		switch(state) {
		case State.SEARCH:
			bool searching = true;
			do {
				int px = Random.Range(0, msize);
				int pz = Random.Range(0, msize);
				Debug.Log (px + ", " + pz);
				int m  = level.getMap()[px, pz];
				if (m != 1 && m != 5) {
					target = new Vector3(px, 0, pz);
					searching = false;
				}
			} while(searching);
			state = State.MOVE;
			break;
		case State.MOVE:
			Vector3 dir = target - transform.position;
			if(dir.magnitude < 0.1f) {
				state = State.SEARCH;
				break;
			}
			dir.Normalize();
			transform.position += dir * Time.deltaTime;
			transform.LookAt(target);
			break;
		}
	}
}
