using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class LaserBeamCard : ICard
    {
        [SerializeField]
        private Object LaserBeamPrefab;
        private float SummonDistance = 2.0f;
        private float Direction;
        private float Length = 100.0f;
        private float delay = 1.0f;

        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public LaserBeamCard(Object prefab)
        {
            this.LaserBeamPrefab = prefab;

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
            var direction = playerController.GetMousePosition();
            var position = new Vector3(playerPos.x + direction.x * this.SummonDistance, playerPos.y + direction.y * this.SummonDistance, player.transform.position.z);
            // TODO: instantiate then use delay to prevent effects from happening until timer is up
            var laserbeam = Object.Instantiate(this.LaserBeamPrefab) as GameObject;
            laserbeam.transform.position = position;

            var laserbeamController = laserbeam.GetComponent<LaserBeamController>();
            // laserbeamController.SetAttributes(this.Info.CardPower, new Vector2(this.ForceBoltSpeed * direction.x, this.ForceBoltSpeed * direction.y));
            GameObject.Destroy(laserbeam, 5f);
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

