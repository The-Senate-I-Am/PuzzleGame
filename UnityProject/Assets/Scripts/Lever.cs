using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] SpringJoint2D spring1;
    [SerializeField] SpringJoint2D spring2;
    [SerializeField] Activatable objToActivate;
    [SerializeField] bool flipBeforeStart;

    //false = left, true = right
    private bool activated;
    private bool direction;
    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        direction = false;

        if (flipBeforeStart)
        {
            transform.Rotate(new Vector3(0,0,-60));
            spring1.enabled = false;
            spring2.enabled = true;
            direction = true;
        }
    }

// Update is called once per frame
    void Update()
    {

        //if leaning left and rotation makes it go right
        if (direction == false && transform.localEulerAngles.z > 180)
        {
            spring1.enabled = false;
            spring2.enabled = true;
            direction = true;
            if (flipBeforeStart)
                activated = false;
            else
                activated = true;
            objToActivate.Activate(activated, this.gameObject);
        }
        else if (direction == true && transform.localEulerAngles.z < 180)
        {
            spring1.enabled = true;
            spring2.enabled = false;
            direction = false;
            if (flipBeforeStart)
                activated = true;
            else
                activated = false;
            objToActivate.Activate(activated, this.gameObject);
        }
    }
}
