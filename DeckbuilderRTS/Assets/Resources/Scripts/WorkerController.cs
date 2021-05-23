using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class WorkerController : MonoBehaviour
    {
        private IUnitCommand CurrentCommand;
        [SerializeField] private GameObject Player;
        private PlayerController PlayerController;

        

        [SerializeField] private float GenerateMatterCoolDown = 5.0f;
        private float CurrentGenerateMatterCoolDown;
        [SerializeField] private int GenerateMatterAmount = 3;

        [SerializeField] private float GenerateEnergyCoolDown = 5.0f;
        private float CurrentGenerateEnergyCoolDown;
        [SerializeField] private int GenerateEnergyAmount = 3;

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
            
        }

        void Update()
        {
            if (this.Player != null)
            {
                transform.Rotate(Vector3.forward * (90 * Time.deltaTime));

                this.CurrentCommand.Execute(this.gameObject);
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
        }

        public bool IsWorkingMana()
        {
            return this.CurrentWorkingMode == WorkingMode.Mana;
        }

        public void SetWorkingMana()
        {
            this.CurrentWorkingMode = WorkingMode.Mana;
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

