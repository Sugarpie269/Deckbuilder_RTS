using UnityEngine;
using System.Collections.ObjectModel;
using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] public Object FireballPrefab;
        [SerializeField] public Object WorkerPrefab;
        private float GameTimerMinutes = 0.0f;
        private float GameTimerSeconds = 0.0f;
        [SerializeField] private float SummonSwarmlingDelay = 10.0f;
        private float CurrentSummonSwarmlingDelay = 0.0f;

        // The start function will initialize our member variables.
        public void Start()
        {

        }

        public void Update()
        {
            this.GameTimerSeconds += Time.deltaTime;
            this.CurrentSummonSwarmlingDelay += Time.deltaTime;

            // Reset the seconds clock if a minute should be added. ~Jackson
            if (this.GameTimerSeconds >= 60.0f)
            {
                this.GameTimerSeconds -= 60.0f;
                this.GameTimerMinutes += 1.0f;
            }

            if (this.CurrentSummonSwarmlingDelay >= this.SummonSwarmlingDelay)
            {
                this.CurrentSummonSwarmlingDelay -= this.SummonSwarmlingDelay;
                this.SummonNewSwarmling();
            }
        }

        private void SummonNewSwarmling()
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
