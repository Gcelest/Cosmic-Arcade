using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeeBallHole : MonoBehaviour
{
    //private HashSet<string> triggeredHoles = new HashSet<string>(); // Keep track of triggered holes
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a ball
        if (other.CompareTag("Ball"))
        {
            {
                Debug.Log("Ball enter trigger.");
                switch (gameObject.name)
                {
                    // Check which hole trigger the ball entered and update the score accordingly
                    case "Hole1":
                        GameControllerSolarSkeeBall.instance.UpdateScore(100);
                        Debug.Log("Added point for 1");
                        break;
                    case "Hole2":
                        GameControllerSolarSkeeBall.instance.UpdateScore(100);
                        Debug.Log("Added point for 2");
                        break;
                    case "Hole3":
                        GameControllerSolarSkeeBall.instance.UpdateScore(50);
                        Debug.Log("Added point for 3");
                        break;
                    case "Hole4":
                        GameControllerSolarSkeeBall.instance.UpdateScore(40);
                        Debug.Log("Added point for 4");
                        break;
                    case "Hole5":
                        GameControllerSolarSkeeBall.instance.UpdateScore(30);
                        Debug.Log("Added point for 5");
                        break;
                    case "Hole6":
                        GameControllerSolarSkeeBall.instance.UpdateScore(20);
                        Debug.Log("Added point for 6");
                        break;
                    case "Hole7":
                        GameControllerSolarSkeeBall.instance.UpdateScore(10);
                        Debug.Log("Added point for 7");
                        break;
                    case "Guther":
                        GameControllerSolarSkeeBall.instance.UpdateScore(0);
                        Debug.Log("Added point for 8");
                        break;
                    case "OutofBounce1":
                        GameControllerSolarSkeeBall.instance.UpdateScore(0);
                        Debug.Log("Added point for 8");
                        break;
                    case "OutofBounce2":
                        GameControllerSolarSkeeBall.instance.UpdateScore(0);
                        Debug.Log("Added point for 8");
                        break;
                    case "OutofBounce3":
                        GameControllerSolarSkeeBall.instance.UpdateScore(0);
                        Debug.Log("Added point for 8");
                        break;
                    case "OutofBounce4":
                        GameControllerSolarSkeeBall.instance.UpdateScore(0);
                        Debug.Log("Added point for 8");
                        break;
                    case "OutofBounce5":
                        GameControllerSolarSkeeBall.instance.UpdateScore(0);
                        Debug.Log("Added point for 8");
                        break;
                    default://OutofBounce
                        Debug.LogWarning("Unknown hole trigger tag: " + gameObject.tag);
                        break;
                }
                // Destroy the ball
                Destroy(other.gameObject);
                GameControllerSolarSkeeBall.instance.BallUsed(); // Update balls left

                //triggeredHoles.Add(gameObject.name);
            }
        }
    }
        // Reset triggered holes HashSet (e.g., when a new ball is spawned or when the game is reset)

        /*private void OnTriggerExit(Collider other)
        {
            // Check if the collider belongs to a ball
            if (other.CompareTag("Ball"))
            {
                // Remove the hole from the triggered holes HashSet
                triggeredHoles.Remove(gameObject.name);
            }
        }

        public void ResetTriggeredHoles()
        {
            triggeredHoles.Clear();
        }*/
}