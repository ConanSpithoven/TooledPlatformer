using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    [SerializeField]
    protected int health;

    protected Vector2 startPos;

    [SerializeField]
    private EdgeCollider2D SwordHitbox;

    [SerializeField]
    private List<string> damageSources = new List<string>();

    public abstract bool IsDead { get; }

    [SerializeField]
    protected Transform throwPoint;

    [SerializeField]
    protected GameObject projectilePrefab;

    [SerializeField]
    protected float MovementSpeed;
    [SerializeField]
    protected float BaseMovementSpeed;

    protected bool facingRight;
    private bool speedup = false;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    public Animator MyAnimator { get; private set; }

    public virtual void Start () {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
    }
	
	void Update () {
		
	}

    public abstract IEnumerator TakeDamage();

    public abstract void AdjustScore();

    public abstract void Death();

    public void ChangeDirection() {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void ThrowProjectile(int value) {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(projectilePrefab, throwPoint.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            tmp.GetComponent<Projectile>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(projectilePrefab, throwPoint.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            tmp.GetComponent<Projectile>().Initialize(Vector2.left);
        }
    }

    public void MeleeAttackStart() {
        SwordHitbox.enabled = true;
    }

    public void MeleeAttackEnd() {
        SwordHitbox.enabled = false;
    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        if (damageSources.Contains(other.tag)) {
            StartCoroutine(TakeDamage());
        }
    }

    public void ChangeSpeed(bool up) {
        if (up)
        {
            StopCoroutine("SpeedDownTimer");
            MovementSpeed = BaseMovementSpeed + 3;
            speedup = true;
        }
        else {
            StartCoroutine("SpeedDownTimer");
        }
    }

    private IEnumerator SpeedDownTimer()
    {
        speedup = false;
        yield return new WaitForSeconds(2);
        if (!speedup)
        {
            MovementSpeed = BaseMovementSpeed;
        }
        else {
            StartCoroutine("SpeedDownTimer");
        }
    }

    public bool GetFacingRight() {
        return facingRight;
    }
}
