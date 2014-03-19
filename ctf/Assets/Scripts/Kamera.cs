using UnityEngine;
using System.Collections;

public class Kamera : MonoBehaviour {
	public Transform player;
	private Vector3 relativeVector;
	public float rSmoothing = 8.0f;
	public float distanceConstant = 1.5f;
	
	
	// Use this for initialization
	void Start () {
		relativeVector = new Vector3(0, -1, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) return;
		float wrangle = player.transform.eulerAngles.y;
		float crangle = transform.eulerAngles.y;
		
		crangle = Mathf.LerpAngle(crangle, wrangle, rSmoothing * distanceConstant);
		Quaternion cRotate = Quaternion.Euler(0, crangle, 0);
		transform.position = player.transform.position;
		transform.position -= cRotate * relativeVector * distanceConstant;
		transform.LookAt(player);
	}
}
