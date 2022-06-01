using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soapable : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite soapSprite;
    private bool soaped = false;

    public void Soap(){
        soaped = true;
        spriteRenderer.sprite = soapSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (soaped && collision.CompareTag("Player")) {
            collision.gameObject.GetComponent<Player>().ChangeSpeed(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (soaped && collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().ChangeSpeed(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (soaped && collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().ChangeSpeed(false);
        }
    }
}
