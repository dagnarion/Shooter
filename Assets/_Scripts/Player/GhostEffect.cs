using System;
using System.Collections;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
   [Header("Reference")]
   [SerializeField] private GameObject Ghost;
   
   private float ghostDelayTime;
   private Coroutine effectCO;
   public void PlayEffect(float numberOfGhost,float ghostTime)
   {
      if(effectCO != null) StopCoroutine(effectCO);
      effectCO = StartCoroutine(HandleEffect(numberOfGhost,ghostTime));
   }
   
   IEnumerator HandleEffect(float numberOfGhost,float ghostTime)
   {
      ghostDelayTime = ghostTime / numberOfGhost;
      float timer = 0;

      while (timer <= ghostTime)
      {
         timer += ghostDelayTime;
         GameUnit unit = SimplePool.Spawn<GameUnit>(PoolType.PlayerGhost, transform.position, transform.rotation);
         // GameObject go = Instantiate(Ghost, transform.position, Quaternion.identity);
         // go.transform.rotation = this.transform.rotation;
         StartCoroutine(DisableObjectAfterTime(unit, ghostTime));
         yield return new WaitForSeconds(ghostDelayTime);
      }
   }

   IEnumerator DisableObjectAfterTime(GameUnit obj,float timer)
   {
      yield return new WaitForSeconds(timer);
      SimplePool.Despawn(obj);
   
   }
}
