using UnityEngine;
using System.Collections;

public class SimpleTestController : MonoBehaviour {
	public int speed = 5;
	public int angular = 30;
	// Use this for initialization
	void Start () {
		GetComponent<AIScript>().playername = "CONTROL";
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.UpArrow)) {
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.LeftArrow)) {
			transform.Rotate(0, -1 * angular * Time.deltaTime, 0);
		}
		if(Input.GetKey(KeyCode.RightArrow)) {
			transform.Rotate(0, angular * Time.deltaTime, 0);
		}
		if(Input.GetKey(KeyCode.Space)) {
			GetComponent<AIScript>().Fire();
		}
	}
}
