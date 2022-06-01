﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollsionTrigger : MonoBehaviour {

    [SerializeField]
    private BoxCollider2D platformCollider;

    [SerializeField]
    private BoxCollider2D platformTrigger;

	// Use this for initialization
	void Start () {
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Log")) {
            Physics2D.IgnoreCollision(platformCollider, other, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Log"))
        {
            Physics2D.IgnoreCollision(platformCollider, other, false);
        }
    }
}
