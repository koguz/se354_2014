using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour {
	private int health = 100;
	private int armour = 50;
	private int puan = 0;
	private float disabledTime;
	private float flagTime;
	private float hexTime;
	private int damageMult;
	public bool hasFlag = false;
	private Level level;
	private bool died = false;
	
	public string playername;
	public string team;
	
	private List<Weapon> weapons;
	private int currentWeapon;
	
	// Use this for initialization
	void Start () {
		weapons = new List<Weapon>();
		weapons.Add(new Weapon()); // default is machine gun
		currentWeapon = 0;
		damageMult = 1;
		level = GameObject.Find ("Level").GetComponent<Level>();
	}
	
	public int getHealth() { return health; }
	public int getArmour() { return armour; }
	public int getPuan()   { return puan;   }
	public float getDisTime(){ return disabledTime; }
	
	public void ClearValues() {
		weapons.Clear();
		weapons.Add(new Weapon());
		currentWeapon = 0;
		damageMult = 1;
		health = 100;
		armour = 50;
		died = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isDead()) {
			disabledTime = Time.time;
			gameObject.SetActive(false);
		}
		if(damageMult > 1 && (Time.time - hexTime > 10)) {
			damageMult = 1; 
		}
		if(hasFlag && (Time.time - flagTime > 1)) {
			increasePoints(1);
			flagTime = Time.time;
		}
		if(hasFlag) {
			if (
				(team == "Red" && !level.getFlagLost("Red") && (transform.position - level.redFlagPoint).magnitude < 1) ||
				(team == "Blue" && !level.getFlagLost("Blue") && (transform.position - level.blueFlagPoint).magnitude < 1)
			) {
				// drop flag, add points and send the flag to original position
				hasFlag = false;
				Transform flag = transform.Find("Flag(Clone)");
				flag.parent = null;
				flag.position = flag.GetComponent<Flag>().originalPosition;
				increasePoints(500); // Score!
			}
		}
	}
	
	public void pickupItem(Item item) {
		health += item.health;
		armour += item.armour;
		if (health > 100) health = 100;
		if (armour > 100) armour = 100;
		if(item.damage > 1) {
			damageMult = item.damage;
			hexTime = Time.time;
		}
	}
	
	public void pickupItem(Weapon weapon) {
		bool iDontHaveThisWeapon = true;
		foreach (Weapon w in weapons) {
			if (w.name.Equals(weapon.name)) {
				iDontHaveThisWeapon = false;
				w.ammoCount = weapon.ammoCount;
			}
		}
		if(iDontHaveThisWeapon) {
			weapons.Add(weapon);
		}
	}

	public void touchFlag(GameObject f) {
		if(f.GetComponent<Flag>().team == team) {
			// return flag to original position
			f.transform.position = f.GetComponent<Flag>().originalPosition;
			increasePoints(Mathf.FloorToInt((f.GetComponent<Flag>().originalPosition - transform.position).magnitude)); 
			level.flagUpdate(team, false);
		} else {
			// pick up the flag
			hasFlag = true;
			f.transform.parent = transform;
			f.transform.position = transform.position;
			increasePoints(50);
			flagTime = Time.time;
			level.flagUpdate(f.GetComponent<Flag>().team, true);
		}
	}
	
	public List<Weapon> getWeapons() { 
		return weapons; 
	}
	
	public void setCurrentWeapon(int index) {
		if(index >= weapons.Count) {
			Debug.LogError("Weapon can not be set - index out of range");
			return; 
		}
		currentWeapon = index;
	}
	
	public int takeAHit(int damage) {
		// full armour saves all hit damage, but gets the damage itself...
		int damageCaused = damage - (damage * armour/100);
		health -= damageCaused; 
		armour -= damage;
		if(armour < 0) armour = 0;
		int bonus = 0;
		if(health <= 0) {
			kill ();
			bonus = 50;
		} 
		return damageCaused + bonus;
	}
	
	private void kill() {
		if(hasFlag) {
			transform.Find("Flag(Clone)").parent = null; // CLONE!! bir saatimi yedi.
			hasFlag = false;
		}
		// disabledTime = Time.time;
		// gameObject.SetActive(false);
		died = true;
		Debug.Log ("killed");
	}

	public bool isDead() {
		return died;
	}

	public void hitObstacle() {
		kill ();
		//puan -= puan/2;
		increasePoints(-50);
	}
	
	public void increasePoints(int p) {
		puan += p;
	}
	
	public void Fire() {
		if 
			( weapons[currentWeapon].ammoCount == 0 ||
			  (Time.time - weapons[currentWeapon].lastFired) < weapons[currentWeapon].ammoPerSec
			) {
			Debug.LogWarning("Cannot fire... yet");
			return;
		}
		Vector3 direction = gameObject.transform.forward;
		GameObject mermi = (GameObject) GameObject.Instantiate(Resources.Load ("Bullet"));
		mermi.transform.position = gameObject.transform.position + (gameObject.transform.forward*0.7f);
		mermi.GetComponent<Bullet>().damage = weapons[currentWeapon].damPerAmmo * damageMult;
		mermi.GetComponent<Bullet>().parent = this;
		mermi.GetComponent<Bullet>().direction = direction;
		weapons[currentWeapon].lastFired = Time.time;
		if(!weapons[currentWeapon].name.Equals("Machine Gun")) {
			weapons[currentWeapon].ammoCount--;
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.collider.gameObject.layer == 10) {
			kill ();
			increasePoints(-50);
		}
	}
	
}
