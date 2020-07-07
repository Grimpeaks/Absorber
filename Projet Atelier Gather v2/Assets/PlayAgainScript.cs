using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainScript : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene("DevMathieu");
    }
}
