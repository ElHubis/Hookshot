using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine; 

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 10f;
    public float JumpHeight = 17.5f;
    private BoxCollider2D Collider;
    private Rigidbody2D PlayerRB;
    public static bool FacingRight = true;
    [SerializeField] private float DistanceCollider = 0.1f;
    [SerializeField] private LayerMask GroundLayer; 

    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
        FacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        Flip();

        if (Input.GetButton("Jump") && Grounded())
        {
            PlayerRB.velocity = new Vector2(0f, JumpHeight);
        }
    }

    private void FixedUpdate()
    {
        if (HookshotController.IsConnected == false && Grounded())
        {
            PlayerRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * Speed, PlayerRB.velocity.y);
        }

        if (!Grounded() && HookshotController.IsConnected == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                PlayerRB.velocity = new Vector2(-1 * Speed, PlayerRB.velocity.y);
            }

            if (Input.GetKey(KeyCode.D))
            {
                PlayerRB.velocity = new Vector2(1 * Speed, PlayerRB.velocity.y);
            }
        }
    }

    private bool Grounded()
    {
       return Physics2D.BoxCast(Collider.bounds.center, Collider.bounds.size, 0f, Vector2.down, DistanceCollider, GroundLayer);
    }

    private void Flip()
    {
        if (FacingRight && HookshotController.Direction.x <= 0f || !FacingRight && HookshotController.Direction.x > 0f)
        {
            FacingRight = !FacingRight;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
        }            
    }
}
