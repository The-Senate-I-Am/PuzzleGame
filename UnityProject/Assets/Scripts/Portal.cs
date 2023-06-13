using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject connectedPortal;
    public Vector2 directionFacing;
    private float rotation;

    private List<GameObject> objsOnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        objsOnCooldown = new List<GameObject>();
        SetDirectionFacing();
    }

    private void SetDirectionFacing()
    {
        rotation = transform.eulerAngles.z;
        if (rotation < 0)
            rotation = rotation * -1;
        rotation = rotation % 360;
        if (rotation == 0)
        {
            directionFacing = new Vector2(1,0);
        }
        else if (rotation == 90)
        {
            directionFacing = new Vector2(0, 1);
        }
        else if (rotation == 180)
        {
            directionFacing = new Vector2(-1, 0);
        }
        else if (rotation == 270)
        {
            directionFacing = new Vector2(0, -1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") && collision.GetType() != typeof(BoxCollider2D)) 
            || collision.CompareTag("Box") || collision.CompareTag("Ball"))
        {
            GameObject obj = collision.gameObject;
            
            if (objsOnCooldown.Contains(obj) || connectedPortal.GetComponent<Portal>().objsOnCooldown.Contains(obj))
            {
                return;
            }

            Vector3 spawnPos = CalculateSpawnLocation(obj.transform.position);

            StartCoroutine(teleportCooldown(obj));

            obj.transform.position = spawnPos;
        }
    }

    IEnumerator teleportCooldown(GameObject needsCooldown)
    {
        objsOnCooldown.Add(needsCooldown);
        yield return new WaitForSeconds(0.1f);
        objsOnCooldown.Remove(needsCooldown);
    }

    private Vector2 CalculateSpawnLocation(Vector3 objLoc)
    {
        Vector2 relativeLoc = objLoc - transform.position;
        Vector2 connectedDir = connectedPortal.GetComponent<Portal>().directionFacing;

        if (directionFacing == connectedDir)
        {
            //do nothing
        }
        else if (directionFacing * -1 == connectedDir)
        {
            if (directionFacing.x != 0)
                relativeLoc = new Vector2(-1 * relativeLoc.x, relativeLoc.y);
            else
                relativeLoc = new Vector2(relativeLoc.x, -1 * relativeLoc.y);
        }
        else
        {
            relativeLoc = new Vector2(0, 0);
        }

        return relativeLoc + (Vector2) connectedPortal.transform.position;
    }

    private Vector2 CalculateResultingVelocity(Vector3 objVelocity)
    {
        Vector2 connectedDir = connectedPortal.GetComponent<Portal>().directionFacing;

        if (directionFacing * -1 == connectedDir)
        {
            //do nothing
        }
        else if (directionFacing == connectedDir)
        {
            if (directionFacing.x != 0)
                objVelocity = new Vector2(-1 * objVelocity.x, objVelocity.y);
            else
                objVelocity = new Vector2(objVelocity.x, -1 * objVelocity.y);
        }
        else
        {
            objVelocity = connectedDir * objVelocity.magnitude;
        }

        return objVelocity;
    }

    private bool ExitingPortal(GameObject obj)
    {
        Vector2 relativePos = obj.transform.position - transform.position;

        if (directionFacing.y == 0)
        {
            if ((relativePos.x / directionFacing.x) > 0)
            {
                return true;
            }
            return false;
        }
        else
        {
            if ((relativePos.y / directionFacing.y) > 0)
            {
                return true;
            }
            return false;
        }
    }

    //Fires a ray from the top and bottom of the portal in the portal's direction to make sure
    //that the object going into the portal is not clipping into the portal's edges.
    private bool CheckIfObjectCanTeleport(GameObject obj)
    {
        Vector2 rayStartPos1 = transform.position;
        Vector2 rayStartPos2 = transform.position;
        if (directionFacing.y == 0)
        {
            rayStartPos1 += new Vector2(0, 1);
            rayStartPos2 += new Vector2(0, -1);
        }
        if (directionFacing.x == 0)
        {
            rayStartPos1 += new Vector2(1, 0);
            rayStartPos2 += new Vector2(-1, 0);
        }

        RaycastHit2D[] hits1 = Physics2D.RaycastAll(rayStartPos1, directionFacing, 1f);
        RaycastHit2D[] hits2 = Physics2D.RaycastAll(rayStartPos2, directionFacing, 1f);

        foreach (RaycastHit2D hit in hits1)
        {
            if (hit.collider.gameObject == obj)
                return false;
        }
        foreach (RaycastHit2D hit in hits2)
        {
            if (hit.collider.gameObject == obj)
                return false;
        }

        return true;
    }
}
