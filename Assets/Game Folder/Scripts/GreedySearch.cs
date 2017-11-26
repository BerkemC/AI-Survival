﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedySearch : MonoBehaviour {

	private Vector3[,] map ;
	BFSMesh BFSCredentials; 

	// Use this for initialization
	void Start () {
		
		BFSCredentials = GameObject.FindObjectOfType<BFSMesh> ();

		//Get the matrix created by bfs
		if(BFSCredentials.isGenerated){
			print ("Generated");
			map = BFSCredentials.matrix;
		}else{
			print ("Not generated");
		}


	}

	/// <summary>
	/// Heuristics function that returns the manhattan distance.
	/// </summary>
	/// <returns>The manhattan distance</returns>
	/// <param name="currentPosition">Current position.</param>
	/// <param name="targetPosition">Target position.</param>
	public float HeuristicFunction(Vector3 currentPosition, Vector3 targetPosition){

		return Vector3.Distance (currentPosition, targetPosition);
	}

	/// <summary>
	/// Gets the greedy best first search path.
	/// </summary>
	/// <returns>The greedy best first search path.</returns>
	/// <param name="currentPosition">Current position.</param>
	/// <param name="targetPosition">Target position.</param>
	public Queue GetGreedyBestFirstSearchPath(Vector3 currentPosition , Vector3 targetPosition){

		//Round down the current position
		currentPosition = new Vector3 ((float)((int)currentPosition.x), (float)((int)currentPosition.y), (float)((int)currentPosition.z));

		//Result List
		Queue ResultingPath = new Queue ();

		//Visited list to prevent loops
		bool[,] visitedList = new bool[200,200];
		//Frontier queue
		PriorityQueue frontier = new PriorityQueue ();
		//Create node for start point
		PriorityQueue.LocationNode currentNode = new PriorityQueue.LocationNode ();
		//Give its credentials
		currentNode.location = currentPosition;
		currentNode.priority = 0f;
		//Add it to queue
		frontier.Enqueue (currentNode);
	

		while (!frontier.isEmpty ()) {

			Vector3 currentPlace = frontier.Dequeue ();
			//print (currentPlace+" "+Vector3.Distance (currentPlace,targetPosition));

			if (currentPlace.Equals (targetPosition)) {
				print ("Found target");
				ResultingPath.Enqueue (currentPlace);
				return ResultingPath;
			} else {
				


				int x, z;
				ConvertToIndex (out z, out x, GetShortestNeighbor (currentPlace, targetPosition));


				if (!visitedList [z, x]) {

					PriorityQueue.LocationNode tempNode = new PriorityQueue.LocationNode ();
					tempNode.location = map[z,x];
					tempNode.priority = HeuristicFunction (map[z,x], targetPosition);
					frontier.Enqueue (tempNode);
					visitedList [z, x] = true;
					ResultingPath.Enqueue (tempNode.location);

				}

			}


		}

		return ResultingPath;

	}

	/// <summary>
	/// Gets the valid neighbors.
	/// </summary>
	/// <returns>The valid neighbors.</returns>
	/// <param name="currentPosition">Current position.</param>
	private Vector3 GetShortestNeighbor(Vector3 currentPosition,Vector3 targetPosition){
		PriorityQueue result = new PriorityQueue ();
		int x, z;
		ConvertToIndex (out z,out x,currentPosition);


		for(int i = -2; i < 3 ;i++){
			for(int j = -2 ; j < 3 ; j++){
				
				if((x+j) < 98 && (x+j) > 0 && (z+ i) < 49 && (z+i) > 0){
					if(!map [(z + i), (x + j)].Equals (Vector3.zero)){
						PriorityQueue.LocationNode tempNode = new PriorityQueue.LocationNode ();
						tempNode.location = map [z + i, x + j];
						tempNode.priority= HeuristicFunction (map [z + i, x + j], targetPosition);
						result.Enqueue (tempNode);
					}
				}
			}
		}


		return result.Dequeue ();
	}

	/// <summary>
	/// Converts the position to index, specific for this map.
	/// </summary>
	/// <param name="zAx">Z ax.</param>
	/// <param name="xAx">X ax.</param>
	/// <param name="location">Location.</param>
	private void ConvertToIndex(out int zAx, out int xAx, Vector3 location){
		xAx = (int)(location.x + 73.5f);
		zAx = (int)(location.z + 24.5f);
	}

}