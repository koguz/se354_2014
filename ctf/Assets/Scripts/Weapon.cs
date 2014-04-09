using UnityEngine;
using System.Collections;

public class Weapon { 
	public string name = "Machine Gun";
	public int ammoCount = int.MaxValue;
	public float ammoPerSec= 1; //0.05f; // 1
	public int damPerAmmo= 1; //66; // 1
	public float lastFired = Time.time;
}
