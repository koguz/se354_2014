using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Level : MonoBehaviour {
	public string LevelFileName = "Level00.txt";
	public Transform kamera;
	private Kamera kameraScript;
	public List<GameObject> redPlayers;
	public List<GameObject> bluePlayers;
	GameObject[] ps;
	private int sure;
	private int spidx;
	private float kameraSure;
	private int kameraIdx;
	public Vector3 redFlagPoint;
	public Vector3 blueFlagPoint;
	string[,] map;
	List<Vector3> redPlayerSpawns;
	List<Vector3> bluePlayerSpawns;
	List<Spawn> itemSpawns;
	List<Spawn> weaponSpawns;
	Material redMaterial;
	Material blueMaterial;
	GameObject redFlag;
	GameObject blueFlag;
	int size = 0;
	public int redPoints = 0;
	public int bluePoints = 0;
	/* Level File
	 * 0 -> empty
	 * 1 -> wall
	 * 2 -> machine gun spawn point
	 * 3 -> health kit spawn point
	 * 4 -> UNUSED - player spawn point
	 * 5 -> water
	 * 6 -> armour spawn point
	 * 7 -> doomsday spawn point
	 * 8 -> hex damage spawn point
	 * 9 -> grenade spawn point
	 * r-> redflag
	 * b -> blueflag
	 * k -> redplayers
	 * m	 -> blueplayers
	 */
	// Use this for initialization
	void Start () {
		redMaterial = (Material) Resources.Load ("Red", typeof(Material));
		blueMaterial = (Material) Resources.Load ("Blue", typeof(Material));
		redFlag = (GameObject) GameObject.Instantiate(Resources.Load ("Flag"));
		blueFlag = (GameObject) GameObject.Instantiate(Resources.Load ("Flag"));
		redFlag.transform.FindChild("Bez").renderer.material = redMaterial;
		blueFlag.transform.FindChild("Bez").renderer.material = blueMaterial;
		redFlag.GetComponent<Flag>().team = "Red";
		blueFlag.GetComponent<Flag>().team = "Blue";
		spidx = 0;
		kameraSure = Time.time;
		kameraIdx = 0;
		LoadMap();
		LoadPlayers();
		ps = GameObject.FindGameObjectsWithTag("Player");
		sure = 310;
		kameraScript = kamera.GetComponent<Kamera>();
		kameraScript.player = ps[kameraIdx].transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (sure - Time.time < 0) {
			Time.timeScale = 0;
		}
		// Debug.Log (Time.time - kameraSure);
		if (Time.time - kameraSure > 10 || !ps[kameraIdx].activeSelf) {
			nextPlayer();
		}
		
		// loop through and spawn if necessary
		for(int i=0;i<weaponSpawns.Count;i++) {
			if (weaponSpawns[i].itemMesh == null && (Time.time - weaponSpawns[i].lastSpawn > weaponSpawns[i].spawnInterval)) {
				weaponSpawns[i].spawnItem();
			}
		}
		
		for(int i=0;i<itemSpawns.Count;i++) {
			if (itemSpawns[i].itemMesh == null && (Time.time - itemSpawns[i].lastSpawn > itemSpawns[i].spawnInterval)) {
				itemSpawns[i].spawnItem();
			}
		}
		
		for(int i=0;i<ps.Length;i++) {
			if(!ps[i].activeSelf && 
			   (Time.time - ps[i].GetComponent<AIScript>().getDisTime()) > 2) {
				if(ps[i].GetComponent<AIScript>().team == "Red") {
					ps[i].transform.position = redPlayerSpawns[spidx%redPlayerSpawns.Count];
				} else if(ps[i].GetComponent<AIScript>().team == "Blue") {
					ps[i].transform.position = bluePlayerSpawns[spidx%bluePlayerSpawns.Count];
				}
				ps[i].GetComponent<AIScript>().ClearValues();
				ps[i].SetActive(true);
				spidx++;
			} 
		}
	}
	
	void nextPlayer() {
		if(ps[kameraIdx].GetComponent<AIScript>().playername == "CONTROL") return;
		kameraIdx = (kameraIdx + 1) % ps.Length;
		kameraScript.player = ps[kameraIdx].transform;
		kameraSure = Time.time;
	}
	
	void OnGUI() {
		redPoints = 0; bluePoints = 0;
		/*for(int i=0;i<redPlayers.Count;i++) redPoints += redPlayers[i].GetComponent<AIScript>().getPuan();
		for(int i=0;i<bluePlayers.Count;i++) bluePoints += bluePlayers[i].GetComponent<AIScript>().getPuan();*/
		for(int i=0;i<ps.Length;i++) {
			if(ps[i].GetComponent<AIScript>().team == "Red") {
				redPoints += ps[i].GetComponent<AIScript>().getPuan();
			}
			if(ps[i].GetComponent<AIScript>().team == "Blue") {
				bluePoints += ps[i].GetComponent<AIScript>().getPuan();
			}
		}
		string display = "Red Team: " + redPoints + " \n" + "Blue Team: " + bluePoints + "\n";
		/*
		for(int i=0;i<ps.Length;i++) {
			AIScript temp = ps[i].GetComponent<AIScript>();
			display += temp.playername + "(Team: " + temp.team + ", " + temp.getHealth() + ", " + temp.getArmour() + "): " + temp.getPuan() + "\n";
		}*/
		display += "Time: " + (sure-Time.time) + "\n";
		display += "Camera is following: " + ps[kameraIdx].GetComponent<AIScript>().playername + ", Team " + ps[kameraIdx].GetComponent<AIScript>().team;
		GUI.Label(new Rect(5, 5, 400, 400), display);
	}
	
	public string[,] getMap() { return map; }
	
	void LoadPlayers() {
		if(redPlayers.Count > redPlayerSpawns.Count) {
			Debug.LogError("More red players than spawn points :(");
			return;
		}
		if(bluePlayers.Count > bluePlayerSpawns.Count) {
			Debug.LogError("More blue players than spawn points :(");
			return;
		}
		for(int i=0;i<redPlayers.Count;i++) {
			GameObject player = (GameObject) Instantiate(redPlayers[i], redPlayerSpawns[i], Quaternion.identity);
			player.GetComponent<AIScript>().team = "Red";
			player.GetComponent<AIScript>().playername = "Red " + (i+1);
			player.transform.FindChild("Govde").renderer.material = redMaterial;
			player.tag = "Player";
		}
		for(int i=0;i<bluePlayers.Count;i++) {
			GameObject player = (GameObject) Instantiate(bluePlayers[i], bluePlayerSpawns[i], Quaternion.identity);
			player.GetComponent<AIScript>().team = "Blue";
			player.GetComponent<AIScript>().playername = "Blue " + (i+1);
			player.transform.FindChild("Govde").renderer.material = blueMaterial;
			player.tag = "Player";
		}
	}
	
	void LoadMap() {
		StreamReader dosya = File.OpenText("Assets/Maps/" + LevelFileName);
		string icerik = dosya.ReadToEnd();
		dosya.Close();
		string[] satirlar = icerik.Split("\n"[0]);
		size = satirlar.Length;
		map = new string[size, size];
		Material zemin = (Material) Resources.Load ("Floor", typeof(Material));
		Material duvar = (Material) Resources.Load ("Duvar", typeof(Material));
		Material su    = (Material) Resources.Load ("Su", typeof(Material));
		redPlayerSpawns = new List<Vector3>();
		bluePlayerSpawns = new List<Vector3>();
		itemSpawns   = new List<Spawn>();
		weaponSpawns = new List<Spawn>();
		
		for(int i=0;i<size;i++) {
			string[] hucreler = satirlar[i].Split(" "[0]);
			for(int j=0;j<size;j++) {
				map[i, j] = hucreler[j].Trim();
				//int.TryParse(hucreler[j], out map[i, j]); 
				GameObject kare = GameObject.CreatePrimitive(PrimitiveType.Cube);
				kare.transform.position = new Vector3(i, -0.04f, j);
				kare.transform.localScale = new Vector3(1.0f, 0.01f, 1.0f);
				kare.renderer.material = zemin;
				switch(map[i, j]) {
				case "0":
					break;
				case "1":
					kare.transform.localScale = new Vector3(1.0f, 1.4f, 1.0f);
					kare.renderer.material = duvar;
					GameObject t1 = new GameObject("walltrigger");
					t1.transform.position = new Vector3(i, -0.04f, j);
					t1.transform.localScale = new Vector3(0.25f, 1, 0.25f);
					t1.AddComponent(typeof(BoxCollider));
					t1.collider.isTrigger = true;
					t1.layer = 11;
					t1.AddComponent(typeof(Obstacle));
					break;
				case "2":
					weaponSpawns.Add(new HeavyMachineGun(new Vector3(i, 0, j)));
					break;
				case "3":
					itemSpawns.Add (new HealthKit(new Vector3(i, 0, j)));
					break;
				case "4":
					//playerSpawns.Add(new Vector3(i, 0, j));
					break;
				case "5":
					kare.renderer.material = su;
					kare.AddComponent(typeof(WaterSimple));
					
					BoxCollider t12 = (BoxCollider)kare.collider;
					t12.isTrigger = true;
					t12.size = new Vector3(1, 80, 1);
					t12.center = new Vector3(0, 30, 0);
					
					kare.layer = 4; // water does not collide with bullets...
					
					GameObject t2 = new GameObject("watertrigger");
					t2.transform.position = new Vector3(i, -0.04f, j);
					t2.transform.localScale = new Vector3(0.25f, 1, 0.25f);
					t2.AddComponent(typeof(BoxCollider));
					t2.collider.isTrigger = true;
					t2.layer = 11;
					t2.AddComponent(typeof(Obstacle));
					break;
				case "6":
					itemSpawns.Add (new Armour(new Vector3(i, 0, j)));
					break;
				case "7":
					weaponSpawns.Add (new Doomsday(new Vector3(i, 0, j)));
					break;
				case "8":
					itemSpawns.Add (new HexDamage(new Vector3(i, 0, j)));
					break;
				case "9":
					weaponSpawns.Add (new Grenade(new Vector3(i, 0, j)));
					break;
				case "r":
					redFlagPoint = new Vector3(i, 0, j);
					redFlag.transform.position = redFlagPoint;
					break;
				case "b":
					blueFlagPoint = new Vector3(i, 0, j);
					blueFlag.transform.position = blueFlagPoint;
					break;
				case "k":
					redPlayerSpawns.Add(new Vector3(i, 0, j));
					break;
				case "m":
					bluePlayerSpawns.Add(new Vector3(i, 0, j));
					break;
				}
			}
		}
	}
}
