using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class buttonShape : MonoBehaviour
{
    public Image theButton;

    void Start()
    {
        theButton.alphaHitTestMinimumThreshold = 0.5f;
    }
}
