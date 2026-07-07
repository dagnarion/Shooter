using UnityEngine;
using UnityEngine.InputSystem;
public class WallJumpAbility : BaseAbility
{
    private string wallJumpAnimationName = "WallJump";
    private int wallJumpParameterID;
    private int verticalVelocityParameterID;
    [SerializeField] private InputActionReference JumpActionRef;
    [SerializeField] private Vector2 jumpForce;
    [SerializeField] private float maxTimeOnWallJump;
    [SerializeField] private float jumpCutMultiphy;
    private float currentTime;
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
        wallJumpParameterID = Animator.StringToHash(wallJumpAnimationName);
    }

    public override void ProcessAbility()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            linkedStateMachine.ChangeState(State.Jump);
            return;
        }
    }

    public override void UpdateAnimation()
    {
        linkedAnimator.SetBool(wallJumpParameterID, linkedStateMachine.CurrentState == State.WallJump);
    }


    public void TryToJump(InputAction.CallbackContext value)
    {
        if (!IsPermitted) return;
        if(linkedPhysic.Grounded) return;
        if (!linkedPhysic.IsStillOnWall) return;
        linkedStateMachine.ChangeState(State.WallJump);
        
        currentTime = maxTimeOnWallJump;
        if (linkedPhysic.IsOnWall)
        {
            linkedPhysic.RB.linearVelocity = new Vector2(-player.FacingDirection*jumpForce.x,player.transform.up.y*jumpForce.y);
            player.ForceFlip();
        } 
        else linkedPhysic.RB.linearVelocity = new Vector2(player.FacingDirection*jumpForce.x,player.transform.up.y*jumpForce.y); 

    }

    public void StopJump(InputAction.CallbackContext value)
    {
        if (!IsPermitted) return;
        if(linkedPhysic.Grounded || linkedStateMachine.CurrentState != State.WallJump) return;
        linkedPhysic.RB.linearVelocity = new Vector2(linkedPhysic.RB.linearVelocityX*jumpCutMultiphy,linkedPhysic.RB.linearVelocityY * jumpCutMultiphy);
        linkedStateMachine.ChangeState(State.Jump);
    }
}
