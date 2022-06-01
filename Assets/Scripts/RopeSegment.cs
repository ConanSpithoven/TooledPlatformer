using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    [SerializeField]
    private GameObject connectedLeft, connectedRight;

    void Start()
    {
        connectedLeft = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RopeSegment aboveSegment = connectedLeft.GetComponent<RopeSegment>();
        if (aboveSegment != null)
        {
            aboveSegment.SetConnectedRight(gameObject);
            float spriteRight = connectedLeft.GetComponent<SpriteRenderer>().bounds.size.x;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(spriteRight * 2, 0);
        } else {
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        }
    }

    public void SetConnectedRight(GameObject connectedRight) {
        this.connectedRight = connectedRight;
    }
}
