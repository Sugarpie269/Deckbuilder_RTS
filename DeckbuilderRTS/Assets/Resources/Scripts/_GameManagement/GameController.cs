using UnityEngine;
using System.Collections.ObjectModel;
using DeckbuilderRTS;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace DeckbuilderRTS
{
    public class GameController : MonoBehaviour
    {
        // Attack Card Prefabs
        [SerializeField] public Object FireballPrefab;
        [SerializeField] public Object LeafbladePrefab;
        [SerializeField] public Object ForceBoltPrefab;
        [SerializeField] public Object IceSpikePrefab;
        [SerializeField] public Object LightningStrikerPrefab; // TODO 
        [SerializeField] public Object LaserBeamPrefab; // TODO 

        // Other Cards/Entities
        [SerializeField] public Object WorkerPrefab;
        [SerializeField] public Object SwarmlingPrefab;
        [SerializeField] public Object SlimelingPrefab;
        [SerializeField] private bool SummonSwarmlings = true;
        [SerializeField] private bool SummonSlimelings = true;
        [SerializeField] private bool IncreaseDifficulty = true;
        [SerializeField] public List<GameObject> MiniBosses;
        [SerializeField] public GameObject Boss;

        [SerializeField] private GameObject Player;

        public enum GameState { Menu, Playing, GameOver};
        [SerializeField] private GameState CurrentState = GameState.Playing;

        private int GameDifficulty = 1;
        private float GameTimerMinutes = 0.0f;
        private float GameTimerSeconds = 0.0f;
        [SerializeField] private float SummonSwarmlingDelay = 10.0f;
        [SerializeField] private float IncreaseDifficultyDelay = 60.0f;
        private float CurrentIncreaseDifficultyDelay = 0.0f;
        private float CurrentSummonSwarmlingDelay = 0.0f;

        private Collection<SwarmlingController> Swarmlings;
        private Collection<SlimelingController> Slimelings;
        private Collection<GameObject> Workers;
        
        [SerializeField] private float WorkerNoise = 5f;

        // The start function will initialize our member variables.
        public void Start()
        {
            this.Swarmlings = new Collection<SwarmlingController>();
            this.Slimelings = new Collection<SlimelingController>();
            this.Workers = new Collection<GameObject>();
        }

        public GameState GetCurrentState()
        {
            return this.CurrentState;
        }

        public void SummonNewWorker(GameObject player)
        {
            var newWorker = Object.Instantiate(this.WorkerPrefab) as GameObject;
            newWorker.transform.position = player.transform.position;
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), newWorker.GetComponent<BoxCollider2D>());
            var workerController = newWorker.GetComponent<WorkerController>();
            workerController.SetPlayer(player);
            this.Workers.Add(newWorker);
        }

        public void SetGameOver()
        {
            this.CurrentState = GameState.GameOver;
            if (this.Player != null)
            {
                var playerController = this.Player.GetComponent<PlayerController>();
                playerController.SetGameOver();
            }
            foreach (var controller in this.Swarmlings)
            {
                controller.SetDisabled();
            }
            foreach (var controller in this.Slimelings)
            {
                controller.SetDisabled();
            }
            var bossController = this.Boss.GetComponent<BossController>();
            bossController.SetDisabled();
        }

        private void SendWorkerAlerts()
        {
            var foundNull = false;
            var foundNullSwarmling = false;
            foreach (var worker in this.Workers)
            {
                if (worker == null)
                {
                    foundNull = true;
                    continue;
                }
                foreach (var swarmlingController in this.Swarmlings)
                {
                    if (swarmlingController == null)
                    {
                        foundNullSwarmling = true;
                        continue;
                    }
                    if (Vector3.Distance(swarmlingController.gameObject.transform.position, worker.transform.position) <= this.WorkerNoise)
                    {
                        swarmlingController.SetTarget(worker.transform);
                    }
                }
                foreach (var slimelingController in this.Slimelings)
                {
                    if (slimelingController == null)
                    {
                        foundNullSwarmling = true;
                        continue;
                    }
                    if (Vector3.Distance(slimelingController.gameObject.transform.position, worker.transform.position) <= this.WorkerNoise)
                    {
                        slimelingController.SetTarget(worker.transform);
                    }
                }
            }
            if (foundNull)
            {
                this.Workers.Remove(null);
            }
            if (foundNullSwarmling)
            {
                this.Swarmlings.Remove(null);
            }
        }

        public void Update()
        {
            if (this.CurrentState == GameState.GameOver)
            {
                return;
            }
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
                    //this.GameDifficulty += 1;

                    // Every difficulty increase tick, reduce the spawn delay of swarmlings by 1 second to a minimum of 5 seconds.
                    if (this.SummonSwarmlingDelay > 5)
                    {
                        this.SummonSwarmlingDelay -= 1;
                    }
                }
            }

            if (this.CurrentSummonSwarmlingDelay >= this.SummonSwarmlingDelay)
            {
                this.CurrentSummonSwarmlingDelay -= this.SummonSwarmlingDelay;
                if (this.SummonSwarmlings && this.SummonSlimelings)
                {
                    int randomInt = Random.Range(0, 8);
                    switch (randomInt)
                    {
                        case 0:
                            this.SummonNewSlimeling();
                            break;
                        default:
                            this.SummonNewSwarmling();
                            break;
                    }
                }
                else
                {
                    if (this.SummonSwarmlings)
                    {
                        this.SummonNewSwarmling();
                    }
                    if (this.SummonSlimelings)
                    {
                        this.SummonNewSlimeling();
                    }
                }
            }

            this.SendWorkerAlerts();
        }

        private GameObject GetRandomMiniboss()
        {
            var randomInt = Random.Range(0, this.MiniBosses.Count);
            return this.MiniBosses[randomInt];
        }

        private void SummonNewSwarmling()
        {
            var chosenMiniboss = this.GetRandomMiniboss();
            var summonLoc = chosenMiniboss.transform.position;
            var newSwarmling = Object.Instantiate(this.SwarmlingPrefab) as GameObject;
            newSwarmling.transform.position = summonLoc;
            var swarmlingController = newSwarmling.GetComponent<SwarmlingController>();
            swarmlingController.AddMaxHealth(this.GameDifficulty - 1);
            this.Swarmlings.Add(swarmlingController);
        }

        private void SummonNewSlimeling()
        {
            var chosenMiniboss = this.GetRandomMiniboss();
            var summonLoc = chosenMiniboss.transform.position;
            var newSlimeling = Object.Instantiate(this.SlimelingPrefab) as GameObject;
            newSlimeling.transform.position = summonLoc;
            var slimelingController = newSlimeling.GetComponent<SlimelingController>();
            slimelingController.AddMaxHealth(this.GameDifficulty - 1);
            this.Slimelings.Add(slimelingController);
        }

        public ICard GenerateCardFireball()
        {
            return new FireballCard(this.FireballPrefab);
        }

        public ICard GenerateCardVoid()
        {
            return new VoidCard();
        }

        public ICard GenerateCardInstantHeal()
        {
            return new InstantHealCard();
        }

        public ICard GenerateCardSummonWorker(GameObject player = null)
        {
            return new SummonWorkerCard(this.WorkerPrefab);
        }

        public ICard GenerateCardLeafblade()
        {
            return new LeafbladeCard(this.LeafbladePrefab);
        }

        public ICard GenerateCardIceSpike()
        {
            return new IceSpikeCard(this.IceSpikePrefab);
        }

        public ICard GenerateCardForceBolt()
        {
            return new ForceBoltCard(this.ForceBoltPrefab);
        }

        public ICard GenerateCardLaserBeam()
        {
            return new LaserBeamCard(this.LaserBeamPrefab);
        }

        public ICard GenerateCardLightningStriker()
        {
            return new LightningStrikerCard(this.LightningStrikerPrefab);
        }

        public CardInfo GetCardInfo(string cardName)
        {
            var cardInfo = new CardInfo();
            cardInfo.CardReference = GameObject.Find(cardName);
            if(!cardInfo.CardReference)
            {
                Debug.Log("Could not find CardInfo in library");
            }
            cardInfo.CardArt = cardInfo.CardReference.transform.GetChild(1).gameObject.GetComponent<Image>().sprite;
            cardInfo.CardName = cardInfo.CardReference.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            cardInfo.CardType = cardInfo.CardReference.transform.GetChild(3).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            cardInfo.DescriptionHeader = cardInfo.CardReference.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            cardInfo.DescriptionContent = cardInfo.CardReference.transform.GetChild(4).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
            cardInfo.FlavorText = cardInfo.CardReference.transform.GetChild(4).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text;
            cardInfo.CardLevel = int.Parse(cardInfo.CardReference.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text);
            cardInfo.CardPower = int.Parse(cardInfo.CardReference.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text);
            cardInfo.CardStrength = int.Parse(cardInfo.CardReference.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text);
            cardInfo.ManaCost = int.Parse(cardInfo.CardReference.transform.GetChild(8).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);
            cardInfo.EnergyCost = int.Parse(cardInfo.CardReference.transform.GetChild(8).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text);
            cardInfo.MatterCost = int.Parse(cardInfo.CardReference.transform.GetChild(8).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text);
            cardInfo.PlaySound = cardInfo.CardReference.transform.GetChild(9).gameObject.GetComponent<AudioSource>();
            return cardInfo;
        }
    }
}
