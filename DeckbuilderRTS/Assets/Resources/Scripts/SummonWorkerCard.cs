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
        [SerializeField] private Sprite CardImage;

        public SummonWorkerCard(Object prefab)
        {
            this.WorkerPrefab = prefab;
            this.Cost = new Vector3Int(1, 0, 0);
            Texture2D tempTexture = Resources.Load<Texture2D>("Sprites/SummonWorkerCard_1000x1500");
            this.CardImage = Sprite.Create(tempTexture, new Rect(0f, 0f, tempTexture.width, tempTexture.height), new Vector2(0.5f, 0.5f));
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
        public Sprite GetCardImage()
        {
            return this.CardImage;
        }
    }
}

