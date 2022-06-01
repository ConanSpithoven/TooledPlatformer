using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState
{
    private Enemy enemy;

    private float shootTimer;
    private float shootCooldown = 3;
    private bool canShoot = true;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {

        ShootBullet();
        if (enemy.InMeleeRange) {
            enemy.ChangeState(new MeleeState());
        }
        else if (enemy.Target != null)
        {
            enemy.Move();
        }
        else {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }

    private void ShootBullet() {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootCooldown) {
            canShoot = true;
            shootTimer = 0;
        }

        if (canShoot) {
            canShoot = false;
            enemy.MyAnimator.SetTrigger("throw");
        }
    }
}
