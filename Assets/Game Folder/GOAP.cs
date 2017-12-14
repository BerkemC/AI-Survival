using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GOAP : MonoBehaviour {

	//Object references
	private GameObject player;
	private GameObject enemies;
	private GameObject healthPickups;
	private GameObject ammoPickups;

	public float ammoGatheringCoeff;
	public float healthGatheringCoeff;
	public float shootToKillCoeff;
	public float meleeKillCoeff;
	public float escapeCoeff;


	public float enemyDistanceLimit;
	public int lowHealthLimit;
	public int lowAmmoLimit;

	private List<ActionSequence> sequenceList;



	public Text SequenceGoapText;

	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
		enemies = GameObject.Find ("Enemies");
		healthPickups = GameObject.Find ("HealthPickups");
		ammoPickups = GameObject.Find ("AmmoPickups");
		sequenceList = new List<ActionSequence> ();


	}
	void Start(){
		InitializeSequences ();
		print (sequenceList [0].sequenceName);
	}
	
	// Update is called once per frame
	void Update () {

		CalculateNextBestSequence ();
	}


	void InitializeSequences(){

		List<ActionSequence.allEffects> effects = new List<ActionSequence.allEffects> ();
		List<ActionSequence.allPreconditions> conditions = new List<ActionSequence.allPreconditions> ();
		List<ActionSequence.allPreconditions> bonusConditions = new List<ActionSequence.allPreconditions> ();

		effects.Add (ActionSequence.allEffects.meleeStrike);
		effects.Add (ActionSequence.allEffects.lowerHealth);
		conditions.Add (ActionSequence.allPreconditions.highHealth);
		bonusConditions.Add (ActionSequence.allPreconditions.enemyClose);
		bonusConditions.Add (ActionSequence.allPreconditions.lowAmmo);

		ActionSequence meleeKillSequence = new ActionSequence("Melee Kill Sequence",conditions,effects,bonusConditions);
		sequenceList.Add (meleeKillSequence);

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		effects = new List<ActionSequence.allEffects> ();
		conditions = new List<ActionSequence.allPreconditions> ();
		bonusConditions = new List<ActionSequence.allPreconditions> ();

		effects.Add (ActionSequence.allEffects.rangedStrike);
		effects.Add (ActionSequence.allEffects.lowerAmmo);
		conditions.Add (ActionSequence.allPreconditions.highAmmo);
		bonusConditions.Add (ActionSequence.allPreconditions.enemyFar);
		bonusConditions.Add (ActionSequence.allPreconditions.lowHealth);

		ActionSequence rangedKillSequence = new ActionSequence("Ranged Kill Sequence",conditions,effects,bonusConditions);
		sequenceList.Add (rangedKillSequence);


		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		effects = new List<ActionSequence.allEffects> ();
		conditions = new List<ActionSequence.allPreconditions> ();
		bonusConditions = new List<ActionSequence.allPreconditions> ();

		effects.Add (ActionSequence.allEffects.moreHealth);
		conditions.Add (ActionSequence.allPreconditions.lowHealth);

		ActionSequence gatherHealthSequence = new ActionSequence("Gather Health Sequence",conditions,effects,bonusConditions);
		sequenceList.Add (gatherHealthSequence);

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		effects = new List<ActionSequence.allEffects> ();
		conditions = new List<ActionSequence.allPreconditions> ();
		bonusConditions = new List<ActionSequence.allPreconditions> ();

		effects.Add (ActionSequence.allEffects.moreAmmo);
		conditions.Add (ActionSequence.allPreconditions.lowAmmo);

		ActionSequence gatherAmmoSequence = new ActionSequence("Gather Ammo Sequence",conditions,effects,bonusConditions);
		sequenceList.Add (gatherAmmoSequence);


		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		effects = new List<ActionSequence.allEffects> ();
		conditions = new List<ActionSequence.allPreconditions> ();
		bonusConditions = new List<ActionSequence.allPreconditions> ();

		effects.Add (ActionSequence.allEffects.escape);
		conditions.Add (ActionSequence.allPreconditions.lowHealth);
		bonusConditions.Add (ActionSequence.allPreconditions.enemyClose);
		bonusConditions.Add (ActionSequence.allPreconditions.lowAmmo);


		ActionSequence escapeSequence = new ActionSequence("Escape Sequence",conditions,effects,bonusConditions);
		sequenceList.Add (escapeSequence);
	}

	ActionSequence.allPreconditions isEnemyClose(){
		float closest = 100f;

		foreach(Transform child in enemies.transform){

			float distance = Vector3.Distance (player.transform.position, child.transform.position);

			if(distance < closest){

				closest = distance;
			}

		}


		if (closest < enemyDistanceLimit)
			return ActionSequence.allPreconditions.enemyClose;
		return ActionSequence.allPreconditions.enemyFar;
	}
	private ActionSequence.allPreconditions isHealthLow(){
		if(player.GetComponent<PlayerHealth>().currentHealth < lowHealthLimit){
			return ActionSequence.allPreconditions.lowHealth;
		}
		return ActionSequence.allPreconditions.highHealth;
	}

	private ActionSequence.allPreconditions isAmmoLow(){
		if(player.transform.GetComponentInChildren<PlayerShooting>().currentAmmo < lowAmmoLimit){
			return ActionSequence.allPreconditions.lowAmmo;
		}
		return ActionSequence.allPreconditions.highAmmo;
	}


	private void CalculateNextBestSequence(){

		ActionSequence.allPreconditions playerHealth = isHealthLow ();
		ActionSequence.allPreconditions playerAmmo = isAmmoLow();
		ActionSequence.allPreconditions enemyCloseness = isEnemyClose ();


		List<ActionSequence.allPreconditions> currentConditions = new List<ActionSequence.allPreconditions> ();
		currentConditions.Add (playerHealth);
		currentConditions.Add (playerAmmo);
		currentConditions.Add (enemyCloseness);

		int highestScore = 0;
		int index = 0;
		int i = 0;
		foreach(var x in sequenceList){

			int score = 0;

			foreach(var y in currentConditions){

				if(x.isConditionValid (y)){
					score+=2;
				}else if(x.isConditionValidForBonus (y)){
					score++;
				}

			}

			if(score > highestScore){
				highestScore = score;
				index = i; 
			}
			i++;
		}
		print (index+" "+highestScore);
		SequenceGoapText.text = sequenceList [index].sequenceName;


	}

		
}
