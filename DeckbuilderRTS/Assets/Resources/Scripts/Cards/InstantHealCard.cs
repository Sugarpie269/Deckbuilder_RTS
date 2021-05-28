using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class InstantHealCard : ICard
    {
        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public InstantHealCard()
        {
            // Instantiate each piece of information about the card. ~Liam
            this.Info.CardReference = GameObject.Find("Card_InstantHeal");
            this.Info.CardArt = this.Info.CardReference.transform.GetChild(1).GetComponent<Image>().sprite;
            this.Info.CardName = this.Info.CardReference.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            this.Info.CardType = this.Info.CardReference.transform.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
            this.Info.DescriptionHeader = this.Info.CardReference.transform.GetChild(4).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            this.Info.DescriptionContent = this.Info.CardReference.transform.GetChild(4).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
            this.Info.FlavorText = this.Info.CardReference.transform.GetChild(4).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
            this.Info.CardLevel = int.Parse(this.Info.CardReference.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text);
            this.Info.CardPower = int.Parse(this.Info.CardReference.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text);
            this.Info.CardStrength = int.Parse(this.Info.CardReference.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text);
            this.Info.ManaCost = int.Parse(this.Info.CardReference.transform.GetChild(8).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            this.Info.EnergyCost = int.Parse(this.Info.CardReference.transform.GetChild(8).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
            this.Info.MatterCost = int.Parse(this.Info.CardReference.transform.GetChild(8).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text);
        }

        // Returns a struct of card information, for use in the UI. ~Liam
        public CardInfo GetCardInfo()
        {
            return this.Info;
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            player.GetComponent<PlayerController>().ApplyHealing(this.Info.CardPower);
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

