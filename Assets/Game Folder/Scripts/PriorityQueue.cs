using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue : MonoBehaviour {

	public struct LocationNode{
		public Vector3 location;
		public float priority;
	};

	public List<LocationNode> priorityQueue;


	public PriorityQueue(){
		
		priorityQueue = new List<LocationNode> ();
	}

	public void Enqueue(LocationNode node){
		
		for(int i = 0; i < priorityQueue.Count ; i++){
			
			if(node.priority < priorityQueue[i].priority){

				priorityQueue.Insert (i,node);
				return;
			}
		}


		priorityQueue.Add (node);

	}

	public Vector3 Dequeue(){

		Vector3 temp = priorityQueue [0].location;
		priorityQueue.RemoveAt (0);

		return temp;
	}

	public bool isEmpty(){

		return (priorityQueue.Count == 0);
	}
}
