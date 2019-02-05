using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedySearch : MonoBehaviour {

	private Vector3[,] map ;
	BFSMesh BFSCredentials; 

	// Use this for initialization
	void Start ()
    {
		
		BFSCredentials = GameObject.FindObjectOfType<BFSMesh> ();

		//Get the matrix created by bfs
		if(BFSCredentials.isGenerated)
        {
			map = BFSCredentials.matrix;
		}
        else
        {
            throw new UnityException("Failed to generate mesh");
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
	public Queue GetGreedyBestFirstSearchPath(Vector3 currentPosition , Vector3 targetPosition)
    {

		//Round down the current position
		currentPosition = new Vector3 ((float)((int)currentPosition.x), (float)((int)currentPosition.y), (float)((int)currentPosition.z));
		//Round down the target position
		targetPosition = new Vector3 ((float)((int)targetPosition.x), (float)((int)targetPosition.y), (float)((int)targetPosition.z));

		//Result List
		Queue ResultingPath = new Queue ();

		//Visited list to prevent loops
		bool[,] visitedList = new bool[200,200];
		//Frontier queue
		Queue frontier = new Queue ();
	
		//Add starting node to queue
		frontier.Enqueue (currentPosition);
	

		while (!frontier.IsEmpty ())
        {

			Vector3 currentPlace = frontier.Dequeue ();


			if (currentPlace.Equals (targetPosition))
            {
				
				ResultingPath.Enqueue (currentPlace);
				return ResultingPath;
			}
            else
            {
				int x, z;
				ConvertToIndex (out z, out x, GetShortestNeighbor (currentPlace, targetPosition));


				if (!visitedList [z, x])
                {
					frontier.Enqueue (map[z,x]);
					visitedList [z, x] = true;
					ResultingPath.Enqueue (map[z,x]);

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
	private Vector3 GetShortestNeighbor(Vector3 currentPosition,Vector3 targetPosition)
    {
		PriorityQueue result = new PriorityQueue ();
		int x, z;
		ConvertToIndex (out z,out x,currentPosition);


		for(int i = -2; i < 3 ;i++)
        {
			for(int j = -2 ; j < 3 ; j++)
            {
				
				if((x+j) < 98 && (x+j) > 0 && (z+ i) < 49 && (z+i) > 0)
                {
					if(!map [(z + i), (x + j)].Equals (Vector3.zero))
                    {
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
	private void ConvertToIndex(out int zAx, out int xAx, Vector3 location)
    {
		xAx = (int)(location.x + 73.5f);
		zAx = (int)(location.z + 24.5f);
	}

}
