using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    private static Scene simulationScene;
    [HideInInspector]
    public static bool simSceneMade = false;
    private PhysicsScene2D physicsScene;

    [SerializeField] private Transform obstaclesParent;

    private void Start()
    {
        linePoints = new List<GameObject>();
        linePositions = new List<Vector2>();
        CreatePhysicsScene();
    }

    void CreatePhysicsScene()
    {
        if (simSceneMade == false)
        {
            simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
            simSceneMade = true;
        }

        physicsScene = simulationScene.GetPhysicsScene2D();

        foreach (Transform obj in obstaclesParent)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.transform.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        }
    }

    [SerializeField] private int maxPhysicsFrameIterations = 100;
    [SerializeField] private float physicsFrameSize = 1;
    private Ball ghostObj;

    public void SimulateTrajectory(Ball ballPrefab, Vector2 pos, Vector2 velocity)
    {
        //Instantiates ghostObj if it has not been yet.
        if (ghostObj == null)
        {
            ghostObj = Instantiate(ballPrefab, pos, Quaternion.identity);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj.gameObject, simulationScene);
        }

        //Resets ghostObj to prepare it for another simulation
        ghostObj.transform.position = pos;
        ghostObj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        //Initializes ghostObj with the initial conditions
        ghostObj.Init(velocity);

        //Simulates ghostObj's trajectory and records the points, putting it into an array to be sent
        //through renderPoints()
        linePositions.Clear();
        for (int i = 0; i < maxPhysicsFrameIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime * physicsFrameSize);
            linePositions.Add(ghostObj.transform.position);

            if (ghostObj.collidedWithWall == true)
            {
                linePositions.RemoveAt(linePositions.Count - 1);
                linePositions.RemoveAt(linePositions.Count - 1);
                break;
            }
        }
        renderPoints();
    }

    private List<Vector2> linePositions;
    private List<GameObject> linePoints;
    [SerializeField] private int distBetweenPoints;
    [SerializeField] private GameObject trajectoryLinePoint;

    private void renderPoints()
    {
        //Adds the points to the line if they were not already there and moves them if they were
        int currentLinePoint = 0;
        for (int i = 0; i < linePositions.Count; i += distBetweenPoints)
        {
            if (currentLinePoint < linePoints.Count && linePoints[currentLinePoint] != null) {
                linePoints[currentLinePoint].transform.position = linePositions[i];
            }
            else if (currentLinePoint >= linePoints.Count)
            {
                linePoints.Add(Instantiate(trajectoryLinePoint, linePositions[i], Quaternion.identity));
            }
            currentLinePoint++;
        }


        //Destroys the points and removes them when they get out of range
        for (int i = currentLinePoint; i < linePoints.Count; i++)
        {
            Destroy(linePoints[i]);
        }
        while (currentLinePoint < linePoints.Count && linePoints[currentLinePoint] == null)
        {
            linePoints.RemoveAt(currentLinePoint);
        }

    }

    public void ToggleLine(bool set)
    {
        //line.enabled = set;
        for (int i = 0; i < linePoints.Count; i++)
        {
            if (linePoints[i] != null)
            {
                linePoints[i].SetActive(set);
            }
        }
    }
}
