using UnityEngine;
using System.Collections;

public class HexDamage:Spawn {
	public HexDamage(Vector3 p):base(p) {
		spawnItem(); spawnInterval = 180; // ?
	}
	private Item getItem() {
		Item item = new Item();
		item.itemName = "Hex Damage";
		item.damage = 6;
		return item;
	}
	
	public override void itemPicked(AIScript player) {
		base.itemPicked(player);
		player.pickupItem(getItem ());
	}
	
	public override void spawnItem() {
		itemMesh = (GameObject) GameObject.Instantiate(Resources.Load ("HexDamage"));
		base.spawnItem();
	}
}
