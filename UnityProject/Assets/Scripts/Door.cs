using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    //Also the distance for each panel to move
    private float panelHeight;
    private bool moving;
    public float timeToOpen = 1;
    private float amountOpened;
    private bool opening;

    private GameObject topPanel;
    private GameObject bottomPanel;

    void Start()
    {
        panelHeight = transform.GetChild(0).lossyScale.y;
        topPanel = transform.GetChild(0).gameObject;
        bottomPanel = transform.GetChild(1).gameObject;
        amountOpened = 0;
    }

    void Update()
    {
        if (moving == true)
        {
            Vector2 mov = new Vector2(0, panelHeight * Time.deltaTime / timeToOpen);
            if (opening == false)
                mov = -1 * mov;
            topPanel.transform.Translate(mov);
            bottomPanel.transform.Translate(mov * -1);
            if (opening == false)
            {
                amountOpened -= Time.deltaTime;
                if (amountOpened <= 0)
                    moving = false;
            }
            else
            {
                amountOpened += Time.deltaTime;
                if (amountOpened >= timeToOpen)
                    moving = false;
            }
        }
    }

    public override void Activate()
    {
        if (opening == true)
            return;
        opening = true;
        moving = true;
    }

    //True = open; False = close
    public override void Activate(bool state)
    {
        if (state == true)
        {
            opening = true;
            moving = true;
        }
        if (state == false)
        {
            opening = false;
            moving = true;
        }
    }
}