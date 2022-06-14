using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NegativePoints : MonoBehaviour
{
    public int counter = 0;
    public TextMeshPro text_neg;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            counter++;
            text_neg.SetText("Nietrafione: " + counter.ToString());
        }
        Debug.Log(collision.gameObject.tag);
    }
}
