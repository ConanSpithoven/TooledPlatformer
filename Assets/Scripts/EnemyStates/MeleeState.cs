using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{
    private Enemy enemy;

    private float meleeTimer;
    private float meleeCooldown = 3;
    private bool canMelee = true;

    public void Enter(Enemy enemy)
    {
       this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();
        if (enemy.InShootRange && !enemy.InMeleeRange) {
            enemy.ChangeState(new RangedState());
        }
        else if (enemy.Target == null)
        {        
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    private void Attack()
    {
        meleeTimer += Time.deltaTime;

        if (meleeTimer >= meleeCooldown)
        {
            canMelee = true;
            meleeTimer = 0;
        }

        if (canMelee)
        {
            canMelee = false;
            enemy.MyAnimator.SetTrigger("attack");
        }
    }
}
