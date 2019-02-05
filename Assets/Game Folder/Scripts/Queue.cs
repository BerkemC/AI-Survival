using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour {

	public List<Vector3> queue;
	public int head = 0;
	public int tail = -1;

	public Queue()
    {
		queue = new List<Vector3> ();
	}

	public void Enqueue(Vector3 node)
    {
		queue.Add (node);
		tail++;
	}

	public Vector3 Dequeue()
    {
		Vector3 temp = queue [head++];
		return temp;
	}

	public bool IsEmpty()
    {
		return (tail<head);
	}

}
