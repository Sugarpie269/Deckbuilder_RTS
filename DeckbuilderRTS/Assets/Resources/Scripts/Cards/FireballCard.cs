using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class FireballCard : ICard
    {
        [SerializeField]
        private Object FireballPrefab;
        private float SummonDistance = 2.0f;
        private float FireballDirection;
        private float FireballSpeed = 15.0f;

        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public FireballCard(Object prefab)
        {
            this.FireballPrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            var gameMaster = GameObject.Find("GameController");
            var gameController = gameMaster.GetComponent<GameController>();

            this.Info = gameController.GetCardInfo("Card_Fireball");
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
            var fireballDirection = playerController.GetMousePosition();

            var fireballPos = new Vector3(playerPos.x + fireballDirection.x * this.SummonDistance, playerPos.y + fireballDirection.y * this.SummonDistance, player.transform.position.z);
            var newFireball = Object.Instantiate(this.FireballPrefab) as GameObject;
            newFireball.transform.position = fireballPos;

            var fireballController = newFireball.GetComponent<FireballController>();
            fireballController.SetAttributes(this.Info.CardPower, new Vector2(this.FireballSpeed * fireballDirection.x, this.FireballSpeed * fireballDirection.y));
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

