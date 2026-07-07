using Unity.VisualScripting;
using UnityEngine;

public class MoveAbiltiy : BaseAbility
{
    private string moveAnimationName = "Move";
    private int moveParameterID;
    [SerializeField] private float maxSpeed;
    [SerializeField] private AnimationCurve speedCurve;

    private float speed;
    private float time = 0;
    
    protected override void Init()
    {
        base.Init();
        moveParameterID = Animator.StringToHash(moveAnimationName);
    }

    public override void EnterAbility()
    {
        base.EnterAbility();
        time = 0;
    }

    public override void ProcessAbility()
    {
        if (!IsPermitted) return;

        if (linkedGatherInput.HorizontalInput == 0 && linkedPhysic.Grounded)
        {
            linkedStateMachine.ChangeState(State.Idle);
            return;
        }

        if (!linkedPhysic.Grounded)
        {
            linkedStateMachine.ChangeState(State.Jump);
            return;
        }
        player.Incline();
        player.Flip();
        time += Time.deltaTime;
        speed = speedCurve.Evaluate(time) * maxSpeed;
        linkedPhysic.RB.linearVelocity = new Vector2(speed * linkedGatherInput.HorizontalInput, linkedPhysic.RB.linearVelocityY);
    }
    
    public override void UpdateAnimation()
    {
        linkedAnimator.SetBool(moveParameterID,linkedStateMachine.CurrentState == State.Run);
        linkedAnimator.SetFloat("MoveMultiplier",1.1f);
    }
}
