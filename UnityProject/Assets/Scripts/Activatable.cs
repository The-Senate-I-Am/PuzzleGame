using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : MonoBehaviour
{
    private List<GameObject> activators = new List<GameObject>();

    public abstract void Activate();
    public abstract void Activate(bool state);
    public void Activate(bool state, GameObject activatedBy)
    {
        if (state == true)
        {
            if (activators.Count == 0)
            {
                Activate(true);
            }
            activators.Add(activatedBy);
        }
        if (state == false)
        {
            activators.Remove(activatedBy);
            if (activators.Count == 0)
            {
                Activate(false);
            }
        }
    }
}
