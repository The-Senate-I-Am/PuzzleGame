using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool collidedWithWall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void Init(Vector2 velocity)
    {
        collidedWithWall = false;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(velocity,ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            collidedWithWall = true;
        }
    }
}
