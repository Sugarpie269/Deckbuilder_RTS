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

        // Start is called before the first frame update
        void generateCard(string cardType) {
            switch (cardType)
            {
                case "Card_Fireball":
                    card = gameController.GenerateCardFireball();
                    break;
                case "Card_InstantHeal":
                    card = gameController.GenerateCardInstantHeal();
                    break;
                case "Card_SummonWorker":
                    card = gameController.GenerateCardSummonWorker();
                    break;
                default:
                    Debug.Log("Error card not available");
                    break;
            }
        }
        void rtnFireballInfo() {
            Debug.Log("New FireballCard!");
            this.FireballPrefab = prefab;
            // Instantiate each piece of information about the card. ~Liam
            this.cardinfo.CardReference = GameObject.Find("Card_Fireball");
            this.cardinfo.CardArt = this.cardinfo.CardReference.transform.GetChild(1).gameObject.GetComponent<Image>().sprite;
            this.cardinfo.CardName = this.cardinfo.CardReference.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.CardType = this.cardinfo.CardReference.transform.GetChild(3).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.DescriptionHeader = this.cardinfo.CardReference.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.DescriptionContent = this.cardinfo.CardReference.transform.GetChild(4).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.FlavorText = this.cardinfo.CardReference.transform.GetChild(4).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.CardLevel = int.Parse(this.cardinfo.CardReference.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.CardPower = int.Parse(this.cardinfo.CardReference.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.CardStrength = int.Parse(this.cardinfo.CardReference.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.ManaCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.EnergyCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.MatterCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text);
        }

        void rtnSumWorkInfo() {
            Debug.Log("New Summon worker!");
            this.WorkerPrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            this.cardinfo.CardReference = GameObject.Find("Card_SummonWorker");
            this.cardinfo.CardArt = this.cardinfo.CardReference.transform.GetChild(1).gameObject.GetComponent<Image>().sprite;
            this.cardinfo.CardName = this.cardinfo.CardReference.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.CardType = this.cardinfo.CardReference.transform.GetChild(3).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.DescriptionHeader = this.cardinfo.CardReference.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.DescriptionContent = this.cardinfo.CardReference.transform.GetChild(4).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.FlavorText = this.cardinfo.CardReference.transform.GetChild(4).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.CardLevel = int.Parse(this.cardinfo.CardReference.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.CardPower = int.Parse(this.cardinfo.CardReference.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.CardStrength = int.Parse(this.cardinfo.CardReference.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.ManaCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.EnergyCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.MatterCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text);
        }

        void rtnInstHealInfo() {
            Debug.Log("New instant heal!");
            this.cardinfo.CardReference = GameObject.Find("Card_InstantHeal");
            this.cardinfo.CardArt = this.cardinfo.CardReference.transform.GetChild(1).gameObject.GetComponent<Image>().sprite;
            this.cardinfo.CardName = this.cardinfo.CardReference.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.CardType = this.cardinfo.CardReference.transform.GetChild(3).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.DescriptionHeader = this.cardinfo.CardReference.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.DescriptionContent = this.cardinfo.CardReference.transform.GetChild(4).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.FlavorText = this.cardinfo.CardReference.transform.GetChild(4).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.cardinfo.CardLevel = int.Parse(this.cardinfo.CardReference.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.CardPower = int.Parse(this.cardinfo.CardReference.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.CardStrength = int.Parse(this.cardinfo.CardReference.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.ManaCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.EnergyCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.cardinfo.MatterCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text);
        }
        void getCardInfo() {
            switch (this.cardType) {
                case "Card_Fireball":
                    rtnFireballInfo();
                    break;
                case "Card_InstantHeal":
                    rtnSumWorkInfo();
                    break;
                case "Card_SummonWorker":
                    rtnInstHealInfo();
                    break;
                default:
                    Debug.Log("Error card not available");
                    break;
            }

        }
        void Start()
        {
            var gameMaster = GameObject.Find("GameController");
            this.gameController = gameMaster.GetComponent<GameController>();
            getCardInfo();
            generateCard(cardinfo.CardType);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Player") {
                Debug.Log("Player touched the market");
                // disp UI
                if (Input.GetKey(KeyCode.B))
                {
                    Debug.Log("Buy card");
                    var playerController = player.GetComponent<PlayerController>();

                    if (playerController.GetMana() >= cardinfo.ManaCost
                        && playerController.GetMatter() >= cardinfo.MatterCost
                        && playerController.GetEnery() >= cardinfo.EnergyCost){
                        playerController.DecEnery(cardinfo.EnergyCost);
                        playerController.DecMana(cardinfo.ManaCost);
                        playerController.DecMatter(cardinfo.MatterCost);
                        Debug.Log("Add a card");
                        // ADD A SWITCH STATEMENT TO CREATE THE RIGHT KIND OF CARD
                        var newcard = gameController.GenerateCardFireball();
                        playerController.AddCard(newcard);
                    }
                    else {
                        Debug.Log("Insufficent funds");
                    }

                }
            }
        }
    }
}
