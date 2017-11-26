using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSMesh : MonoBehaviour {

	public ColliderManagement collider;
	public int xOffset;
	public int zOffset;
	public int xAxis;
	public int zAxis;


	public Vector3[,] matrix ;
	bool[,] visitedMatrix;
	public GameObject obstacles1,obstacles2;


	public GameObject spawn;
	public bool isShown;


	public bool isGenerated = false;
	// Use this for initialization

	void Awake(){
		visitedMatrix = new bool[zAxis+zOffset,xAxis+xOffset];
		matrix = new Vector3[zAxis+zOffset,xAxis+xOffset];


		DeleteObstaclesFromMatrix ();


		BFSMeshGeneration ();
		isGenerated = true;

		if(isShown){
			ShowGrid ();
		}
	}

	void ShowGrid ()
	{
		for (int i = 0; i < zAxis; i++) {
			for (int j = 0; j < xAxis; j++) {
				Instantiate (spawn, matrix [i, j], Quaternion.identity);
			}
		}
	}

	void DeleteObstaclesFromMatrix ()
	{
		foreach (Transform child in obstacles1.transform) {
			MarkWithRange (child);
			foreach (Transform child2 in child) {
				MarkWithRange (child2);
			}
		}
		foreach (Transform child in obstacles2.transform) {
			MarkWithRange (child);
			foreach (Transform child2 in child) {
				MarkWithRange (child2);
			}
		}
	}

	private void ConvertToIndex(out int zAx, out int xAx, Vector3 location){
		xAx = (int)(location.x + 73.5f);
		zAx = (int)(location.z + 24.5f);
	}
	private void MarkWithRange(Transform child){
		int zc, xc;

		ConvertToIndex (out zc,out xc,child.position);
		visitedMatrix [zc, xc] = true;

	}
	private void BFSMeshGeneration(){
		


		Vector3 startPoint = GenerateStartPoint ();
		int z, x;

		ConvertToIndex (out z,out  x, startPoint);
		matrix [z, x] = startPoint;
		//Set start point as visited
		visitedMatrix [z, x] = true;


		//Frontier queue
		Queue frontier = new Queue ();
		//Add start point to the queue
		frontier.Enqueue (startPoint);


		while(!frontier.isEmpty ()){
			
			Vector3 current = frontier.Dequeue ();


			List<Vector3> neighbors = collider.GetNeighbors (current);


			neighbors.ForEach (delegate(Vector3 obj) {

					
				ConvertToIndex (out z,out  x, obj);


				if(!visitedMatrix[z,x]){
					
					matrix[z,x] = obj;
					frontier.Enqueue (obj);
					visitedMatrix[z,x] = true;
				}

			});

		}
	}


	private Vector3 GenerateStartPoint(){

		return collider.transform.position;
	}
}
