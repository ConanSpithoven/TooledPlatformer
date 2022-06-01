using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    private IEnemyState currentState;

    [SerializeField]
    private ScoreText scoretext;

    public GameObject Target { get; set; }

    [SerializeField]
    private int meleeRange;
    [SerializeField]
    private int shootRange;

    public bool InMeleeRange {
        get {
            if (Target != null) {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }

    public bool InShootRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= shootRange;
            }
            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    [SerializeField]
    private Transform leftEdge;

    [SerializeField]
    private Transform rightEdge;

    // Use this for initialization
    public override void Start () {
        base.Start();
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
        startPos = transform.position;
    }  
	
	// Update is called once per frame
	void Update () {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
	}

    public void RemoveTarget() {
        Target = null;
        ChangeState(new PatrolState());
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeState(IEnemyState newState) {
        if (currentState != null) {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move() {
        if (!Attack) {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                MyAnimator.SetFloat("speed", 1);

                transform.Translate(GetDirection() * (MovementSpeed * Time.deltaTime));
            }
            else if (currentState is PatrolState) {
                ChangeDirection();
            }
        } 
    }

    public Vector2 GetDirection() {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override void ThrowProjectile(int value)
    {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(projectilePrefab, throwPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            tmp.GetComponent<Projectile>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(projectilePrefab, throwPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)));
            tmp.GetComponent<Projectile>().Initialize(Vector2.left);
        }
    }

    public override IEnumerator TakeDamage()
    {

        health -= 10;

        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else {
            MyAnimator.SetTrigger("death");
            yield return null;
        }
    }

    public override void Death()
    {
        Target = null;
        MyAnimator.ResetTrigger("death");
        MyAnimator.SetTrigger("idle"); 
        health = 30;
        transform.position = startPos;
    }

    public override void AdjustScore()
    {
        scoretext.score += 100;
    }
}
