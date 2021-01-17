using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score_counter : MonoBehaviour
{
    public GameManager manager;
    void Update()
    {
        GetComponent<Text>().text = "" + manager.getScore();
    }
}
