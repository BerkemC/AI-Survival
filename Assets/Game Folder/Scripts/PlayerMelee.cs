using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour {

    [SerializeField]
	private int meleeDamage ;
	
	void OnTriggerEnter(Collider col)
    {
		
		if(col.tag.Equals ("Enemy"))
        {
			
			col.GetComponent <CompleteProject.EnemyHealth>().TakeDamage (meleeDamage,transform.position);

		}
	}

	
}
