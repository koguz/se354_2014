using UnityEngine;
using System.Collections;

public class Doomsday:Spawn {
	public Doomsday(Vector3 p):base(p) {
		spawnItem();
		spawnInterval = 120;
	}
	private Weapon getWeapon() {
		Weapon weapon = new Weapon();
		weapon.name = "DeathBringer";
		weapon.ammoCount = 1;
		weapon.ammoPerSec = 5;
		weapon.damPerAmmo = 80;
		return weapon;
	}
	
	public override void itemPicked(AIScript player) {
		base.itemPicked(player);
		player.pickupItem(getWeapon());
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("Doomsday"));
		base.spawnItem();
	}
}
