using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBoxPickup : MonoBehaviour
{
    private bool inRangeOfBox;
    private List<GameObject> boxesInRange;

    public bool holdingBox;

    // Start is called before the first frame update
    void Start()
    {
        boxesInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick(InputValue input)
    {
        //If this is mousedown, do nothing, the function only performs actions on mouseup
        if (input.Get<float>() == 1.0)
            return;

        if (holdingBox)
        {
            //Drop the box
            holdingBox = false;
        }
        else
        {
            if (inRangeOfBox == false || GetComponent<PlayerThrowing>().holdingBall == true)
            {
                //Pick up the box
                GetClosestBox();
            }
        }
    }

    //Gets the box closest to the player in boxesInRange, just in case more than one box is in range of the player
    private GameObject GetClosestBox()
    {
        GameObject closest = null;
        float dist = float.MaxValue;

        foreach (GameObject box in boxesInRange)
        {
            float newdist = Vector2.Distance(box.transform.position, transform.position);
            if (newdist < dist)
            { 
                closest = box;
                dist = newdist;
            }
        }

        return closest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            inRangeOfBox = true;
            boxesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            boxesInRange.Remove(collision.gameObject);
            if (boxesInRange.Count == 0)
                inRangeOfBox = false;
        }
    }
}
