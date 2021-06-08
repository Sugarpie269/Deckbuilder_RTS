using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class SummonWorkerCard : ICard
    {
        private int HP; // TODO: calculate based on card level and toughness/strength stat
        [SerializeField]
        private Object WorkerPrefab;
        private float SummonDistance = 3.0f;

        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public SummonWorkerCard(Object prefab)
        {
            this.WorkerPrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            var gameMaster = GameObject.Find("GameController");
            var gameController = gameMaster.GetComponent<GameController>();

            this.Info = gameController.GetCardInfo("Card_SummonWorker");
        }

        // Returns a struct of card information, for use in the UI. ~Liam
        public CardInfo GetCardInfo()
        {
            return this.Info;
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            var gameMaster = GameObject.Find("GameController");
            var gameController = gameMaster.GetComponent<GameController>();
            gameController.SummonNewWorker(player);
        }

        // This returns true if the card should be removed from the deck after use. ~Jackson.
        public bool ShouldBeDestroyed()
        {
            return false;
        }


        // This returns true if the card can be removed from the deck after use. ~Jackson.
        public bool CanBeDestroyed()
        {
            // TODO: SHOULD THE PLAYER BE ABLE TO DESTROY THEIR WORKER CARD?
            return true;
        }
    }
}

