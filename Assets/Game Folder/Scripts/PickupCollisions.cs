using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollisions : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		
		if(col.gameObject.tag.Equals ("Player"))
        {

			if(gameObject.tag.Equals ("Health"))
            {
				
				GameObject.FindObjectOfType<PlayerHealth> ().currentHealth =100;
				GameObject.FindObjectOfType<PlayerAction> ().ShouldCalculateSequence = true;
				GameObject.FindObjectOfType<PlayerHealth> ().healthSlider.value = 100; 
			}
            else if(gameObject.tag.Equals ("Ammo"))
            {

				GameObject.FindObjectOfType<PlayerShooting> ().currentAmmo = 50;
				GameObject.FindObjectOfType<PlayerAction> ().ShouldCalculateSequence = true;
			}

			Destroy (gameObject.transform.parent.gameObject);
		}
	}
}
