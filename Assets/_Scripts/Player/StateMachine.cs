using UnityEngine;

public class StateMachine
{
   public State PreviousState { get; private set; }
   public State CurrentState { get; private set; }
   public BaseAbility[] Abilities; // lưu ý chỗ này nên refactor sau

   public void ChangeState(State newState)
   {
      if(newState == CurrentState) return;
      foreach (var ability in Abilities)
      {
         if (ability.ThisAbility == CurrentState)
         {
            PreviousState = CurrentState;
            ability.ExitAbility();
         }
      }

      foreach (var ability in Abilities)
      {
         if (ability.ThisAbility == newState)
         {
            if (ability.IsPermitted)
            {
               ability.EnterAbility();
               CurrentState = newState;
            }
            return;
         }
      }
   }

   public void ForceChange(State newState)
   {
      PreviousState = CurrentState;
      CurrentState = newState;
   }
   
}
