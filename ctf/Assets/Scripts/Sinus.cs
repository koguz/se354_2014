using UnityEngine;
using System.Collections;

public class Sinus : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 150*Time.deltaTime, 0);
		transform.position += 
			(Mathf.Sin( Mathf.PI * 2 * Time.time) - Mathf.Sin( Mathf.PI * 2 * (Time.time-Time.deltaTime))) * transform.up*0.15f;
	}
}
