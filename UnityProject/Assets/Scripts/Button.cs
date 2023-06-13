using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Activatable objToActivate;
    [SerializeField] private bool needsToBeHeldDown = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Button") && collision.gameObject != this.gameObject)
        {
            if (needsToBeHeldDown)
            {
                objToActivate.Activate(true, this.gameObject);
            }
            else
            {
                objToActivate.Activate();
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Button") && collision.gameObject != this.gameObject)
        {
            if (needsToBeHeldDown)
            {
                objToActivate.Activate(false, this.gameObject);
            }
        }
    }
}
