using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //replace with enum if we have time
    [SerializeField]
    private string item_type;
    [SerializeField]
    private string item_state = "ready";
    [SerializeField]
    private Image image;
    [SerializeField]
    private Image content_image;
    [SerializeField]
    private LayerMask whatIsShroom;
    [SerializeField]
    private LayerMask whatIsGround;

    private Vector2 direction;
    private Transform hammerPos;
    private Rope rope;
    private GameObject hook;

    public string GetState() {
        return item_state;
    }

    public void UseItem()
    {
        switch (item_type) {
            case "soap":
                // Alter blocks below the player
                List<GameObject> blocks = Player.Instance.GetFloor();
                foreach (GameObject block in blocks) {
                    if (block.TryGetComponent(out Soapable soapable)){
                        soapable.Soap();
                    }
                }
                SetUsed();
                break;
            case "log":
                //place the log infront of the player
                Transform logPos = Player.Instance.GetLogPos();
                GameObject log = Resources.Load<GameObject>("Log");
                Instantiate(log, logPos.position, logPos.rotation);
                SetUsed();
                break;
            case "pot":
                if (item_state == "ready") {
                    //search for mushroom
                    //if mushroom, put shroomy in pot
                    Transform shroomPos = Player.Instance.GetShroomPos();
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(shroomPos.position, 0.05f, whatIsShroom);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i].gameObject.CompareTag("Mushroom"))
                        {
                            colliders[i].gameObject.SetActive(false);
                            item_state = "filled";
                            content_image.enabled= true;
                            return;
                        }
                    }
                } else if (item_state == "filled") {
                    //place shroom
                    //empty pot
                    GameObject shroomRes = Resources.Load<GameObject>("BounceShroom");
                    Transform shroomPos = Player.Instance.GetShroomPos();
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(shroomPos.position, 0.05f, whatIsGround);
                    if (colliders.Length > 0)
                    {
                        GameObject shroom = Instantiate(shroomRes, new Vector2(0f + shroomPos.position.x, 0.7f + shroomPos.position.y), shroomPos.rotation);
                        shroom.GetComponent<BouncePad>().Setup();
                        SetUsed();
                        return;
                    }
                }
                break;
            case "hammer":
                //destroy first obstacle infront of you
                hammerPos = Player.Instance.GetHammerPos();
                if (Player.Instance.GetFacingRight()) {
                    direction = hammerPos.right;
                } else {
                    direction = hammerPos.right * -1;
                }
                RaycastHit2D hammerHit = Physics2D.Raycast(hammerPos.position, direction, 2f, whatIsGround);
                if (hammerHit.collider != null && hammerHit.collider.gameObject.CompareTag("Log"))
                {
                    //if is rope, run destroyrope function instead
                    Destroy(hammerHit.collider.gameObject);

                    //something has been broken, hammer has been used
                    SetUsed();
                }
                break;
            case "rope":
                //attach a rope to the obstacle infront of you
                /*
                if (item_state == "ready") {
                    //fire raycast same as hammer, create hook on contactpoint
                    //link end of rope to player
                    hammerPos = Player.Instance.GetHammerPos();
                    if (Player.Instance.GetFacingRight())
                    {
                        direction = hammerPos.right;
                    }
                    else
                    {
                        direction = hammerPos.right * -1;
                    }
                    RaycastHit2D ropeHit = Physics2D.Raycast(hammerPos.position, direction, 2f, whatIsGround);
                    if (ropeHit.collider != null && ropeHit.collider.gameObject.CompareTag("Log"))
                    {
                        hook = ropeHit.collider.gameObject;
                        rope = hook.AddComponent<Rope>();
                        rope.GenerateRope(hook.GetComponent<Rigidbody2D>(), 4, Player.Instance.GetComponent<Rigidbody2D>(), Player.Instance.GetFacingRight());
                        item_state = "hooked";
                    }
                } else if (item_state == "hooked") {
                    //detach rope from player and attach to nearby object if any.
                    rope.DestroyRope();
                    hammerPos = Player.Instance.GetHammerPos();
                    if (Player.Instance.GetFacingRight())
                    {
                        direction = hammerPos.right;
                    }
                    else
                    {
                        direction = hammerPos.right * -1;
                    }
                    RaycastHit2D ropeHit = Physics2D.Raycast(hammerPos.position, direction, 2f, whatIsGround);
                    if (ropeHit.collider != null && ropeHit.collider.gameObject.CompareTag("Log")) //&& distance is not more than max rope distance
                    {
                        GameObject hook2 = ropeHit.collider.gameObject;
                        rope = hook2.AddComponent<Rope>();
                        rope.GenerateRope(hook2.GetComponent<Rigidbody2D>(), 4, hook.GetComponent<Rigidbody2D>(), Player.Instance.GetFacingRight());
                        SetUsed();
                    }
                }*/
                break;
        }
    }

    private void SetUsed() {
        item_state = "used";
        image.enabled = false;
        if (item_type == "pot")
        {
            content_image.enabled = false;
        }
    }

    public void ResetItem() {
        item_state = "ready";
        image.enabled = true;
        if (item_type == "pot")
        {
            content_image.enabled = false;
        }
    }
}
