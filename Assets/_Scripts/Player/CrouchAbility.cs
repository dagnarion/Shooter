using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class CrouchAbility : BaseAbility
{
    [SerializeField] private InputActionReference crouchActionRef;
    [SerializeField] private float crouchSpeed;
    private string crouchAnimationName = "Crouch";
    private int crouchParameterID;
    private bool HadReleaseCrouchButton;
    public void OnEnable()
    {
        crouchActionRef.action.performed += TryToCrouch;
        crouchActionRef.action.canceled += StopCrouch;
    }

    private void OnDisable()
    {
        crouchActionRef.action.performed -= TryToCrouch;
        crouchActionRef.action.canceled -= StopCrouch;
    }

    protected override void Start()
    {
        base.Start();
        crouchParameterID = Animator.StringToHash(crouchAnimationName);
    }

    public override void EnterAbility()
    {
        base.EnterAbility();
        linkedPhysic.RB.linearVelocity = Vector2.zero; 
        linkedPhysic.CrouchCollider();
        player.ResetInclineToBase();
    }
    
    public override void ExitAbility()
    {
        base.ExitAbility();
        linkedPhysic.StandCollider();
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        if (!linkedPhysic.Grounded)
        {
            linkedStateMachine.ChangeState(State.Jump);
            return;
        }

        if (HadReleaseCrouchButton && !linkedPhysic.CeilingDetected)
        {
            if(linkedGatherInput.HorizontalInput == 0)  linkedStateMachine.ChangeState(State.Idle);
            else linkedStateMachine.ChangeState(State.Run);
            return;
        }
        
        player.Flip();
        linkedPhysic.RB.linearVelocity = new Vector2(linkedGatherInput.HorizontalInput * crouchSpeed, linkedPhysic.RB.linearVelocityY);
    }

    public override void UpdateAnimation()
    {
        base.UpdateAnimation();
        linkedAnimator.SetBool(crouchParameterID,linkedStateMachine.CurrentState == State.Crouch);
    }

    public void TryToCrouch(InputAction.CallbackContext value)
    {
        if(!IsPermitted) return;
        if(!linkedPhysic.Grounded || linkedStateMachine.CurrentState == State.Dash 
                                  || linkedStateMachine.CurrentState == State.Ladders  ) return;
        linkedStateMachine.ChangeState(State.Crouch);
        HadReleaseCrouchButton = false;
    }

    public void StopCrouch(InputAction.CallbackContext value)
    {
        if(!IsPermitted) return;
        HadReleaseCrouchButton = true;
        if(linkedStateMachine.CurrentState != State.Crouch) return;
        if(linkedPhysic.CeilingDetected) return;
        if(linkedGatherInput.HorizontalInput == 0) linkedStateMachine.ChangeState(State.Idle);
        else linkedStateMachine.ChangeState(State.Run);
    }
}
