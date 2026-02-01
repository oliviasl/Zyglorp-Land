using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class buttonShape : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }
}
