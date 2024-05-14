using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public Vector3 startPosition;

    public Quaternion startRotation;
    //bool isHolding = false;

    BoxCollider boxCollider;

    Rigidbody hammerRigidbody;

    public GameObject hammerReset;
    // Start is called before the first frame update
    public void Start()
    {
        hammerReset.SetActive(true);
        startPosition = transform.localPosition;
        startRotation = transform.rotation;

        boxCollider = transform.GetComponent<BoxCollider>();
        //boxCollider.isTrigger = true;
        hammerRigidbody = transform.GetComponent<Rigidbody>();
    }

   /* public void HoldHammer()
    {
        boxCollider.isTrigger = true;
        isHolding = true;
    }

    public void ThrowHammer()
    {
        isHolding = false;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BadSlime"))
        {
            GameObject slime = other.gameObject;
            GameController.instance.DestroySlime(slime);
        }
        else if (other.CompareTag("FriendlySlime"))
        {
            GameObject badslime = other.gameObject;
            GameController.instance.DestroyFriendlySlime(badslime);
        }
        else if (other.CompareTag("GoldenSlime"))
        {
            GameObject goldenslime = other.gameObject;
            GameController.instance.DestroyGoldenCharacter(goldenslime);
        }
        else if (other.CompareTag("Ground"))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        hammerRigidbody.velocity = Vector3.zero;
        hammerRigidbody.angularVelocity = Vector3.zero;
       // boxCollider.isTrigger = false;
        transform.localPosition = startPosition;
        transform.rotation = transform.rotation;
    }
}
