using UnityEngine;

public class WallSlideAbility : BaseAbility
{
   private string wallSlideAnimationName = "WallSlide";
   private int wallSlideParameterID;
   [SerializeField] private float slideFallSpeed;

   protected override void Start()
   {
      base.Start();
      wallSlideParameterID = Animator.StringToHash(wallSlideAnimationName);
   }

   public override void EnterAbility()
   {
      base.EnterAbility();
      player.ResetInclineToBase();
   }

   public override void ProcessAbility()
   {

      if (!IsPermitted)
      {
         linkedStateMachine.ChangeState(linkedStateMachine.PreviousState);
         return;
      }
      
      if (linkedPhysic.Grounded)
      {
         if(linkedGatherInput.HorizontalInput == 0)  linkedStateMachine.ChangeState(State.Idle);
         else linkedStateMachine.ChangeState(State.Run);
         return;
      }
      
      if (linkedGatherInput.HorizontalInput == 0 || !linkedPhysic.IsOnWall)
      {
         Debug.Log("Change To Air State");
         linkedStateMachine.ChangeState(State.Jump);
         return;
      }

      linkedPhysic.RB.linearVelocity = new Vector2(linkedPhysic.RB.linearVelocityX,-slideFallSpeed);
   }

   public override void UpdateAnimation()
   {
      linkedAnimator.SetBool(wallSlideParameterID, linkedStateMachine.CurrentState == State.WallSlide);
   }
}
