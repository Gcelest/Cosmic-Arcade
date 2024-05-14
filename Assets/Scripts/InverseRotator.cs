using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 2) * Time.deltaTime);

    }

}
