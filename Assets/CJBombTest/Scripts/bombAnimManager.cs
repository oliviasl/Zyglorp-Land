using UnityEngine;

public class bombAnimManager : MonoBehaviour
{
    [SerializeField] GlorpIt glorp;

    public void NextStage()
    {
        BombManager.instance.UpdateAllFaces();
    }

    public void SendGlorp(int ans)
    {
        glorp.Glorp(ans);
    }
}
