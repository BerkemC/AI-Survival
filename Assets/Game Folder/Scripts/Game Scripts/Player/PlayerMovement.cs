using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using System.Collections;

namespace CompleteProject
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 6f;            // The speed that the player will move at.


        Vector3 movement;                   // The vector to store the direction of the player's movement.
        Animator anim;                      // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.

		Queue path = new Queue();
		Vector3 nextLocation;
		public Vector3 norm;
		float timer = 1f;
		public BoxCollider col;
        void Awake ()
        {
        // Set up references.
            anim = GetComponent <Animator> ();
            playerRigidbody = GetComponent <Rigidbody> ();



        }
		void Start(){

			GreedySearch gs = GetComponent<GreedySearch> ();
			path = gs.GetGreedyBestFirstSearchPath (transform.position, new Vector3 (2f, 1f, 5f));
			nextLocation = path.Dequeue ();
			norm = new Vector3(-transform.position.x+nextLocation.z,0f,-transform.position.z+nextLocation.z).normalized;
			col.transform.position = nextLocation;
		}

		public void ChangeTargetNode ()
		{
			if (!path.isEmpty ()) {
				//Vector3 myPosition = new Vector3 ((float)((int)transform.position.x), (float)((int)transform.position.y), (float)((int)transform.position.z));
				//if (nextLocation.Equals (myPosition)) {
					nextLocation = path.Dequeue ();

				if (nextLocation.x > transform.position.x)
					norm.x = 1f;
				else if (nextLocation.x.Equals (transform.position.x))
					norm.x = 0f;
				else
					norm.x = -1f;

				if (nextLocation.z > transform.position.z)
					norm.z = 1;
				else if (nextLocation.z.Equals (transform.position.z))
					norm.z = 0f;
				else
					norm.z = -1;

					col.transform.position = nextLocation;

				//}
			}else{
				norm = Vector3.zero;
			}
		}
	
        void FixedUpdate ()
        {
			
		
            // Store the input axes.
			float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");


			/*while(!path.isEmpty ()){
				Instantiate (GameObject.FindObjectOfType<BFSMesh> ().spawn,path.Dequeue (),Quaternion.identity);
			}*/



			if(!path.isEmpty ()){
				if (nextLocation.x > transform.position.x)
					norm.x = 1f;
				else if (nextLocation.x -.5f < transform.position.x  && nextLocation.x +.5f > transform.position.x  )
					norm.x = 0f;
				else
					norm.x = -1f;

				if (nextLocation.z > transform.position.z)
					norm.z = 1;
				else if (nextLocation.z -.25f < transform.position.z  && nextLocation.z +.25f > transform.position.z )
					norm.z = 0f;
				else
					norm.z = -1;
			}else {
				norm = Vector3.zero;
			}

			


			Move (norm.x,norm.z);

            // Move the player around the scene.
           

            // Turn the player to face the mouse cursor.
			Turning (new Vector3(norm.x,0f,norm.z));

            // Animate the player.
			Animating (norm.x, norm.z);
			timer -= 1f * Time.deltaTime;
			/*if(timer < 0f){
				timer = 1f;
				ChangeTargetNode ();
			}*/




        }


        void Move (float h, float v)
        {
            // Set the movement vector based on the axis input.
            movement.Set (h, 0f, v);
            
            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * speed * Time.deltaTime;

            // Move the player to it's current position plus the movement.
            playerRigidbody.MovePosition (transform.position + movement);
        }


        void Turning (Vector3 toLookLocation)
        {


            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotatation = Quaternion.LookRotation (toLookLocation );

             // Set the player's rotation to this new rotation.
             playerRigidbody.MoveRotation (newRotatation);

        }


        void Animating (float h, float v)
        {
            // Create a boolean that is true if either of the input axes is non-zero.
            bool walking = h != 0f || v != 0f;

            // Tell the animator whether or not the player is walking.
            anim.SetBool ("IsWalking", walking);
        }
    }
}