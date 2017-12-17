using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour {

	public int MeleeDamage ;
	//public float meleeIntervals;
	void OnTriggerEnter(Collider col){
		
		if(col.tag.Equals ("Enemy")){
			
			col.GetComponent <CompleteProject.EnemyHealth>().TakeDamage (MeleeDamage,transform.position);

		}
	}

	/*void OnTriggerStay(Collider col){

		if(col.tag.Equals ("Enemy") && Random.value < meleeIntervals*(Time.deltaTime/4)){

			col.GetComponent <CompleteProject.EnemyHealth>().TakeDamage (MeleeDamage,transform.position);

		}
	}*/
}
