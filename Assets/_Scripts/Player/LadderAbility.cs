using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderAbility : BaseAbility
{
    private string ladderAnimationName = "Ladder";
    private int ladderParameterID;
    [SerializeField] private InputActionReference ladderAction;
    [SerializeField] private Vector2 climbSpeed;
    public bool CanClimb;

    protected override void Start()
    {
        base.Start();
        ladderParameterID = Animator.StringToHash(ladderAnimationName);
    }

    private void OnEnable()
    {
        ladderAction.action.started += StartClimb;
        ladderAction.action.canceled += StopClimb;
    }

    private void OnDisable()
    {
        ladderAction.action.started -= StartClimb;
        ladderAction.action.canceled -= StopClimb;
    }

    public override void ProcessAbility()
    {
        if (linkedPhysic.Grounded && linkedGatherInput.VerticalInput <1)
        {
            if(linkedGatherInput.HorizontalInput == 0)  linkedStateMachine.ChangeState(State.Idle);
            else linkedStateMachine.ChangeState(State.Run);
            return;
        }
        
        if (!CanClimb)
        {
            linkedStateMachine.ChangeState(State.Jump);
            return;
        }

        if (linkedGatherInput.VerticalInput == 0)
        {
            linkedAnimator.enabled = false;
        }
        linkedPhysic.RB.linearVelocity = new Vector2(0,climbSpeed.y * linkedGatherInput.VerticalInput);
    }

    public override void UpdateAnimation()
    {
        linkedAnimator.SetBool(ladderParameterID,linkedStateMachine.CurrentState == State.Ladders);
    }

    public override void ExitAbility()
    {
        linkedPhysic.EnableGravity();
        linkedAnimator.enabled = true;
    }


    private void StartClimb(InputAction.CallbackContext value)
    {
        linkedAnimator.enabled = true;
        if(!IsPermitted||!CanClimb) return;
        linkedStateMachine.ChangeState(State.Ladders);
        linkedPhysic.DisableGravity();
        linkedPhysic.RB.linearVelocity = Vector2.zero;
    }
    
    private void StopClimb(InputAction.CallbackContext value)
    {
        if(!IsPermitted || linkedStateMachine.CurrentState != State.Ladders) return;
        linkedPhysic.RB.linearVelocity = Vector2.zero;
    }
    
    
}
