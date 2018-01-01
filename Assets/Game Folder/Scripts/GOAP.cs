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
	private PlayerAction pa;
	private PlayerShooting ps;
	private PlayerHealth ph;

	public int startingAmmo;

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
		pa = GameObject.FindObjectOfType<PlayerAction> ();
		ps = player.transform.Find ("GunBarrelEnd").gameObject.GetComponent<PlayerShooting> ();
		ph = player.GetComponent<PlayerHealth> ();

	}
	void Start(){
		InitializeSequences ();
	}
	
	// Update is called once per frame
	void Update () {


		if(pa.ShouldCalculateSequence){
			
			CalculateNextBestSequence ();
		}

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
		bonusConditions.Add (ActionSequence.allPreconditions.enemyFar);
		bonusConditions.Add (ActionSequence.allPreconditions.noEnemy);

		ActionSequence gatherHealthSequence = new ActionSequence("Gather Health Sequence",conditions,effects,bonusConditions);
		sequenceList.Add (gatherHealthSequence);

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		effects = new List<ActionSequence.allEffects> ();
		conditions = new List<ActionSequence.allPreconditions> ();
		bonusConditions = new List<ActionSequence.allPreconditions> ();


		effects.Add (ActionSequence.allEffects.moreAmmo);
		conditions.Add (ActionSequence.allPreconditions.lowAmmo);
		bonusConditions.Add (ActionSequence.allPreconditions.enemyFar);
		bonusConditions.Add (ActionSequence.allPreconditions.noEnemy);


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

		if (enemies.transform.childCount == 0)
			return ActionSequence.allPreconditions.noEnemy;

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
	float closestEnemy(){
		float closest = 100f;

		foreach(Transform child in enemies.transform){

			float distance = Vector3.Distance (player.transform.position, child.transform.position);

			if(distance < closest){

				closest = distance;
			}

		}
		return closest;
	}
	private ActionSequence.allPreconditions isHealthLow(){
		
		if(player.GetComponent<PlayerHealth>().currentHealth < lowHealthLimit)
		{
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

			List<ActionSequence> suitableSequences = new List<ActionSequence> ();
			List<float> scores = new List<float> ();
			int i = 0;
			foreach(var x in sequenceList){

				float score = 0;
				int satisfiedConditions = 0;

				foreach(var y in currentConditions){

					if(x.isConditionValid (y)){

						score+= CalculateConditionScore(y,x);
						satisfiedConditions++;

					}else if(x.isConditionValidForBonus (y)){

						score += CalculateConditionScore(y,x)/2f;
					}

				}

				if(satisfiedConditions == x.preconditions.Count){

					suitableSequences.Add (x);
					scores.Add (score);
				}

			}

			ActionSequence resultingSequence = suitableSequences [FindHighestScore (scores)];

			SequenceGoapText.text = resultingSequence.sequenceName;

			pa.ExecuteSequence (resultingSequence);



	}


	private float CalculateConditionScore(ActionSequence.allPreconditions condition,ActionSequence currentSequence){

		if(condition.Equals (ActionSequence.allPreconditions.enemyClose)){

			return  (2f/(enemyDistanceLimit))*closestEnemy ();


		}else if (condition.Equals (ActionSequence.allPreconditions.enemyFar)){

			if(currentSequence.sequenceName.Equals ("Gather Health Sequence") )
			{
				return (closestEnemy () - enemyDistanceLimit) * (1f - ((1f / ph.startingHealth) * ph.currentHealth))/2f;
			}
			else if (currentSequence.sequenceName.Equals ("Gather Ammo Sequence") )
			{
				return (closestEnemy () - enemyDistanceLimit) * (1f - ((1f / startingAmmo) * ps.currentAmmo)) / 2f;
			}

			return (closestEnemy () - enemyDistanceLimit)*(1f/enemyDistanceLimit);

		}else if (condition.Equals (ActionSequence.allPreconditions.lowAmmo)){
			
			if(currentSequence.sequenceName.Equals ("Melee Kill Sequence"))
			{
				return  1f - ((1f / startingAmmo) * ps.currentAmmo);
			}

			return 1f-((1f/startingAmmo)*ps.currentAmmo) + ammoPickups.transform.childCount * 0.25f;

		}else if (condition.Equals (ActionSequence.allPreconditions.highAmmo)){

			return ((1f/startingAmmo)*ps.currentAmmo);

		}else if (condition.Equals (ActionSequence.allPreconditions.lowHealth)){

			if(currentSequence.sequenceName.Equals ("Ranged Kill Sequence"))
			{
				return 1f - ((1f / ph.startingHealth) * ph.currentHealth);
			}
			
			return 1f-((1f/ph.startingHealth)*ph.currentHealth) + healthPickups.transform.childCount * 0.25f;

		}else if (condition.Equals (ActionSequence.allPreconditions.highHealth)){
			return ((1f/ph.startingHealth)*ph.currentHealth);
		}
		else if (condition.Equals (ActionSequence.allPreconditions.noEnemy)){
			return 2f + (healthPickups.transform.childCount + ammoPickups.transform.childCount) * 0.1f;
		}


		return 0f;

	}

	private int FindHighestScore(List<float> scores){
		int index = 0;
		float highestScore = 0f;

		for(int i = 0;i<scores.Count;i++){
			if(scores[i] > highestScore){
				highestScore = scores [i];
				index = i;
			}
		}
		print (highestScore);
		return index;
	}
		
}
