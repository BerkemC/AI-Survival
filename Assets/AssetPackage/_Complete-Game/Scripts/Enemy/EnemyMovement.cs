using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Reference to the player's position.
        PlayerHealth playerHealth;      // Reference to the player's health.
        EnemyHealth enemyHealth;        // Reference to this enemy's health.
        UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.
		Queue path = new Queue();

		GreedySearch gs; //Greedy search script reference
        void Start ()
        {
            // Set up the references.
            player = GameObject.FindGameObjectWithTag ("Player").transform;

			playerHealth = GameObject.FindObjectOfType <PlayerHealth> ();

            enemyHealth = GetComponent <EnemyHealth> ();

            nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();

			gs = GetComponent <GreedySearch> ();

        }


        void Update ()
        {
			
			path = gs.GetGreedyBestFirstSearchPath (transform.position, player.transform.position);
            // If the enemy and the player have health left...
           if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {
				if(path.queue.Count > 0){
					// ... set the destination of the nav mesh agent to the player.
					nav.SetDestination (path.Dequeue ());
				}
				else{
					nav.SetDestination (player.transform.position);
				}
                
            }
            // Otherwise...
           else
           {
                // ... disable the nav mesh agent.
                nav.enabled = false;
           }
        }
    }
}