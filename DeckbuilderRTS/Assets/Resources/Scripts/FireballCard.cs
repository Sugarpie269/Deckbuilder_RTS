using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class FireballCard : ICard
    {
        private string Name = "Fireball";
        private Vector3Int Cost;
        private uint level;
        private Object FireballPrefab;
        private float SummonDistance = 3.0f;
        private float FireballDirection;
        private float FireballSpeed = 15.0f;
        private float FireballDamage = 5.0f; // use separate power to set damage based on level
        [SerializeField] private Texture2D CardImage;

        public FireballCard(Object prefab)
        {
            Debug.Log("New FireballCard!");
            this.FireballPrefab = prefab;
            this.Cost = new Vector3Int(1, 0, 0);
            this.CardImage = Resources.Load<Texture2D>("Sprites/FireballCard_1000x1500");
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
            fireballController.SetAttributes(this.FireballDamage, new Vector2(this.FireballSpeed * fireballDirection.x, this.FireballSpeed * fireballDirection.y));
            GameObject.Destroy(newFireball, 5f);
        }

        public string GetName()
        {
            return this.Name;
        }

        public Vector3 GetCost()
        {
            return this.Cost;
        }

        public int GetPower()
        {
            return (int)this.FireballDamage;
        }

        public int GetStrength()
        {
            return 0;
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

        // Returns the image of this card, for use in the UI. ~Liam
        public Texture2D GetCardImage()
        {
            return this.CardImage;
        }

    }
}

