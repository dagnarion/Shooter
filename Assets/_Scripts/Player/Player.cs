using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public GatherInput Input { get; private set; }
    [SerializeField] private Transform sprite;
    [SerializeField] private float inclinedAngle;
    private BaseAbility[] playerAbilities;
    public StateMachine StateMachine { get; private set; }
    public Animator anim;
    public PhysicControl PhysicControl;
    private bool isFacingRight = true;
    public float FacingDirection { get; private set; } = 1;
    private void Awake()
    {
        StateMachine = new StateMachine();
        playerAbilities = this.GetComponents<BaseAbility>();
        StateMachine.Abilities = playerAbilities;
    }

    private void Start()
    {
        StateMachine.ForceChange(State.Idle);
    }

    private void Update()
    {
        foreach (var ability in playerAbilities)
        {
            if (ability.ThisAbility == StateMachine.CurrentState)
            {
                if(ability.IsPermitted)
                    ability.ProcessAbility();
            }
            ability.UpdateAnimation(); // maybe bug here
        }
        
    }    
    
    private void FixedUpdate()
    {
        foreach (var ability in playerAbilities)
        {
            if (ability.ThisAbility == StateMachine.CurrentState)
            {
                if(ability.IsPermitted)
                    ability.ProcessFixedAbility();
            }
        }
    }

    public bool Flip()
    {
        if (isFacingRight && Input.HorizontalInput < 0)
        {
            isFacingRight = !isFacingRight;
            FacingDirection = FacingDirection * -1;
            transform.Rotate(0,180,0);
            return true;
        }
        
        if (!isFacingRight && Input.HorizontalInput > 0)
        {
            isFacingRight = !isFacingRight;
            FacingDirection = FacingDirection * -1;
            transform.Rotate(0,180,0);
            return true;
        }

        return false;
    }

    public void Incline()
    {
        if (Input.HorizontalInput == 0)
        {
            sprite.localEulerAngles = new Vector3(0, 0, 0);
        }
        else sprite.localEulerAngles = new Vector3(0, 0, inclinedAngle);
    }

    public void ResetInclineToBase()
    {
        sprite.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void ForceFlip()
    {
        isFacingRight = !isFacingRight;
        FacingDirection = FacingDirection * -1;
        transform.Rotate(0,180,0);
    }
}

// cách dùng enum thay vì lưu ref đến các state sẽ tốn hiệu năng trong việc tìm state hiện tại để chạy
