using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public RectTransform Content;

    public float width = 130;

    public int index = 0;

    private float pivValue = 1.5f;

    public void Update()
    {
        float currentValue = (Content.anchoredPosition.x) / 130;

        float result = Mathf.Abs((currentValue + index));
  
        float animationValue = (pivValue - Mathf.Clamp(result, 0, pivValue))/ pivValue;;

        this.transform.localScale = Vector3.one * animationValue;
    }
}
