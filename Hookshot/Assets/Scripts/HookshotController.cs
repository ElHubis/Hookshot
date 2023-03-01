using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookshotController : MonoBehaviour
{
    public Vector2 Direction;
    private Vector2 ConnectedPoint;
    public DistanceJoint2D HookJoint;
    private LineRenderer RopeRender;
    public Transform FirePoint;

    public bool IsConnected;
    [HideInInspector]public float DistanceMax = 12f;
    [SerializeField] private LayerMask GroundLayer;

    // Start is called before the first frame update
    void Start()
    {
        RopeRender = GetComponent<LineRenderer>();

        HookJoint.enabled = false;
        HookJoint.distance = DistanceMax;
        RopeRender.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Rotator();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartGrapple();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopGrapple();
        }

    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void Rotator()
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = MousePosition - transform.position;

        if (PlayerMovement.FacingRight == true)
        {
            float angle = Vector2.SignedAngle(Vector2.right, Direction);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        if (PlayerMovement.FacingRight == false)
        {
            float angle = Vector2.SignedAngle(Vector2.left, Direction);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void StartGrapple()
    {
        Vector2 GrappleDistance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - FirePoint.position;
        RaycastHit2D hit = Physics2D.Raycast(FirePoint.position, GrappleDistance, DistanceMax, GroundLayer);

        ConnectedPoint = hit.point;

        if(hit.collider != null)
        {
            HookJoint.connectedAnchor = hit.point;
            HookJoint.enabled = true;
            IsConnected = true;
        }
    }

    public void StopGrapple()
    {
        HookJoint.enabled = false;
        IsConnected = false;
        RopeRender.positionCount = 0;
        RopeRender.enabled = false;
    }

    private void DrawRope()
    {
        if(!IsConnected) return;

        RopeRender.positionCount = 100;
        RopeRender.SetPosition(0, FirePoint.position);
        RopeRender.SetPosition(1, ConnectedPoint);
        RopeRender.enabled = true;
    }

}
