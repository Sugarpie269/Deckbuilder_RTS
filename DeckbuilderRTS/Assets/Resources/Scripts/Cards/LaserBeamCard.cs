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
        private float SummonDistance = 7.0f;
        // private float Direction;
        // private float Length = 100.0f;
        [SerializeField]
        private float Delay = 1.0f;
        [SerializeField]
        private float Lifetime = 2.0f;

        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public LaserBeamCard(Object prefab)
        {
            this.LaserBeamPrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            var gameMaster = GameObject.Find("GameController");
            var gameController = gameMaster.GetComponent<GameController>();

            this.Info = gameController.GetCardInfo("Card_LaserBeam");
        }

        // Returns a struct of card information, for use in the UI. ~Liam
        public CardInfo GetCardInfo()
        {
            return this.Info;
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            Debug.Log("Playing Laser Beam");
            var playerController = player.GetComponent<PlayerController>();
            var playerPos = player.transform.position;
            var direction = playerController.GetMousePosition();
            var position = new Vector3(playerPos.x + direction.x * this.SummonDistance, playerPos.y + direction.y * this.SummonDistance, player.transform.position.z);
            // TODO: instantiate then use delay to prevent effects from happening until timer is up
            var laserbeam = Object.Instantiate(this.LaserBeamPrefab) as GameObject;
            laserbeam.transform.position = position;

            // set rotation
            var vec = Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);
            var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) + 90;

            var laserbeamController = laserbeam.GetComponent<LaserBeamController>();
            laserbeamController.SetAttributes(this.Info.CardPower, Delay, Lifetime, angle);
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), laserbeam.GetComponent<BoxCollider2D>());
            GameObject.Destroy(laserbeam, this.Lifetime);
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

