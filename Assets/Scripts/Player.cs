using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeadEventHandler();

public class Player : Character {

    private static Player instance;
    private bool immortal = false;
    private SpriteRenderer MySpriteRenderer;

    [SerializeField]
    private BarScript bar;
    [SerializeField]
    private ScoreText scoretext;
    [SerializeField]
    private float immortalTime;
    [SerializeField]
    private float JumpForce;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private List<InventorySlot> inventorySlots;
    [SerializeField]
    private Transform logPos;
    [SerializeField]
    private Transform shroomPos;
    [SerializeField]
    private Transform hammerPos;

    public event DeadEventHandler Dead;

    public static Player Instance
    {
        get
        {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    public override bool IsDead
    {
        get
        {
            if (health <= 0)
            {
                OnDead();
            }
            return health <= 0;
        }
    }

    

    public Rigidbody2D MyRigidbody { get; set; }

    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public int score = 0;

    public override void Start () {
        base.Start();
        startPos = transform.position;
        MyRigidbody = GetComponent<Rigidbody2D>();
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        //bar.Health = health;
    }

    private void Update()
    {
        //if (!TakingDamage && !IsDead)
        //{
            if (transform.position.y <= -14f)
            {
                MyRigidbody.velocity = Vector2.zero;
                transform.position = startPos;
                /*health -= 10;
                bar.Health = health;
                scoretext.score -= 10;*/
            }
            HandleInput();
        //}
    }

    void FixedUpdate () {

        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");

            OnGround = IsGrounded();
            HandleMovement(horizontal);

            Flip(horizontal);

            HandleLayers();
        }
    }

    private void HandleInput() {
        /*if (Input.GetKeyDown(KeyCode.E)) {
            MyAnimator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            MyAnimator.SetTrigger("throw");
        }*/
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MyAnimator.SetTrigger("slide");
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            MyAnimator.SetTrigger("jump");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseItem(4);
        }
    }

    private void UseItem(int item_number) {
        if (inventorySlots[item_number].GetState() != "used") {
            inventorySlots[item_number].UseItem();
        }
    }

    public void OnDead() {
        if (Dead != null) {
            Dead();
        }
    }

    private void HandleMovement(float horizontal)
    {
        if (MyRigidbody.velocity.y < 0) {
            MyAnimator.SetBool("land", true);
        }
        if (!Attack && !Slide) {
            MyRigidbody.velocity = new Vector2(horizontal * MovementSpeed, MyRigidbody.velocity.y);
        }
        if (Jump && OnGround) {
            MyRigidbody.AddForce(new Vector2(0, JumpForce));
            Jump = false;
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void Flip(float horizontal) {
        
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
            ChangeDirection();
        }
    }

    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        else if (MyRigidbody.velocity.y <= 0.01)
        {
            
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers() {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void ThrowProjectile(int value) {
        if (!OnGround && value == 1 || OnGround && value == 0) {
            base.ThrowProjectile(value);
        }       
    }

    private IEnumerator IndicateImmortal() {
        while (immortal) {
            MySpriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            MySpriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            health -= 10;
            bar.Health = health;
            scoretext.score -= 10;

            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("death");
                //Display YOU DIED
            }
        }
    }

    public override void Death()
    {
        // reload scene instead?
        MyRigidbody.velocity = Vector2.zero;
        MyAnimator.SetTrigger("idle");
        //health = 100;
        //bar.Health = health;
        transform.position = startPos;
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            inventorySlot.ResetItem();
        }
    }

    public override void AdjustScore()
    {
        scoretext.score -= 300;
    }

    public List<GameObject> GetFloor() {
        List<GameObject> floorTiles = new List<GameObject>();

        foreach (Transform point in groundPoints)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject && colliders[i].gameObject.CompareTag("Ground"))
                {
                    floorTiles.Add(colliders[i].gameObject);
                }
            }
        }


        return floorTiles;
    }

    public Transform GetLogPos() {
        return logPos;
    }

    public Transform GetShroomPos()
    {
        return shroomPos;
    }

    public Transform GetHammerPos()
    {
        return hammerPos;
    }
}
