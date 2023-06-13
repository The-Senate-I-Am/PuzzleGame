using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowing : MonoBehaviour
{
    public Vector2 throwStartRelative = new Vector2(0, 1);
    private bool mouseDown;
    public GameObject ball;

    public float throwStrength;
    public float throwMaxStrength;


    private Vector2 startLoc;
    private Vector2 mouseWorldPos;
    private Vector2 force;

    public bool holdingBall;
    private bool inRangeOfBall;
    private GameObject ballInstance;

    private void calcBallThrow()
    {
        startLoc = (Vector2)transform.position + throwStartRelative;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        force = (mouseWorldPos - startLoc) * throwStrength;
        if (force.magnitude > throwMaxStrength)
        {
            force = force.normalized * throwMaxStrength;
        }
    }

    private void OnClick(InputValue input)
    {
        mouseDown = (input.Get<float>() == 1.0f);
        if (holdingBall)
        {
            calcBallThrow();
            if (mouseDown == false)
            {
                ballInstance = Instantiate(ball, startLoc, Quaternion.identity);
                ballInstance.GetComponent<Ball>().Init(force);
                projection.ToggleLine(false);
                holdingBall = false;
            }
            else
            {
                projection.ToggleLine(true);
            }
        }
        else
        {
            if (mouseDown == false && inRangeOfBall == true)
            {
                Destroy(ballInstance);
                holdingBall = true;
                inRangeOfBall = false;
            }
        }
    }

    [SerializeField] private Projection projection;

    private void Update()
    {
        if (mouseDown && holdingBall)
        {
            calcBallThrow();
            projection.SimulateTrajectory(ball.GetComponent<Ball>(), startLoc, force);
        }
    }
    private void Start()
    {
        holdingBall = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.CompareTag("Ball"))
        {
            inRangeOfBall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.CompareTag("Ball"))
        {
            inRangeOfBall = false;
        }
    }
}
