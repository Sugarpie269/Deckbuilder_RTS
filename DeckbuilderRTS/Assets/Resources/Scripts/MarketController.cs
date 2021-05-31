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

        [SerializeField] private float MarketCoolDown = 10f;
        private float CurrentCoolDown;

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
                default:
                    Debug.Log("Error card not available");
                    return null;
            }
            
        }
        void rtnFireballInfo() {
            Debug.Log("New FireballCard!");
            this.FireballPrefab = prefab;
            // Instantiate each piece of information about the card. ~Liam
            this.cardinfo = this.gameController.GetCardInfo("Card_Fireball");
            /*this.cardinfo.CardReference = GameObject.Find("Card_Fireball");
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
            this.cardinfo.MatterCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text);*/
        }

        void rtnSumWorkInfo() {
            Debug.Log("New Summon worker!");
            this.WorkerPrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            this.cardinfo = this.gameController.GetCardInfo("Card_SummonWorker");
            /*this.cardinfo.CardReference = GameObject.Find("Card_SummonWorker");
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
            this.cardinfo.MatterCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text);*/
        }

        void rtnInstHealInfo() {
            Debug.Log("New instant heal!");
            this.cardinfo = this.gameController.GetCardInfo("Card_InstantHeal");
            /*this.cardinfo.CardReference = GameObject.Find("Card_InstantHeal");
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
            this.cardinfo.MatterCost = int.Parse(this.cardinfo.CardReference.transform.GetChild(8).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text);*/
        }
        void getCardInfo() {
            this.cardinfo = this.gameController.GetCardInfo(this.cardType);


            /*switch (this.cardType) {
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
            }*/

        }
        void Start()
        {
            var gameMaster = GameObject.Find("GameController");
            this.gameController = gameMaster.GetComponent<GameController>();
            getCardInfo();
            card = generateCard(cardType);
            this.CurrentCoolDown = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            this.CurrentCoolDown -= Time.deltaTime;
            if (this.CurrentCoolDown <= 0f)
            {
                this.CurrentCoolDown = 0f;
            }


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
                        && playerController.GetEnery() >= cardinfo.EnergyCost
                        && this.CurrentCoolDown <= 0f){
                        playerController.DecEnery(cardinfo.EnergyCost);
                        playerController.DecMana(cardinfo.ManaCost);
                        playerController.DecMatter(cardinfo.MatterCost);
                        Debug.Log("Add a card");
                        var newcard = generateCard(cardType);
                        playerController.AddCard(newcard);
                        this.CurrentCoolDown = this.MarketCoolDown;
                    }
                    else {
                        Debug.Log("Insufficent funds");
                    }

                }
            }
        }
    }
}
