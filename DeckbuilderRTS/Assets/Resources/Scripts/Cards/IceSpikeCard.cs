using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeckbuilderRTS
{
    public class IceSpikeCard : ICard
    {
        [SerializeField]
        private Object IceSpikePrefab;
        private float SummonDistance = 1.0f;
        private float IceSpikeDirection;
        private float IceSpikeSpeed = 10.0f;

        // These are the references to the card's information within the CardDisplayLibrary gameObject. ~Liam
        private CardInfo Info;

        public IceSpikeCard(Object prefab)
        {
            this.IceSpikePrefab = prefab;

            // Instantiate each piece of information about the card. ~Liam
            var gameMaster = GameObject.Find("GameController");
            var gameController = gameMaster.GetComponent<GameController>();

            this.Info = gameController.GetCardInfo("Card_IceSpike");
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
            var iceSpikeDirection = playerController.GetMousePosition();
            var iceSpikePos = new Vector3(playerPos.x + iceSpikeDirection.x * this.SummonDistance, playerPos.y + iceSpikeDirection.y * this.SummonDistance, player.transform.position.z);
            var newIceSpike = Object.Instantiate(this.IceSpikePrefab) as GameObject;
            newIceSpike.transform.position = iceSpikePos;

            // Set the IceSpike's rotation by calculating angle between the 2 points. ~Liam
            var vec = Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);
            var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) + 90;

            var iceSpikeController = newIceSpike.GetComponent<IceSpikeController>();
            iceSpikeController.SetAttributes(this.Info.CardPower, new Vector2(this.IceSpikeSpeed * iceSpikeDirection.x, this.IceSpikeSpeed * iceSpikeDirection.y), angle);

            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), newIceSpike.GetComponent<BoxCollider2D>());
            GameObject.Destroy(newIceSpike, 2f);
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

