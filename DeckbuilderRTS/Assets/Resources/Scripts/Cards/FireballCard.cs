using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class FireballCard : ICard
    {
        private Object FireballPrefab;
        private float SummonDistance = 2.0f;
        private float FireballDirection;
        private float FireballSpeed = 15.0f;

        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public FireballCard(Object prefab)
        {
            Debug.Log("New FireballCard!");
            this.FireballPrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            this.Info.CardReference = GameObject.Find("Card_Fireball");
            this.Info.CardArt = this.Info.CardReference.transform.GetChild(1).gameObject.GetComponent<Image>().sprite;
            this.Info.CardName = this.Info.CardReference.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.Info.CardType = this.Info.CardReference.transform.GetChild(3).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.Info.DescriptionHeader = this.Info.CardReference.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.Info.DescriptionContent = this.Info.CardReference.transform.GetChild(4).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.Info.FlavorText = this.Info.CardReference.transform.GetChild(4).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text;
            this.Info.CardLevel = int.Parse(this.Info.CardReference.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.Info.CardPower = int.Parse(this.Info.CardReference.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.Info.CardStrength = int.Parse(this.Info.CardReference.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.Info.ManaCost = int.Parse(this.Info.CardReference.transform.GetChild(8).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.Info.EnergyCost = int.Parse(this.Info.CardReference.transform.GetChild(8).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text);
            this.Info.MatterCost = int.Parse(this.Info.CardReference.transform.GetChild(8).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text);
        }

        // Returns a struct of card information, for use in the UI. ~Liam
        public CardInfo GetCardInfo()
        {
            return this.Info;
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            Debug.Log("FireballCard.OnCardPlayed()");
            var playerController = player.GetComponent<PlayerController>();
            var playerPos = player.transform.position;
            var fireballDirection = playerController.GetMousePosition();
            var fireballPos = new Vector3(playerPos.x + fireballDirection.x * this.SummonDistance, playerPos.y + fireballDirection.y * this.SummonDistance, player.transform.position.z);
            var newFireball = Object.Instantiate(this.FireballPrefab) as GameObject;
            newFireball.transform.position = fireballPos;

            var fireballController = newFireball.GetComponent<FireballController>();
            fireballController.SetAttributes(this.Info.CardStrength, new Vector2(this.FireballSpeed * fireballDirection.x, this.FireballSpeed * fireballDirection.y));
            GameObject.Destroy(newFireball, 5f);
        }

        // This returns true if the card should be removed from the deck after use. ~Jackson.
        public bool ShouldBeDestroyed()
        {
            return false;
        }

        // This returns true if the card can be removed from the deck after use. ~Jackson.
        public bool CanBeDestroyed()
        {
            return true;
        }
    }
}
