using UnityEngine;
using System.Collections;

public class Grenade:Spawn {
	public Grenade(Vector3 p):base(p) {
		spawnItem();
		spawnInterval = 10;
	}
	private Weapon getWeapon() {
		Weapon weapon = new Weapon();
		weapon.name = "Grenade";
		weapon.ammoCount = 3;
		weapon.ammoPerSec = 2;
		weapon.damPerAmmo = 15;
		return weapon;
	}
	
	public override void itemPicked(AIScript player) {
		base.itemPicked(player);
		player.pickupItem(getWeapon());
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("Grenade"));
		base.spawnItem();
	}
}