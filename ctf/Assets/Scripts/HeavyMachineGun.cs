using UnityEngine;
using System.Collections;

public class HeavyMachineGun:Spawn {
	public HeavyMachineGun(Vector3 p):base(p) {
		spawnItem();
		spawnInterval = 10;
	}
	
	private Weapon getWeapon() {
		Weapon weapon = new Weapon();
		weapon.name = "Heavy Machine Gun";
		weapon.ammoCount = 30;
		weapon.ammoPerSec = 0.3f;
		weapon.damPerAmmo = 2;
		return weapon;
	}
	
	public override void itemPicked(AIScript player) {
		base.itemPicked(player);
		player.pickupItem(getWeapon());
	}
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("HeavyMachineGun"));
		base.spawnItem();
	}
}
