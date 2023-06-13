using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : Activatable
{
    private List<GameObject> objsToBlow;
    public float fanForce;
    public float maxVelocity;
    [SerializeField] bool startDeactivated;
    [SerializeField] bool activated;


    private void Start()
    {
        activated = true;
        objsToBlow = new List<GameObject>();
        if (startDeactivated)
        {
            activated = false;
        }
    }

    private void Update()
    {
        if (activated == false)
            return;

        foreach (GameObject g in objsToBlow)
        {
            if (g.GetComponent<Rigidbody2D>().velocity.y <= maxVelocity)
                g.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, fanForce));
        }
    }
    
    public override void Activate()
    {
        activated = true;
    }

    public override void Activate(bool state)
    {
        activated = state;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ball"))
        {
            if (other.GetComponent<Rigidbody2D>() == null)
                return;
            objsToBlow.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ball"))
        {
            if (other.GetComponent<Rigidbody2D>() == null)
                return;
            objsToBlow.Remove(other.gameObject);
        }
    }
}
