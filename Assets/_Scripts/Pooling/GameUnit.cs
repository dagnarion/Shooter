using Unity.VisualScripting;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    public PoolType PoolType;

    public Transform TF
    {
        get
        {
            return transform;
        }
    }
    
}
