using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddGoal : MonoBehaviour
{
    public int counter=0;
    public TextMeshPro text;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            counter++;
            text.SetText("Trafione: " + counter.ToString());
        }
        Debug.Log(collision.gameObject.tag);
    }


}
