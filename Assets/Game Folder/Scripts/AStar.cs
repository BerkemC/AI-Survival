using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

	private Vector3[,] map ;
	BFSMesh BFSCredentials; 

	//Visited list to prevent loops
	bool[,] visitedList = new bool[200,200];

	// Use this for initialization
	void Start () {

		BFSCredentials = GameObject.FindObjectOfType<BFSMesh> ();

		//Get the matrix created by bfs
		if(BFSCredentials.isGenerated)
        {
			map = BFSCredentials.matrix;
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
	/// Gets A star search path.
	/// </summary>
	/// <returns>The A star search path.</returns>
	/// <param name="currentPosition">Current position.</param>
	/// <param name="targetPosition">Target position.</param>
	public Queue GetAStarSearchPath(Vector3 currentPosition , Vector3 targetPosition)
	{
		
		visitedList = new bool[200,200];

		//Round down the current position
		currentPosition = new Vector3 ((float)((int)currentPosition.x), 1f, (float)((int)currentPosition.z));
		//Round down the target position
		targetPosition = new Vector3 ((float)((int)targetPosition.x),1f, (float)((int)targetPosition.z));

		//Result List
		Queue ResultingPath = new Queue ();
		PriorityQueue res = new PriorityQueue ();
		//Cost Vector
		int[] costSoFar = new int[100000];

		//Path Vector
		Vector3[] pathSoFar = new Vector3[100000];

		//Current position coordinates
		int currentX,currentZ;
		ConvertToIndex (out currentZ, out currentX, currentPosition);

		//Initial cost is 0
		costSoFar [0] = 0;

		//Frontier queue
		PriorityQueue frontier = new PriorityQueue ();
		//Create node for start point
		PriorityQueue.LocationNode currentNode = new PriorityQueue.LocationNode ();
		//Give its credentials
		currentNode.location = currentPosition;
		currentNode.priority = 0f;
		res.Enqueue (currentNode);
		//Add it to queue
		frontier.Enqueue (currentNode);


		//To keep track of the last place to be modified
		int LastIndex = 1;




		while(!frontier.IsEmpty ())
		{
			

			//Get the current location
			PriorityQueue.LocationNode currentN = frontier.DequeueNode();
			Vector3 currentLocation = currentN.location;

			if (currentLocation.Equals (targetPosition)) //Goal Achieved
			{
				res.Enqueue (currentN);
				ResultingPath = FillResultingQueue (res);
				ResultingPath.Enqueue (currentLocation);

				return ResultingPath;
			}
			else
			{
				//For each valid neighbor
				GetValidNeighbors (currentLocation).queue.ForEach (delegate(Vector3 obj) 
                {

					int neighborCost = costSoFar[LastIndex -1]+1;  //As all the steps cost 1 

					if(costSoFar[LastIndex] == 0 || costSoFar[LastIndex] > neighborCost)
					{
						PriorityQueue.LocationNode newNeighbor = new PriorityQueue.LocationNode();
						newNeighbor.cameFrom = currentLocation;
						newNeighbor.location = obj;
						newNeighbor.priority = costSoFar[LastIndex -1] +1 + (int) HeuristicFunction (obj,targetPosition);

						frontier.Enqueue (newNeighbor);
						res.Enqueue (newNeighbor);

					}


				});
			}


			LastIndex++;


		}

		ResultingPath = FillResultingQueue (res);

		return ResultingPath;
	}
		



	private Queue FillResultingQueue(Vector3[] pathSoFar)
	{
		Queue temp = new Queue ();
		int i = 1;

		while(!pathSoFar[i].Equals (Vector3.zero))
		{
			
			temp.Enqueue (pathSoFar[i++]);
		}

		return temp;
	}


	private Queue FillResultingQueue(PriorityQueue res)
	{
		Queue temp = new Queue ();
		List<Vector3> result = new List<Vector3> ();
		
		PriorityQueue.LocationNode tempNode = new PriorityQueue.LocationNode();
		int index = res.priorityQueue.Count - 1;

		if(index >= 0)
		{
			tempNode = res.priorityQueue [index];

			result.Add (tempNode.location);
	

			index--;



			Vector3 cFrom = tempNode.cameFrom;
		
			while(index>=0)
			{	
				
				if(res.priorityQueue[index].location.Equals (cFrom))
				{
					
					result.Add (res.priorityQueue[index].location);
					cFrom = res.priorityQueue [index].cameFrom;
				}
				index--;
			}


			while(result.Count>=1)
			{
				temp.Enqueue (result[result.Count-1]);
				result.RemoveAt (result.Count-1);
			}
		}
			

		return temp;
	}

	/// <summary>
	/// Gets the valid neighbors.
	/// </summary>
	/// <returns>The valid neighbors.</returns>
	/// <param name="currentPosition">Current position.</param>
	private Queue GetValidNeighbors(Vector3 currentPosition){


		Queue result = new Queue ();
		int x, z;
		ConvertToIndex (out z,out x,currentPosition);


		for(int i = -1; i < 2 ;i++)
        {
			for(int j = -1 ; j < 2 ; j++)
            {

				if((x+j) < 98 && (x+j) > 0 && (z+ i) < 49 && (z+i) > 0)
                {//Within the map boundaries
					if(!map [(z + i), (x + j)].Equals (Vector3.zero) && !visitedList[(z+i),(x+j)])
                    { //If the node is valid
						result.Enqueue (map [(z + i), (x + j)]);
						visitedList [(z + i), (x + j)] = true;
					}
				}
			}
		}

		
		return result;
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
