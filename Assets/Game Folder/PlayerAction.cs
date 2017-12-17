using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAction : MonoBehaviour {

	private bool shouldCalculateSequence;
	private GameObject player;
	private GameObject enemies;
	private GameObject healthPickups;
	private GameObject ammoPickups;
	private GameObject bat;
	private PlayerHealth ph;
	private CompleteProject.PlayerMovement pm;
	private PlayerShooting ps;


	private ActionSequence currentSequence;
	private bool isEscaping = false;
	private bool isShooting = false;
	public bool isMelee = false;
	private GameObject currentEnemy;


	public Text enemyNumberText;

	public bool IsEscaping {
		get {
			return isEscaping;
		}
		set {
			isEscaping = value;
		}
	}

	public bool ShouldCalculateSequence {
		get {
			return shouldCalculateSequence;
		}
		set {
			shouldCalculateSequence = value;
		}
	}

	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
		enemies = GameObject.Find ("Enemies");
		healthPickups = GameObject.Find ("HealthPickups");
		ammoPickups = GameObject.Find ("AmmoPickups");
		bat = player.transform.Find ("Bat").gameObject;

		ph = player.GetComponent<PlayerHealth> ();
		pm = player.GetComponent<CompleteProject.PlayerMovement> ();
		ps = player.transform.Find ("GunBarrelEnd").gameObject.GetComponent<PlayerShooting> ();



		ShouldCalculateSequence = true;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		if(isShooting && currentEnemy != null){
			
			player.transform.LookAt (currentEnemy.transform);
		}

		if(isMelee){

			player.transform.LookAt (currentEnemy.transform);
		}

		if(isEscaping){

			GameObject closestEnemy = FindClosestEnemy ();

			if (closestEnemy && Vector3.Distance (closestEnemy.transform.position, player.transform.position) > GameObject.FindObjectOfType<GOAP> ().enemyDistanceLimit) {
				GameObject closestAmmo = FindClosestAmmo ();
				GameObject closestHealth = FindClosestHealth ();

				if (closestAmmo && closestHealth) {

					if (Vector3.Distance (player.transform.position, closestAmmo.transform.position) < Vector3.Distance (player.transform.position, closestAmmo.transform.position)) {

						pm.Destination = closestAmmo.transform.position;
						pm.StartMovement ();

					} else {
						pm.Destination = closestHealth.transform.position;
						pm.StartMovement ();
					}
				} else if (closestAmmo && !closestHealth) {

					pm.Destination = closestAmmo.transform.position;
					pm.StartMovement ();
				} else if (!closestAmmo && closestHealth) {

					pm.Destination = closestHealth.transform.position;
					pm.StartMovement ();
				} else {

					Vector3 randomLoc = new Vector3 (Random.Range (-65f, 15f), 1f, Random.Range (-20f, 15f));
					pm.Destination = randomLoc;
					pm.StartMovement ();
				}
			}

		}


		enemyNumberText.text = "Enemies: " + enemies.transform.childCount.ToString ();
	}


	public void ExecuteSequence(ActionSequence sequence){

		ResetForNewSequence (sequence);

		if(sequence.sequenceName.Equals ("Melee Kill Sequence")){

			GameObject closestEnemy = FindClosestEnemy ();

			if(closestEnemy){
				isMelee = true;
				pm.Destination = closestEnemy.transform.position;
				pm.StartMovement ();
				currentEnemy = closestEnemy;

				bat.SetActive (true);
			}
			else{

				shouldCalculateSequence = true;
			}
			ShouldCalculateSequence = true;

			isEscaping = false;
		}else if(sequence.sequenceName.Equals ("Ranged Kill Sequence")){
			
			GameObject closestEnemy = FindClosestEnemy ();

			if(closestEnemy){
				pm.Destination = closestEnemy.transform.position;
				pm.StartMovement ();
				ps.target = closestEnemy;
				currentEnemy = closestEnemy;
				isShooting = true;
			}

			isMelee = false;
			shouldCalculateSequence = true;
			isEscaping = false;
		}else if(sequence.sequenceName.Equals ("Gather Health Sequence")){
			
			GameObject closestHealthPickup = FindClosestHealth ();

			if(closestHealthPickup){
				pm.Destination = closestHealthPickup.transform.position;
				pm.StartMovement ();

				
			}else{

				shouldCalculateSequence = true;
			}

			isMelee = false;
			isEscaping = false;

		}else if(sequence.sequenceName.Equals ("Gather Ammo Sequence")){

			GameObject closestAmmoPickup = FindClosestAmmo ();

			if(closestAmmoPickup){
				pm.Destination = closestAmmoPickup.transform.position;
				pm.StartMovement ();

			}else{
				
				ShouldCalculateSequence = true;
			}

			isMelee = false;
			isEscaping = false;
		}else if(sequence.sequenceName.Equals ("Escape Sequence")){

			if(pm.Path.isEmpty ()){
				Vector3 randomLocation = new Vector3 (Random.Range (-65f, 15f), 1f, Random.Range (-20f, 15f));
				pm.Destination = randomLocation;
				pm.StartMovement ();

			}
			
			isMelee = false;
			isEscaping = true;
			shouldCalculateSequence = true;
		}



	}



	void ResetForNewSequence (ActionSequence sequence)
	{
		shouldCalculateSequence = false;
		currentSequence = sequence;
		if(!isEscaping) pm.Destination = player.transform.position;
		ps.target = null;
	}

	public void ResetForDeadTarget(){
		

		ps.target = null;
		currentEnemy = null; 
		ShouldCalculateSequence = true;
		isMelee = false;
		bat.SetActive (false);
	}



	private GameObject FindClosestEnemy(){

		float distance = 100f;
		GameObject enemy = null;

		foreach(Transform child in enemies.transform){
			
			if(Vector3.Distance(player.transform.position,child.position) < distance){
				
				distance = Vector3.Distance (player.transform.position, child.position);
				enemy = child.gameObject;
			}
		}

		return enemy;

	}

	private GameObject FindClosestHealth(){

		float distance = 100f;
		GameObject health = null;

		foreach(Transform child in healthPickups.transform){

			if(Vector3.Distance(player.transform.position,child.position) < distance){

				distance = Vector3.Distance (player.transform.position, child.position);
				health = child.gameObject;
			}
		}

		return health;
	}

	private GameObject FindClosestAmmo(){

		float distance = 100f;
		GameObject ammo = null;

		foreach(Transform child in ammoPickups.transform){

			if(Vector3.Distance(player.transform.position,child.position) < distance){

				distance = Vector3.Distance (player.transform.position, child.position);
				ammo = child.gameObject;
			}
		}

		return ammo;
	}
}
