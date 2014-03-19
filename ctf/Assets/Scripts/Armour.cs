using UnityEngine;
using System.Collections;

public class Armour:Spawn {
	public Armour(Vector3 p):base(p) {
		spawnItem(); spawnInterval = 30; // ?
	}
	private Item getItem() {
		Item item = new Item();
		item.itemName = "Armour";
		item.armour = 10;
		return item;
	}
	
	public override void itemPicked(AIScript player) {
		base.itemPicked(player);
		player.pickupItem(getItem());
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("Armour"));
		base.spawnItem();
	}
}
