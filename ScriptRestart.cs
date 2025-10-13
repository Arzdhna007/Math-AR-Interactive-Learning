using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptRestart : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene("bangunruang");
    }
}
