using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class WorkerController : MonoBehaviour
    {
        private IUnitCommand CurrentCommand;
        [SerializeField] private GameObject Player;
        private PlayerController PlayerController;
        [SerializeField] private int MaxHealth = 1;
        private int CurrentHealth;

        private enum WorkingMode{BasicMatter, Matter, Energy, Mana};

        private WorkingMode CurrentWorkingMode = WorkingMode.BasicMatter;
                
        void Start()
        {
            // this.CurrentCommand = ScriptableObject.CreateInstance<???>();
            if (this.Player != null)
            {
                this.PlayerController = this.Player.GetComponent<PlayerController>();
            }
            this.CurrentCommand = new WorkBasicMatterCommand();
            this.CurrentHealth = this.MaxHealth;
            
        }

        void Update()
        {
            if (this.Player != null && !(this.CurrentWorkingMode == WorkingMode.BasicMatter))
            {
                transform.Rotate(Vector3.forward * (90 * Time.deltaTime));

                this.CurrentCommand.Execute(this.gameObject);
            }
            
        }

        public void TakeDamage(float damage)
        {
            this.CurrentHealth -= Mathf.FloorToInt(damage);
            if (this.CurrentHealth <= 0f)
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        public bool IsWorkingBasicMatter()
        {
            return this.CurrentWorkingMode == WorkingMode.BasicMatter;
        }

        public bool IsWorkingMatter()
        {
            return this.CurrentWorkingMode == WorkingMode.Matter;
        }

        public void SetWorkingMatter()
        {
            this.CurrentWorkingMode = WorkingMode.Matter;
            this.CurrentCommand = new WorkMatterCommand();
        }

        public bool IsWorkingEnergy()
        {
            return this.CurrentWorkingMode == WorkingMode.Energy;
        }

        public void SetWorkingEnergy()
        {
            this.CurrentWorkingMode = WorkingMode.Energy;
            this.CurrentCommand = new WorkEnergyCommand();
        }

        public void SetWorkingBasic()
        {
            this.CurrentWorkingMode = WorkingMode.BasicMatter;
            this.CurrentCommand = new WorkBasicMatterCommand();
        }

        public bool IsWorkingMana()
        {
            return this.CurrentWorkingMode == WorkingMode.Mana;
        }

        public void SetWorkingMana()
        {
            this.CurrentWorkingMode = WorkingMode.Mana;
            this.CurrentCommand = new WorkManaCommand();
        }

        public PlayerController GetPlayerController()
        {
            return this.PlayerController;
        }

        public void SetPlayer(GameObject player)
        {
            this.Player = player;
            this.PlayerController = this.Player.GetComponent<PlayerController>();
        }
    }
}

