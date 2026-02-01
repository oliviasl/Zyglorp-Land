using UnityEngine;

public class bombAnimManager : MonoBehaviour
{
    public void NextStage()
    {
        BombManager.instance.UpdateAllFaces();
    }
}
