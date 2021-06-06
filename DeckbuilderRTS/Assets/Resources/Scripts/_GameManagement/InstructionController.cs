using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionController : MonoBehaviour
{
    //private AssetBundle LoadedAssetBundle;
    //private string[] ScenePaths;
    // Start is called before the first frame update
    private GameObject[] AllTextInstructions;
    private GameObject[] AllImageInstructions;
    [SerializeField] private GameObject ControlsTitleText;
    [SerializeField] private GameObject ControlText;
    [SerializeField] private GameObject CameraWarningText;
    void Start()
    {
        // Get all game objects with the Instructions Text tag, for deactivating quickly. ~Liam
        this.AllTextInstructions = GameObject.FindGameObjectsWithTag("Instruction Text");
        this.AllImageInstructions = GameObject.FindGameObjectsWithTag("Instruction Image");
        this.DeactivateAllInfo();
    }

    public void ClickStartButton()
    {
        SceneManager.LoadScene("MainMap");
    }

    public void ClickControlsButton()
    {
        // Deactivate any text on screen. ~Liam
        this.DeactivateAllInfo();

        // Activate the objects related to control information. ~Liam
        this.ControlsTitleText.SetActive(true);
        this.ControlText.SetActive(true);
        this.CameraWarningText.SetActive(true);
    }

    public void ClickCardUsageButton()
    {
        // Deactivate any text on screen. ~Liam
        this.DeactivateAllInfo();

        // TODO: Activate objects related to card usage information. ~Liam
    }
    
    private void DeactivateAllInfo()
    {
        // Deactivate each text object. ~Liam
        foreach (GameObject gObject in this.AllTextInstructions)
        {
            gObject.SetActive(false);
        }
        // Deactivate each image object. ~Liam
        foreach (GameObject gObject in this.AllImageInstructions)
        {
            gObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
