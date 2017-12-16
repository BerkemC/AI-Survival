using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour {

	public int MeleeDamage = 50;

	void OnTriggerEnter(Collider col){
		
		if(col.tag.Equals ("Enemy")){
			
			col.GetComponent <CompleteProject.EnemyHealth>().TakeDamage (MeleeDamage,transform.position);

		}
	}
}
