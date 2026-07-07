using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : BaseAbility
{
    private string dashAnimationName = "Dash";
    private int dashAnimationID;
    [SerializeField] private InputActionReference dashInput;
    [SerializeField] private GhostEffect ghostEffect;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private int numberOfDash;
    private int currentDashAvaliable;
    private float baseGravity; 
    private float currentTime;
    private float dashProccessTime;

    void OnEnable()
    {
        dashInput.action.started += TryToDash;
    }

    void OnDisable()
    {
        dashInput.action.started -= TryToDash;
    }
    
    void TryToDash(InputAction.CallbackContext value)
    {
        if (!IsPermitted) return;
        
        if(currentDashAvaliable <= 0) return;
        
        if(linkedStateMachine.CurrentState == State.Dash) return;
        
        if(linkedPhysic.IsOnWall && player.FacingDirection == linkedGatherInput.HorizontalInput) player.ForceFlip();
        currentDashAvaliable--;
        linkedStateMachine.ChangeState(State.Dash);
        ghostEffect.PlayEffect(5,dashTime);
        linkedPhysic.DisableGravity();
        linkedPhysic.RB.linearVelocity = new Vector2(dashSpeed * player.FacingDirection, 0);
        currentTime = dashTime;
        dashProccessTime = 0.05f;
    }

    protected override void Start()
    {
        base.Start();
        baseGravity = linkedPhysic.RB.gravityScale;
        currentDashAvaliable = numberOfDash;
        dashAnimationID = Animator.StringToHash(dashAnimationName);
    }

    public override void ProcessAbility()
    {
        currentTime -= Time.deltaTime;
        dashProccessTime -= Time.deltaTime;
        if (linkedPhysic.IsOnWall && dashProccessTime <= 0)
        {
            if(linkedGatherInput.HorizontalInput == player.FacingDirection) linkedStateMachine.ChangeState(State.WallSlide);
            else linkedStateMachine.ChangeState(State.Jump);
            return;
        }
        
        if (currentTime <= 0)
        {
            if (linkedPhysic.Grounded)
            {
                if (linkedGatherInput.HorizontalInput != 0) linkedStateMachine.ChangeState(State.Run);
                else linkedStateMachine.ChangeState(State.Idle);
                return;
            }
            linkedStateMachine.ChangeState(State.Jump);
            return;
        }
    }

    public override void UpdateAnimation()
    {
        if (linkedPhysic.Grounded)
        {
            currentDashAvaliable = numberOfDash;
        }
        linkedAnimator.SetBool(dashAnimationID,linkedStateMachine.CurrentState == State.Dash);
    }

    public override void ExitAbility()
    {
        linkedPhysic.RB.linearVelocity = new Vector2(5*player.FacingDirection, 0);
        linkedPhysic.EnableGravity();
    }
    
}