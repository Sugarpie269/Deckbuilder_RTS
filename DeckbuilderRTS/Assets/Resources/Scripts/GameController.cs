using UnityEngine;
using System.Collections.ObjectModel;
using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] public Object FireballPrefab;
        [SerializeField] public Object WorkerPrefab;
        [SerializeField] public Object SwarmlingPrefab;
        [SerializeField] private bool SummonSwarmlings = true;
        [SerializeField] private bool IncreaseDifficulty = true;
        [SerializeField] public GameObject Miniboss1;
        [SerializeField] public GameObject Miniboss2;
        [SerializeField] public GameObject Miniboss3;

        private int GameDifficulty = 1;
        private float GameTimerMinutes = 0.0f;
        private float GameTimerSeconds = 0.0f;
        [SerializeField] private float SummonSwarmlingDelay = 10.0f;
        [SerializeField] private float IncreaseDifficultyDelay = 60.0f;
        private float CurrentIncreaseDifficultyDelay = 0.0f;
        private float CurrentSummonSwarmlingDelay = 0.0f;

        // The start function will initialize our member variables.
        public void Start()
        {

        }

        public void Update()
        {
            this.GameTimerSeconds += Time.deltaTime;
            this.CurrentSummonSwarmlingDelay += Time.deltaTime;
            this.CurrentIncreaseDifficultyDelay += Time.deltaTime;

            // Reset the seconds clock if a minute should be added. ~Jackson
            if (this.GameTimerSeconds >= 60.0f)
            {
                this.GameTimerSeconds -= 60.0f;
                this.GameTimerMinutes += 1.0f;
            }

            if (this.CurrentIncreaseDifficultyDelay >= this.IncreaseDifficultyDelay)
            {
                this.CurrentIncreaseDifficultyDelay -= this.IncreaseDifficultyDelay;

                if (this.IncreaseDifficulty)
                {
                    this.GameDifficulty += 1;
                }
            }

            if (this.CurrentSummonSwarmlingDelay >= this.SummonSwarmlingDelay)
            {
                this.CurrentSummonSwarmlingDelay -= this.SummonSwarmlingDelay;

                if (this.SummonSwarmlings)
                {
                    this.SummonNewSwarmling();
                }
            }
        }

        private GameObject GetRandomMiniboss()
        {
            var randomInt = Random.Range(0, 4);
            if (randomInt == 0)
            {
                return this.Miniboss1;
            }
            else if (randomInt == 1)
            {
                return this.Miniboss2;
            }
            else
            {
                return this.Miniboss3;
            }
        }

        private void SummonNewSwarmling()
        {
            var chosenMiniboss = this.GetRandomMiniboss();
            var summonLoc = chosenMiniboss.transform.position;
            var newSwarmling = Object.Instantiate(this.SwarmlingPrefab) as GameObject;
            newSwarmling.transform.position = summonLoc;
        }

        public ICard GenerateCardFireball()
        {
            Debug.Log("Created Fireball!");
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
