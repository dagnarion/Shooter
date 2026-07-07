using UnityEngine;

public class BaseAbility : MonoBehaviour
{
   protected Player player;
   protected GatherInput linkedGatherInput;
   protected StateMachine linkedStateMachine;
   protected Animator linkedAnimator;
   protected PhysicControl linkedPhysic;
   [field:SerializeField] public State ThisAbility { get; protected set; }
   [field:SerializeField] public bool IsPermitted { get; protected set; } = true;

   protected virtual void Start()
   {
      Init();
   }

   public virtual void EnterAbility()
   {
      
   }

   public virtual void ExitAbility()
   {
      
   }

   public virtual void ProcessAbility()
   {
      
   }

   public virtual void ProcessFixedAbility()
   {
      
   }

   public virtual void UpdateAnimation()
   {
      
   }

   protected virtual void Init()
   {
      player = this.GetComponent<Player>();
      if (player != null)
      {
         linkedStateMachine = player.StateMachine;
         linkedGatherInput = player.Input;
         linkedAnimator = player.anim;
         linkedPhysic = player.PhysicControl;
      }
   }
}
