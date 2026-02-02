using UnityEngine;

public class bombAnimManager : MonoBehaviour
{
    [SerializeField] GlorpIt glorp;

    public void NextStage()
    {
        BombManager.instance.UpdateAllFaces();
    }
}
