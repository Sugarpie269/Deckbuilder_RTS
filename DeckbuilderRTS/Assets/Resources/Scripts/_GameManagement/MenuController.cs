using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //private AssetBundle LoadedAssetBundle;
    //private string[] ScenePaths;
    // Start is called before the first frame update
    void Start()
    {
        //LoadedAssetBundle = AssetBundle.LoadFromFile("Assets/scenes");
        //ScenePaths = LoadedAssetBundle.GetAllScenePaths();
    }

    public void ClickStartButton()
    {
        SceneManager.LoadScene("MainMap");
    }

    public void ClickInstructionsButton()
    {
        SceneManager.LoadScene("Instructions Screen");
    }

    public void ClickQuitGameButton()
    {
        Application.Quit();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
