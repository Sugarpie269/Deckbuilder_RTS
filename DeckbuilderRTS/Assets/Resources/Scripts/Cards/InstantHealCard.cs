using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class InstantHealCard : ICard
    {
        [SerializeField]
        private Object cardPrefab; // prefab for instantiation, 
        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public InstantHealCard()
        {
            // Instantiate each piece of information about the card. ~Liam
            var gameMaster = GameObject.Find("GameController");
            var gameController = gameMaster.GetComponent<GameController>();

            this.Info = gameController.GetCardInfo("Card_InstantHeal");
        }

        // Returns a struct of card information, for use in the UI. ~Liam
        public CardInfo GetCardInfo()
        {
            return this.Info;
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            player.GetComponent<PlayerController>().ApplyHealing(this.Info.CardStrength);
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

