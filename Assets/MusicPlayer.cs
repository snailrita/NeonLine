using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadFistScene", 2f);
    }

    void LoadFistScene()
    {
        SceneManager.LoadScene(1);
    }
}
