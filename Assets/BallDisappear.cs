using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDisappear : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            Destroy (collision.gameObject);

            
            //GameObject objectToDisappear = GameObject.FindWithTag("goal");
            //Destroy(objectToDisappear);
            Debug.Log("Destroy");
            //gameObject.SetActive(false);
        }
    }
}
