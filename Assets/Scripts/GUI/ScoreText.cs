using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    public int score = 0;

    void Update()
    {
        string[] tmp = GetComponent<Text>().text.Split(':');
        GetComponent<Text>().text = tmp[0] + ": " + score + "" ;
    }
}
