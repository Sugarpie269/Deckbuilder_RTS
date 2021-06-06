using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class ForceBoltCard : ICard
    {
        [SerializeField]
        private Object ForceBoltPrefab;
        private float SummonDistance = 2.0f;
        private float ForceBoltDirection;
        private float ForceBoltSpeed = 25.0f;

        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public ForceBoltCard(Object prefab)
        {
            this.ForceBoltPrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            var gameMaster = GameObject.Find("GameController");
            var gameController = gameMaster.GetComponent<GameController>();

            this.Info = gameController.GetCardInfo("Card_ForceBolt");
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
            var forceBoltDirection = playerController.GetMousePosition();
            var forceBoltPos = new Vector3(playerPos.x + forceBoltDirection.x * this.SummonDistance, playerPos.y + forceBoltDirection.y * this.SummonDistance, player.transform.position.z);
            var newForceBolt = Object.Instantiate(this.ForceBoltPrefab) as GameObject;
            newForceBolt.transform.position = forceBoltPos;

            // Set the ForceBolt's rotation by calculating angle between the 2 points. ~Liam
            var vec = Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);
            var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) + 90;

            var forceBoltController = newForceBolt.GetComponent<ForceBoltController>();
            forceBoltController.SetAttributes(this.Info.CardPower, new Vector2(this.ForceBoltSpeed * forceBoltDirection.x, this.ForceBoltSpeed * forceBoltDirection.y), angle);
            GameObject.Destroy(newForceBolt, .1f); // Lifetime of the bolt?
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

