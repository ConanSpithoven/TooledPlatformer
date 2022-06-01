using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D hook;

    private List<GameObject> ropeSegs = new List<GameObject>();

    public void GenerateRope(Rigidbody2D hook, int links, Rigidbody2D hook2, bool facingRight)
    {
        GameObject ropeSeg = Resources.Load<GameObject>("RopeSegment");
        Rigidbody2D prevBod = hook;
        Vector3 newSegPos;
        if (facingRight) {
            newSegPos = transform.position;
        } else {
            newSegPos = transform.position - transform.right;
        }
        for (int i = 0; i < links; i++)
        {
            GameObject newSeg = Instantiate(ropeSeg, newSegPos, Quaternion.identity, transform);
            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBod;

            ropeSegs.Add(newSeg);
            prevBod = newSeg.GetComponent<Rigidbody2D>();
        }

        GameObject lastSeg = ropeSegs[ropeSegs.Count - 1];

        HingeJoint2D secondHJ = lastSeg.AddComponent<HingeJoint2D>();
        lastSeg.GetComponent<BoxCollider2D>().enabled = false;
        float spriteRight = lastSeg.GetComponent<SpriteRenderer>().bounds.size.x;
        secondHJ.anchor = new Vector2(spriteRight * 2, 0);
        secondHJ.connectedBody = hook2;
    }

    public void DestroyRope() {
        foreach (GameObject ropeSeg in ropeSegs) {
            Destroy(ropeSeg);
        }
        Destroy(gameObject);
    }
}
