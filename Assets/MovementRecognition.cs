using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;

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
    private List<Gesture> trainingSet = new List<Gesture>();


    // Start is called before the first frame update
    void Start()
    {
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        for (int i = 0; i < trainingSet.Count; i++)
        {
            Debug.Log(i + ": " + trainingSet[i].Name);
        }
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

        Point[] pointArray = new Point[positionList.Count];

        for (int i = 0; i < positionList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture= new Gesture(pointArray);

        //Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
        float x = PointCloudRecognizer.Classify2(newGesture, trainingSet.ToArray()[9]);

        Debug.Log(x);
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

    public static bool InsidePolygon(Vector2 target, Vector2[] poly)
    {
        Vector2 p1, p2;
        bool result = false;
        //return false if this is not an actual polygon
        if (poly.Length < 3) return result;

        Vector2 oldV = new Vector2(poly[poly.Length - 1].x, poly[poly.Length - 1].y);
        for (int i = 0; i < poly.Length; i++)
        {
            Vector2 newV = new Vector2(poly[i].x, poly[i].y);
            if (newV.x > oldV.x)
            {
                p1 = oldV;
                p2 = newV;
            }
            else
            {
                p1 = newV;
                p2 = oldV;
            }
            if ((newV.x < target.x) == (target.x <= oldV.x))
            {
                if ((target.y - p1.y) * (p2.x - p1.x) < (p2.y - p1.y) * (target.x - p1.x))
                {
                    result = !result;
                }
            }
            oldV = newV;
        }
        return result;
    }
}
