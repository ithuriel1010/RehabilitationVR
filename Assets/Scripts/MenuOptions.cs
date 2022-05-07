using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene()
    {
        Debug.Log("GUZIK!!!!");
        SceneManager.LoadScene(1);
    }
}
