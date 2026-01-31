using UnityEngine;
using System.Collections;

public class BombRotate : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] float rotationTime; //how long a rotation takes
    [SerializeField] GameObject[] faces; //set of currently active faces
    [SerializeField] int currentFace = 0;
    private bool isRotating = false;

    void Start()
    {
        currentFace = 0;
        bomb = this.gameObject;
        foreach(GameObject face in faces)
        {
            face.SetActive(false);
        }
        faces[0].SetActive(true);
    }

    //rotate the bomb left or right. Check the boolean to make it rotate left
    public void RotateBomb(bool left)
    {
        if (!isRotating)
        {
            faces[currentFace].SetActive(false);
            StartCoroutine(RotationCoroutine(left));
        }
    }

    private IEnumerator RotationCoroutine(bool left)
    {
        isRotating = true;
        float totalDegrees = 0;
        float degrees = 90;

        if (left) {
            degrees = -90;
            if(currentFace == 0)
            {
                currentFace = faces.Length - 1;
            }
            else
            {
                currentFace--;
            }
        }
        else
        {
            if (currentFace == faces.Length - 1)
            {
                currentFace = 0;
            }
            else
            {
                currentFace++;
            }
        }

        while (Mathf.Abs(totalDegrees) < Mathf.Abs(degrees))
        {
            float currentRotation = (degrees * Time.deltaTime) / rotationTime;
            float previousDegrees = totalDegrees;
            totalDegrees += currentRotation;
            if(Mathf.Abs(totalDegrees) > Mathf.Abs(degrees))
            {
                if (left)
                {
                    currentRotation = Mathf.Abs(previousDegrees) - Mathf.Abs(degrees);
                }
                else
                {
                    currentRotation = degrees - previousDegrees;
                }
       
            }
            bomb.transform.Rotate(0, currentRotation, 0);
            yield return null;
        }

        isRotating = false;
        faces[currentFace].SetActive(true);
    }

    //update all the faces. This is called by BombManager when a phase progresses
    public void UpdateFaces(GameObject[] newFaces)
    {
        for(int i = 0; i < faces.Length; i++)
        {
            faces[i] = newFaces[i];
        }

        foreach (GameObject face in faces)
        {
            face.SetActive(false);
        }
        faces[currentFace].SetActive(true);
    }
}
