using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public string level_name = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(level_name);
    }
}
