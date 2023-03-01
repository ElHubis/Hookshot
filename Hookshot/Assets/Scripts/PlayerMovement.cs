using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine; 

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 10f;
    public float SwingSpeed = 5f;
    public float JumpHeight = 17.5f;
    private float Horizontal;
    private BoxCollider2D Collider;
    private Rigidbody2D PlayerRB;
    public static bool FacingRight = true;
    [SerializeField] private float DistanceCollider = 0.1f;
    [SerializeField] private LayerMask GroundLayer;

    HookshotController HookshotController;
    [SerializeField] GameObject HookShot;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
        HookshotController = HookShot.GetComponent<HookshotController>();
        FacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HookshotController.IsConnected)
        {
            Flip();
        }
            

        if (Input.GetButton("Jump") && Grounded())
        {
            PlayerRB.velocity = new Vector2(0f, JumpHeight);
        }
    }

    private void FixedUpdate()
    {



        Horizontal = (Input.GetAxisRaw("Horizontal"));

        if (Grounded() && !HookshotController.IsConnected)
        {
            PlayerRB.velocity = new Vector2(Horizontal * Speed, PlayerRB.velocity.y);
        }

        if (!Grounded() && !HookshotController.IsConnected)
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

        Vector3 SwingMovement = new Vector2(Horizontal, 0);

        if (!Grounded() && HookshotController.IsConnected)
        {
            PlayerRB.AddForce(SwingMovement * SwingSpeed);
        }

        if (HookshotController.IsConnected )
        {
            if (Input.GetKey(KeyCode.W))
            {
                HookshotController.HookJoint.distance -= 0.1f;
            }

            if (Input.GetKey(KeyCode.S) && HookshotController.HookJoint.distance <= HookshotController.DistanceMax)
            {
                HookshotController.HookJoint.distance += 0.1f;
            }
        }
    }
    
    private bool Grounded()
    {
       return Physics2D.BoxCast(Collider.bounds.center, Collider.bounds.size, 0f, Vector2.down, DistanceCollider, GroundLayer);
    }

    private void Flip()
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (FacingRight && MousePosition.x <= transform.position.x || !FacingRight && MousePosition.x > transform.position.x)
        {
            FacingRight = !FacingRight;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
        }            
    }
}
