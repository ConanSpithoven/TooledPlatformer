using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D bouncecol;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Log")) && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y == 0) {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 800));
        }
    }

    public void Setup() {
        StartCoroutine("BounceCooldown");
    }

    private IEnumerator BounceCooldown() {
        bouncecol.enabled = false;
        yield return new WaitForSeconds(0.5f);
        bouncecol.enabled = true;
    }
}
