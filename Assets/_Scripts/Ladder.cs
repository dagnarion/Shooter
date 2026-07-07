using System;
using UnityEngine;

public class Ladder : MonoBehaviour
{
   private LadderAbility ladderAbility;

   private void OnTriggerEnter2D(Collider2D other)
   {
       ladderAbility = other.GetComponent<LadderAbility>();
       if (ladderAbility != null) ladderAbility.CanClimb = true;
   }

   private void OnTriggerExit2D(Collider2D other)
   {
       if (ladderAbility != null) ladderAbility.CanClimb = false;
       return;
   }
}
