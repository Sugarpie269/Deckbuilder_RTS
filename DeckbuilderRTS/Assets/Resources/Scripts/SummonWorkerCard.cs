using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class SummonWorkerCard : ICard
    {
        private string Name = "Summon Worker";
        private Vector3Int Cost;
        private uint level;
        private int HP; // TODO: calculate based on card level and toughness/strength stat
        private Object WorkerPrefab;
        private float SummonDistance = 3.0f;
        [SerializeField] private Texture2D CardImage;

        public SummonWorkerCard(Object prefab)
        {
            this.WorkerPrefab = prefab;
            this.Cost = new Vector3Int(1, 0, 0);
            this.CardImage = Resources.Load<Texture2D>("Sprites/FireballCard_1000x1500");
        }

        public void OnCardPlayed(GameObject player, Vector2 target)
        {
            var newWorker = Object.Instantiate(this.WorkerPrefab) as GameObject;
            newWorker.transform.position = player.transform.position;
            var workerController = newWorker.GetComponent<WorkerController>();
            workerController.SetPlayer(player);
            // Old code: new Vector3(player.transform.position.x + this.SummonDistance, player.transform.position.y, player.transform.position.z);

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
            return 0;
        }
        public int GetStrength()
        {
            return this.HP;
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

        // Returns the image of this card, for use in the UI. ~Liam
        public Texture2D GetCardImage()
        {
            return this.CardImage;
        }
    }
}

