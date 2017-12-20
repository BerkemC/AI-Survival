using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

	private Transform targetPlayer;
	public Vector3 offset;
	void Awake(){
		//Find the player reference
		targetPlayer = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 targetPosition = targetPlayer.transform.position + offset;
		Vector3 smoothPosition = Vector3.Lerp (transform.position, targetPosition, 0.125f);
		transform.position = smoothPosition;


		transform.LookAt (targetPlayer);

	}
}
