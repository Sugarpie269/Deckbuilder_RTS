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

    // Game objects for the Basic Controls page. ~Liam
    [SerializeField] private GameObject ControlsTitleText;
    [SerializeField] private GameObject ControlText;
    [SerializeField] private GameObject CameraWarningText;

    // Game objects for the Using Cards page. ~Liam
    [SerializeField] private GameObject CardUsageTitleText;
    [SerializeField] private GameObject DetailedCardImage;
    [SerializeField] private GameObject CardTitleText;
    [SerializeField] private GameObject CardTypeText;
    [SerializeField] private GameObject CardCostText;
    [SerializeField] private GameObject CardStrengthText;
    [SerializeField] private GameObject CardFlavorText;
    [SerializeField] private GameObject CardPowerText;
    [SerializeField] private GameObject CardDescriptionText;
    [SerializeField] private GameObject CardLevelText;
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
        this.CardUsageTitleText.SetActive(true);
        this.DetailedCardImage.SetActive(true);
        this.CardTitleText.SetActive(true);
        this.CardTypeText.SetActive(true);
        this.CardCostText.SetActive(true);
        this.CardStrengthText.SetActive(true);
        this.CardFlavorText.SetActive(true);
        this.CardPowerText.SetActive(true);
        this.CardDescriptionText.SetActive(true);
        this.CardLevelText.SetActive(true);
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
