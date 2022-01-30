using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadLevel : MonoBehaviour
{
    public string level;
    public int time = 5;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("NewLevel", time);
    }

    // Update is called once per frame
    void NewLevel()
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}
