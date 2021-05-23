using UnityEngine;

using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] public Object FireballPrefab;
        [SerializeField] public Object WorkerPrefab;
        // The start function will initialize our member variables.
        public void Start()
        {
        }

        public void Update()
        {
        }

        public ICard GenerateCardFireball()
        {
            return new FireballCard(this.FireballPrefab);
        }

        public ICard GenerateCardInstantHeal()
        {
            return new InstantHealCard();
        }

        public ICard GenerateCardSummonWorker(GameObject player = null)
        {
            return new SummonWorkerCard(this.WorkerPrefab);
        }
    }
}
