using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManagement : MonoBehaviour {

	List<Vector3> validNeighbors = new List<Vector3>();
	Vector3 cLoc;
	bool isValid = true;
	bool isColliding;

	public List<Vector3> GetNeighbors(Vector3 currentLocation)
    {
		
		CheckPositiveX (currentLocation);

		CheckPositiveZ (currentLocation);

		CheckNegativeX (currentLocation);

		CheckNegativeZ (currentLocation);

		List<Vector3> tempList = new List<Vector3>();


		validNeighbors.ForEach (delegate(Vector3 obj) 
        {
			tempList.Add (obj);

		});

		validNeighbors.Clear ();

		return tempList;
	}

	public void CheckPositiveX(Vector3 location)
    {
		
		cLoc = new Vector3 ((location.x + transform.localScale.x), 1f, location.z);
		transform.position = cLoc;

		if (cLoc.x < (GameObject.FindObjectOfType<BFSMesh> ().xAxis / 4f)-1f) {
			
			validNeighbors.Add (cLoc);
			
		}

	}

	public void CheckPositiveZ(Vector3 location)
    {

		cLoc = new Vector3 (location.x, 1f, (location.z + transform.localScale.z));
		transform.position = cLoc;

		if (cLoc.z < (GameObject.FindObjectOfType<BFSMesh> ().zAxis / 2f)-1f)
        {
			validNeighbors.Add (cLoc);
		}

	}
	public void CheckNegativeX(Vector3 location)
    {

		cLoc = new Vector3 ((location.x - transform.localScale.x), 1f, location.z);
		transform.position = cLoc;

		if(cLoc.x > (GameObject.FindObjectOfType<BFSMesh>().xAxis*-3f/4f)+1f)
        {

			validNeighbors.Add (cLoc);	
		}

	}

	public void CheckNegativeZ(Vector3 location)
    {

		cLoc = new Vector3 (location.x, 1f, (location.z - transform.localScale.z));
		transform.position = cLoc;

		if(cLoc.z > (GameObject.FindObjectOfType<BFSMesh>().zAxis/-2f)+1f)
        {
			validNeighbors.Add (cLoc);	
		}

	}
		
}
