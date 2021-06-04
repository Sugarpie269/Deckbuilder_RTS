using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class LeafbladeCard : ICard
    {
        [SerializeField]
        private Object LeafbladePrefab;
        private float SummonDistance = 1.0f;
        private float LeafbladeDirection;
        private float LeafbladeSpeed = 3.0f;

        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public LeafbladeCard(Object prefab)
        {
            this.LeafbladePrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            var gameMaster = GameObject.Find("GameController");
            var gameController = gameMaster.GetComponent<GameController>();

            this.Info = gameController.GetCardInfo("Card_Leafblade");
        }

        // Returns a struct of card information, for use in the UI. ~Liam
        public CardInfo GetCardInfo()
        {
            return this.Info;
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            var playerController = player.GetComponent<PlayerController>();
            var playerPos = player.transform.position;
            var leafBladeDirection = playerController.GetMousePosition();
            var leafbladePos = new Vector3(playerPos.x + leafBladeDirection.x * this.SummonDistance, playerPos.y + leafBladeDirection.y * this.SummonDistance, player.transform.position.z);
            var newLeafblade = Object.Instantiate(this.LeafbladePrefab) as GameObject;
            newLeafblade.transform.position = leafbladePos;

            var leafbladeController = newLeafblade.GetComponent<LeafbladeController>();
            leafbladeController.SetAttributes(this.Info.CardPower, new Vector2(this.LeafbladeSpeed * leafBladeDirection.x, this.LeafbladeSpeed * leafBladeDirection.y));
            GameObject.Destroy(newLeafblade, 5f);
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

