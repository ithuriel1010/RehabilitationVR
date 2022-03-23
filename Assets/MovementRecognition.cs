using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementRecognition : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputThreshold = 0.1f;
    public Transform movementSource;
    public float newPositionThresholdDistance = 0.05f;
    public GameObject debugCubePrefab;

    private bool isMoving = false;
    private List<Vector3> positionList = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold);

        //start moving
        if(!isMoving && isPressed)
        {
            StartMovement();
        }
        //end moving
        else if(isMoving && !isPressed)
        {
            EndMovement();
        }
        //continous movement
        else if(isMoving && isPressed)
        {
            UpdateMovement();
        }
    }

    public void StartMovement()
    {
        Debug.Log("Start");
        isMoving = true;
        positionList.Clear();
        positionList.Add(movementSource.position);
        Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
    }

    public void EndMovement()
    {
        Debug.Log("End");
        isMoving = false;
    }

    public void UpdateMovement()
    {
        Debug.Log("Update");
        Vector3 lastPosition = positionList[positionList.Count - 1];

        if(Vector3.Distance(movementSource.position, lastPosition)>newPositionThresholdDistance)
        {
            positionList.Add(movementSource.position);
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
        }

    }
}
