using System;
using UnityEngine;

public class PhysicControl : MonoBehaviour
{
    [field: SerializeField] public Rigidbody2D RB { get; private set; }
    [Header("Ground")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float groundCoyoteTime;
    [SerializeField] private Transform leftGroundCheckPoint;
    [SerializeField] private Transform rightGroundCheckPoint;
    [SerializeField] private LayerMask whatIsTheGround;
    [field:SerializeField] public bool Grounded { get; private set; }
    [field:SerializeField] public bool IsStillOnGround { get; private set; }
    private RaycastHit2D leftGround;
    private RaycastHit2D rightGround;
    private float groundCoyoteCurrentTime;
    
    [Header("Wall")] 
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float wallCoyoteTime;
    private float wallCoyoteCurrentTime;
    [SerializeField] private Transform downWallCheckPoint;
    [SerializeField] private Transform upWallCheckPoint;
    private RaycastHit2D downWall;
    private RaycastHit2D upWall;
    [field:SerializeField] public bool IsOnWall { get; private set; }
    [field:SerializeField] public bool IsStillOnWall { get; private set; }
    private float gravityValue;

    [Header("Crouch")] 
    [SerializeField] private Collider2D standCollider;
    [SerializeField] private Collider2D crouchCollider;
    [SerializeField] private Transform leftCeilingCheckPoint;
    [SerializeField] private Transform rightCeilingCheckPoint;
    [SerializeField] private float ceilingCheckDistance;
    private RaycastHit2D leftCeiling;
    private RaycastHit2D rightCeiling;
    public bool CeilingDetected { get; private set; }

    private void Start()
    {
        gravityValue = RB.gravityScale;
    }


    public void CrouchCollider()
    {
        standCollider.enabled = false;
        crouchCollider.enabled = true;
    }

    public void StandCollider()
    {
        standCollider.enabled = true;
        crouchCollider.enabled = false;
    }

    public bool CheckCeiling()
    {
        leftCeiling = Physics2D.Raycast(leftCeilingCheckPoint.position, transform.up,ceilingCheckDistance,whatIsTheGround);
        rightCeiling = Physics2D.Raycast(rightCeilingCheckPoint.position, transform.up,ceilingCheckDistance,whatIsTheGround);
    
        Debug.DrawRay(leftCeilingCheckPoint.position,transform.up * ceilingCheckDistance,Color.red );
        Debug.DrawRay(rightCeilingCheckPoint.position,transform.up * ceilingCheckDistance, Color.red);
        
        if (leftCeiling || rightCeiling)
        {
            return true;
        }
        
        return false;
    }
    

    public bool CheckWall()
    {
        downWall = Physics2D.Raycast(downWallCheckPoint.position, transform.right,wallCheckDistance,whatIsTheGround);
        upWall = Physics2D.Raycast(upWallCheckPoint.position, transform.right,wallCheckDistance,whatIsTheGround);
    
        Debug.DrawRay(downWallCheckPoint.position,transform.right * wallCheckDistance,Color.red );
        Debug.DrawRay(upWallCheckPoint.position,transform.right * wallCheckDistance, Color.red);
        
        if (downWall && upWall)
        {
            return true;
        }
        
        return false;
    }

    public bool CheckGround()
    {
        leftGround = Physics2D.Raycast(leftGroundCheckPoint.position, Vector2.down, 
            groundCheckDistance,whatIsTheGround);
        rightGround = Physics2D.Raycast(rightGroundCheckPoint.position, Vector2.down,
            groundCheckDistance, whatIsTheGround);

        Debug.DrawRay(leftGroundCheckPoint.position,Vector3.down * groundCheckDistance);
        Debug.DrawRay(rightGroundCheckPoint.position,Vector3.down * groundCheckDistance);
        
        if (leftGround || rightGround) return true;
        return false;
    }

    private void Update()
    {
        Grounded = CheckGround();
        IsOnWall = CheckWall();
        CeilingDetected = CheckCeiling();
        CheckIsStillOnWall();
        CheckIsStillOnGround();
    }

    void CheckIsStillOnWall()
    {
        if (CheckWall())
        {
            IsStillOnWall = true;
            wallCoyoteCurrentTime = Time.time;
        }   
        if (wallCoyoteCurrentTime + wallCoyoteTime <= Time.time)
        {
            IsStillOnWall = false;
            return;
        }
    }   
    
    void CheckIsStillOnGround()
    {
        if (CheckGround())
        {
            IsStillOnGround = true;
            groundCoyoteCurrentTime = Time.time;
        }   
        
        if (groundCoyoteCurrentTime + groundCoyoteTime <= Time.time)
        {
            IsStillOnGround = false;
            return;
        }
    }

    public void DisableGravity()
    {
        RB.gravityScale = 0;
    }

    public void EnableGravity()
    {
        RB.gravityScale = gravityValue;
    }
}
