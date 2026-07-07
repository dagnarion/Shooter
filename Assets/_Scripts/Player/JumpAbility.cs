using UnityEngine;
using UnityEngine.InputSystem;

public class JumpAbility : BaseAbility
{
    private string jumpAnimationName = "Jump";
    private string verticalVelocityAnimationName = "VerticalVelocity";
    private int jumpParameterID;
    private int verticalVelocityParameterID;
    [SerializeField] private InputActionReference JumpActionRef;
    [SerializeField] private int numberOfJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airMoveSpeed;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float fallGravity;
    private bool hasGroundJumped;
    private int airJumpsRemaining;
    [Header("Time")] [SerializeField] private float miniumAirTime;
    [SerializeField] private float jumpBufferTime;
    private float jumpBufferCurrentTime;

    private float baseGravity;
    private float onAirTime;
    private bool DoneJumpCut;

    private void OnEnable()
    {
        JumpActionRef.action.performed += TryToJump;
        JumpActionRef.action.canceled += StopJump;
    }

    private void OnDisable()
    {
        JumpActionRef.action.performed -= TryToJump;
        JumpActionRef.action.canceled -= StopJump;
    }

    protected override void Start()
    {
        base.Start();
        hasGroundJumped = false;
        airJumpsRemaining = numberOfJump - 1;
        jumpParameterID = Animator.StringToHash(jumpAnimationName);
        verticalVelocityParameterID = Animator.StringToHash(verticalVelocityAnimationName);
        baseGravity = linkedPhysic.RB.gravityScale;
    }


    public override void ProcessAbility()
    {
        onAirTime -= Time.deltaTime;
        if (linkedPhysic.RB.linearVelocityY <= 0 && linkedPhysic.IsOnWall &&
            (linkedGatherInput.HorizontalInput == player.FacingDirection))
        {
            linkedStateMachine.ChangeState(State.WallSlide);
            return;
        }

        if (jumpBufferCurrentTime + jumpBufferTime <= Time.time)
        {
            if (linkedPhysic.Grounded && onAirTime < 0)
            {
                hasGroundJumped = false;
                airJumpsRemaining = numberOfJump - 1;
                if (linkedGatherInput.HorizontalInput == 0) linkedStateMachine.ChangeState(State.Idle);
                else linkedStateMachine.ChangeState(State.Run);
                return;
            }
        }
        else
        {
            if (linkedPhysic.Grounded && onAirTime < 0)
            {
                linkedPhysic.RB.linearVelocity = new Vector2(airMoveSpeed * linkedGatherInput.HorizontalInput, jumpForce);
                onAirTime = miniumAirTime;
                hasGroundJumped = true;
                airJumpsRemaining = numberOfJump - 1;
                DoneJumpCut = false;
            }
        }
        player.Incline();
        player.Flip();
        if (linkedPhysic.RB.linearVelocityY <= 0) // chỗ này implement jump hang time
        {
            linkedPhysic.RB.gravityScale = fallGravity;
        }

        if (linkedGatherInput.HorizontalInput != 0)
            linkedPhysic.RB.linearVelocityX = airMoveSpeed * linkedGatherInput.HorizontalInput;
        linkedPhysic.RB.linearVelocityY = Mathf.Clamp(linkedPhysic.RB.linearVelocityY, -maxFallSpeed, 100);
    }

    public override void UpdateAnimation()
    {
        linkedAnimator.SetBool(jumpParameterID, linkedStateMachine.CurrentState == State.Jump);
        linkedAnimator.SetFloat(verticalVelocityParameterID, linkedPhysic.RB.linearVelocityY);
    }

    public override void ExitAbility()
    {
        linkedPhysic.RB.gravityScale = baseGravity;
    }

    public void TryToJump(InputAction.CallbackContext value)
    {
        if (!IsPermitted) return;
        if(linkedStateMachine.CurrentState == State.Dash) return;
        
        bool isCoyoteActive = linkedPhysic.IsStillOnGround;
        
        if (isCoyoteActive && !hasGroundJumped)
        {
            hasGroundJumped = true;
        }
        else if (airJumpsRemaining > 0)
        {
            airJumpsRemaining--;
        }
        else
        {
            jumpBufferCurrentTime = Time.time;
            return;
        }
        
        linkedPhysic.RB.gravityScale = baseGravity;
        linkedStateMachine.ChangeState(State.Jump);
        linkedPhysic.RB.linearVelocity = new Vector2(airMoveSpeed * linkedGatherInput.HorizontalInput, jumpForce);
        onAirTime = miniumAirTime;
        DoneJumpCut = false;
    }

    public void StopJump(InputAction.CallbackContext value)
    {
        if (DoneJumpCut) return;
        if (linkedStateMachine.CurrentState != State.Jump) return;

        if (linkedPhysic.RB.linearVelocityY > 0)
            linkedPhysic.RB.linearVelocity =
                new Vector2(linkedPhysic.RB.linearVelocityX, linkedPhysic.RB.linearVelocityY * 0.5f);
        DoneJumpCut = true;
    }
}