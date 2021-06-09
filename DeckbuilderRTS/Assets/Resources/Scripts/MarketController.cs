using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS {
    public class MarketController : MonoBehaviour
    {
        public GameObject player;
        public string cardType;
        public Object prefab;

        private Object FireballPrefab;
        private Object WorkerPrefab;
        private ICard card;
        private CardInfo cardinfo;
        private GameController gameController;

        private float CurrentCoolDown;

        // Variables needed for market card examine function. ~Liam
        [SerializeField] private GameObject ExamineText;
        private bool isGameOver;
        private bool PointerHovering;

        private bool loaded = false;

        // Start is called before the first frame update
        private ICard generateCard(string cardType) {
            ICard newcard;
            switch (cardType)
            {
                case "Card_Fireball":
                    newcard = gameController.GenerateCardFireball();
                    return newcard;
                case "Card_InstantHeal":
                    newcard = gameController.GenerateCardInstantHeal();
                    return newcard;
                case "Card_SummonWorker":
                    newcard = gameController.GenerateCardSummonWorker();
                    return newcard;
                case "Card_Leafblade":
                    newcard = gameController.GenerateCardLeafblade();
                    return newcard;
                case "Card_IceSpike":
                    newcard = gameController.GenerateCardIceSpike();
                    return newcard;
                case "Card_Void":
                    newcard = gameController.GenerateCardVoid();
                    return newcard;
                case "Card_LightningStriker":
                    newcard = gameController.GenerateCardLightningStriker();
                    return newcard;
                case "Card_LaserBeam":
                    newcard = gameController.GenerateCardLaserBeam();
                    return newcard;
                default:
                    Debug.Log("Error card not available");
                    return null;
            }
            
        }
        void rtnFireballInfo() {
            this.FireballPrefab = prefab;
            // Instantiate each piece of information about the card. ~Liam
            this.cardinfo = this.gameController.GetCardInfo("Card_Fireball");
        }

        void rtnSumWorkInfo() {
            this.WorkerPrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            this.cardinfo = this.gameController.GetCardInfo("Card_SummonWorker");
        }

        void rtnInstHealInfo() {
            this.cardinfo = this.gameController.GetCardInfo("Card_InstantHeal");
        }
        void getCardInfo() {
            this.cardinfo = this.gameController.GetCardInfo(this.cardType);

        }
        void Start()
        {
            var gameMaster = GameObject.Find("GameController");
            this.gameController = gameMaster.GetComponent<GameController>();
            getCardInfo();
            card = generateCard(cardType);
            this.CurrentCoolDown = 0f;

            // Setup for market card examine function. ~Liam
            this.ExamineText = GameObject.Find("MarketExamineText");
            this.PointerHovering = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!this.loaded)
            {
                this.loaded = true;
                this.ExamineText.SetActive(false);

            }
            if ((player.transform.position - this.transform.position).sqrMagnitude < 10) {
                if (Input.GetButtonDown("PurchaseCard"))
                {
                    var playerController = player.GetComponent<PlayerController>();

                    // Modified the code here to use the ModifyPlayerResource() functions already created in PlayerController.cs. Previous code is commented below. ~Liam
                    if (!playerController.IsPurchaseCooldown()
                        && (playerController.GetMana() - cardinfo.ManaCost >= 0)
                        && (playerController.GetEnergy() - cardinfo.EnergyCost >= 0)
                        && (playerController.GetMatter() - cardinfo.MatterCost >= 0))
                    {
                        var newcard = generateCard(cardType);
                        if (cardinfo.ManaCost > 0)
                        {
                            playerController.ModifyPlayerMana(-cardinfo.ManaCost);
                        }
                        if (cardinfo.EnergyCost > 0)
                        {
                            playerController.ModifyPlayerEnergy(-cardinfo.EnergyCost);
                        }
                        if (cardinfo.MatterCost > 0)
                        {
                            playerController.ModifyPlayerMatter(-cardinfo.MatterCost);
                        }
                        playerController.AddCard(newcard);
                        playerController.SetPurchaseCooldown(playerController.PURCHASE_CARD_COOL_DOWN_BASE);
                        playerController.SetPurchaseSuccessText();
                    }
                    else
                    {
                        // Display error to user upon trying to purchase a card. ~Liam
                        playerController.SetPurchaseErrorText();
                    }
                }
            }
        }

        // UI FUNCTION: Implementation for displaying the market card at the player's discretion. ~Liam
        private void OnMouseEnter()
        {
            

            // Only render the examine text if the player is not already hovering over another UI examine function, like their hand or discard, and if the game is not over. ~Liam
            if (!this.isGameOver)
            {
                if (!GameObject.Find("Card1").GetComponent<ExamineDisplay>().IsPointerHovering()
                && !GameObject.Find("Card2").GetComponent<ExamineDisplay>().IsPointerHovering()
                && !GameObject.Find("Card3").GetComponent<ExamineDisplay>().IsPointerHovering()
                && !GameObject.Find("DiscardPile").GetComponent<ExamineDisplay>().IsPointerHovering())
                {
                    this.ExamineText.SetActive(true);
                }
                this.PointerHovering = true;
            }
        }
        private void OnMouseExit()
        {
            this.ExamineText.SetActive(false);
            this.PointerHovering = false;
        }

        // Returns true while the pointer is hovering over the market. ~Liam
        public bool IsPointerHovering()
        {
            return this.PointerHovering;
        }

        // Returns the CardInfo corresponding to the card in this market. ~Liam
        public CardInfo GetMarketCardInfo()
        {
            return this.cardinfo;
        }

        // Called when the player's game ends, to turn off tooltips and functionality. ~Liam
        public void GameOver()
        {
            this.isGameOver = true;
            this.PointerHovering = false;
            this.ExamineText.SetActive(false);
        }
    }
}
