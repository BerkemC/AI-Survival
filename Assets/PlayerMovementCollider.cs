using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCollider : MonoBehaviour {

	void OnTriggerEnter(Collider col){

		if(col.tag == "Player"){
			print ("hit");
			GameObject.Find ("Player").GetComponent <CompleteProject.PlayerMovement> ().norm = Vector3.zero;
			GameObject.Find ("Player").GetComponent <CompleteProject.PlayerMovement> ().ChangeTargetNode ();

		}
	}
}
