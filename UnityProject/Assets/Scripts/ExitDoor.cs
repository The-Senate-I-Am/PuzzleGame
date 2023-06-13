using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    private bool activatable;
    private GameObject player;
    private void Start()
    {
        activatable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activatable = true;
            if (player == null)
            {
                player = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            activatable = false;
    }

    void OnJump(InputValue input)
    {
        if (input.Get<float>() == 1.0f && activatable && player.GetComponent<PlayerThrowing>().holdingBall)
        {
            string name = SceneManager.GetActiveScene().name;
            int sceneNum = int.Parse(name[name.Length - 1].ToString());
            sceneNum++;
            Projection.simSceneMade = false;
            SceneManager.LoadScene("Level " + sceneNum);
        }
    }
}
