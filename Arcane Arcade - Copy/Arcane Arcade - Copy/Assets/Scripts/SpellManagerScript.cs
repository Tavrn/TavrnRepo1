using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CNamespace;
public class SpellManagerScript : MonoBehaviour {
	public Transform wandTip;
	public Transform wandHandle;
	public WandScript wandScript;
	[Space(10)]
	public Transform minionSpawn1;
	public Transform minionSpawn2;
	public GameObject minionPrefab;
	[Space(10)]
	public Skybox playerSkybox;
	public Material clearSkiesSkybox;
	public Material fireSkybox;
	[Space(10)]
	public GameObject fireballPrefab;
	private int cuedSpellNum = -1;
	List<Coordinate> fireball = new List<Coordinate>();
	List<Coordinate> airslash = new List<Coordinate>();
	List<Coordinate> spawnMinion = new List<Coordinate>();
	List<Coordinate> weatherClear = new List<Coordinate>();
	List<Coordinate> weatherFire = new List<Coordinate>();
	List<List<Coordinate>> spellCompendium = new List<List<Coordinate>>();
	List<string> spellNameCompendium = new List<string>();
	void Start () {
		setUpSpellCompendium();
		setUpSpellNameCompendium();
	}
	void setUpSpellNameCompendium(){
		spellNameCompendium.Add("Fireball");
		spellNameCompendium.Add("AirSlash");
		spellNameCompendium.Add("I_SpawnMinion");
		spellNameCompendium.Add("I_WeatherClear");
		spellNameCompendium.Add("I_WeatherFire");
	}
	void setUpSpellCompendium(){
		fireball.Add(new Coordinate(0,0,0));
		fireball.Add(new Coordinate(0,1,0));
		spellCompendium.Add(fireball);
		airslash.Add(new Coordinate(0,0,0));
		airslash.Add(new Coordinate(0,-1,0));
		spellCompendium.Add(airslash);
		spawnMinion.Add(new Coordinate(0,0,0));
		spawnMinion.Add(new Coordinate(0,0,1));
		spawnMinion.Add(new Coordinate(0,0,2));
		spellCompendium.Add(spawnMinion);
		weatherClear.Add(new Coordinate(0,0,0));
		weatherClear.Add(new Coordinate(1,0,0));
		weatherClear.Add(new Coordinate(1,1,0));
		weatherClear.Add(new Coordinate(2,1,0));
		spellCompendium.Add(weatherClear);
		weatherFire.Add(new Coordinate(0,0,0));
		weatherFire.Add(new Coordinate(-1,0,0));
		weatherFire.Add(new Coordinate(-1,1,0));
		weatherFire.Add(new Coordinate(-2,1,0));
		spellCompendium.Add(weatherFire);
	}
	public void checkForSpell(){
		List<Coordinate> pattern = wandScript.pattern;
		if(pattern.Count>0){ //make sure the origin collider is in the pattern list
			if(pattern[0].getX()!=0 || pattern[0].getY() != 0 || pattern[0].getZ()!=0){
				pattern.Insert(0, new Coordinate(0,0,0));
			}
		}
		for(int spellNum = 0; spellNum<spellCompendium.Count; spellNum++){ //check pattern against every spell in the compendium
			if(compareSpells(spellCompendium[spellNum], pattern)){
				GetComponent<AudioSource>().Play();
				string temp = spellNameCompendium[spellNum];
				if(temp[0] == 'I' && temp[1] == '_'){ //check if spell is instantly cast or not
					Invoke(spellNameCompendium[spellNum], 0f);
					cuedSpellNum = -1;
				}else{
					cuedSpellNum = spellNum;
				}
			}
		}
		wandScript.pattern.Clear();
	}
	bool compareSpells(List<Coordinate> spell1, List<Coordinate> spell2){
		Debug.Log("Counts: " + spell1.Count + " " + spell2.Count);
		if(spell1.Count!=spell2.Count){
			return false;
		}else{
			bool result = true;
			for(int i=0; i<spell1.Count; i++){
				if(!compareCoordinates(spell1[i], spell2[i])){
					result = false;
				}
			}
			return result;
		}
	}
	bool compareCoordinates(Coordinate a, Coordinate b){
		// Debug.Log(a.getX() + " " + b.getX() + " " + a.getY() + " " + b.getY() + " " + a.getZ() + " " + b.getZ());
		return a.getX()==b.getX() && a.getY() == b.getY() && a.getZ() == b.getZ();
	}
	public void FireSpell(){
		if(cuedSpellNum!=-1){
			Invoke(spellNameCompendium[cuedSpellNum], 0f);
			cuedSpellNum = -1;
		}
	}
	void Fireball(){
		Debug.Log("Fireball called");
		Vector3 dir = wandTip.position-wandHandle.position;
		GameObject fb = Instantiate(fireballPrefab) as GameObject;
		fb.transform.position = wandTip.position+dir;
		fb.GetComponent<Rigidbody>().AddForce(dir.normalized*10);
	}
	void AirSlash(){
		Debug.Log("Air slash called");
	}
	void I_SpawnMinion(){
		Debug.Log("spawn minion called");
		GameObject minion = Instantiate(minionPrefab) as GameObject;
		minion.transform.position = minionSpawn1.position;
	}
	void I_WeatherClear(){
		playerSkybox.material = clearSkiesSkybox;
	}
	void I_WeatherFire(){
		playerSkybox.material = fireSkybox;
	}
}
