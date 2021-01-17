using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beat : MonoBehaviour
{
    RectTransform t;
    Vector3 originalScale;
    void Start()
    {
        t = this.GetComponent<RectTransform>();

        originalScale = t.localScale;

    }

    public void setScale(float prc)
    {
        t.localScale = new Vector3(originalScale.x * prc, originalScale.y * prc, originalScale.z);
    }
}
