using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] AllTextInstructions;
    private GameObject[] AllImageInstructions;

    // Game objects for the Objective page. ~Liam
    [SerializeField] private GameObject ObjectiveTitleText;
    [SerializeField] private GameObject ObjectiveNarrationText;
    [SerializeField] private GameObject ObjectiveBasicText;
    [SerializeField] private GameObject ObjectiveBossImage;

    // Game objects for the Basic Controls page. ~Liam
    [SerializeField] private GameObject ControlsTitleText;
    [SerializeField] private GameObject ControlText;

    // Game objects for the Cards page. ~Liam
    [SerializeField] private GameObject CardUsageTitleText;
    [SerializeField] private GameObject DetailedCardImage;
    [SerializeField] private GameObject CardTypeText;
    [SerializeField] private GameObject CardCostText;
    [SerializeField] private GameObject CardStrengthText;
    [SerializeField] private GameObject CardPowerText;
    [SerializeField] private GameObject CardLevelText;
    [SerializeField] private GameObject CardExplanationText;
    [SerializeField] private GameObject CardExplanationText2;
    [SerializeField] private GameObject PriceExplanationText;

    // Game objects for the Markets page. ~Liam
    [SerializeField] private GameObject MarketsTitleText;
    [SerializeField] private GameObject MarketsExplanationText;
    [SerializeField] private GameObject MarketsExplanationText2;
    [SerializeField] private GameObject MarketsImage;

    // Game objects for the Resources page. ~Liam
    [SerializeField] private GameObject ResourcesTitleText;
    [SerializeField] private GameObject ResourcesExplanationText;
    [SerializeField] private GameObject ResourcesExplanationText2;
    [SerializeField] private GameObject ResourcesExampleImage;
    [SerializeField] private GameObject ResourcesImageDescriptionText;
    void Start()
    {
        // Get all game objects with the Instructions Text tag, for deactivating quickly. ~Liam
        this.AllTextInstructions = GameObject.FindGameObjectsWithTag("Instruction Text");
        this.AllImageInstructions = GameObject.FindGameObjectsWithTag("Instruction Image");
        this.DeactivateAllInfo();
        // set default to controls ~ Jarod
        this.ClickControlsButton();
    }

    public void ClickStartButton()
    {
        SceneManager.LoadScene("MainMap");
    }

    public void ClickObjectiveButton()
    {
        // Deactivate any text on screen. ~Liam
        this.DeactivateAllInfo();

        // Activate the objects related to objective information. ~Liam
        this.ObjectiveTitleText.SetActive(true);
        this.ObjectiveNarrationText.SetActive(true);
        this.ObjectiveBasicText.SetActive(true);
        this.ObjectiveBossImage.SetActive(true);
    }

    public void ClickControlsButton()
    {
        // Deactivate any text on screen. ~Liam
        this.DeactivateAllInfo();

        // Activate the objects related to control information. ~Liam
        this.ControlsTitleText.SetActive(true);
        this.ControlText.SetActive(true);
    }

    public void ClickCardUsageButton()
    {
        // Deactivate any text on screen. ~Liam
        this.DeactivateAllInfo();

        // Activate objects related to card usage information. ~Liam
        this.CardUsageTitleText.SetActive(true);
        this.DetailedCardImage.SetActive(true);
        this.CardTypeText.SetActive(true);
        this.CardCostText.SetActive(true);
        this.CardStrengthText.SetActive(true);
        this.CardPowerText.SetActive(true);
        this.CardLevelText.SetActive(true);
        this.CardExplanationText.SetActive(true);
        this.CardExplanationText2.SetActive(true);
        this.PriceExplanationText.SetActive(true);
    }

    public void ClickMarketsButton()
    {
        // Deactivate any text on screen. ~Liam
        this.DeactivateAllInfo();

        // Activate objects related to market information. ~Liam
        this.MarketsTitleText.SetActive(true);
        this.MarketsExplanationText.SetActive(true);
        this.MarketsExplanationText2.SetActive(true);
        this.MarketsImage.SetActive(true);
    }
    
    public void ClickResourcesButton()
    {
        // Deactivate any text on screen. ~Liam
        this.DeactivateAllInfo();

        // Activate objects related to resource information. ~Liam
        this.ResourcesTitleText.SetActive(true);
        this.ResourcesExplanationText.SetActive(true);
        this.ResourcesExplanationText2.SetActive(true);
        this.ResourcesExampleImage.SetActive(true);
        this.ResourcesImageDescriptionText.SetActive(true);
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
