using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public string SceneName;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnMouseDown()
    {
        //Application.LoadLevel(SceneName);
        SceneManager.LoadScene(SceneName);
    }
}