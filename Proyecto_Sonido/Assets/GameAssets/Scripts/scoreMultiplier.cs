﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreMultiplier : MonoBehaviour
{
    public GameManager manager;
    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "x " + manager.getScoreMult();
    }
}
