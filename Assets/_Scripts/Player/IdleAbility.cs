using UnityEngine;

public class IdleAbility : BaseAbility
{
  private string idleAnimationName = "Idle";
  private int idleParameterID;

  protected override void Init()
  {
    base.Init();
    idleParameterID = Animator.StringToHash(idleAnimationName);
  }

  public override void EnterAbility()
  {
    base.EnterAbility();
    linkedPhysic.RB.linearVelocity = new Vector2(0, linkedPhysic.RB.linearVelocityY);
  }

  public override void ProcessAbility()
  {
    if (linkedGatherInput.HorizontalInput != 0)
    {
      linkedStateMachine.ChangeState(State.Run);
      return;
    }
    linkedPhysic.RB.linearVelocity = new Vector2(0, linkedPhysic.RB.linearVelocityY);
    player.Incline();
  }

  public override void UpdateAnimation()
  {
    linkedAnimator.SetBool(idleParameterID,linkedStateMachine.CurrentState == State.Idle);
  }
}
