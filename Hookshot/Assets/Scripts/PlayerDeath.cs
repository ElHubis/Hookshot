using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private Rigidbody2D PlayerRB;
    // Start is called before the first frame update

    HookshotController HookshotController;
    [SerializeField] GameObject HookShot;
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        HookshotController = HookShot.GetComponent<HookshotController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Death();
            HookshotController.StopGrapple();
        }
    }

    private void Death()
    {
        PlayerRB.position = CheckpointSystem.CurrentCheckpoint;
    }
}
