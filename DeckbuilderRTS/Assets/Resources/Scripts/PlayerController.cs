using UnityEngine;
using TMPro;
using DeckbuilderRTS;

// Temporary citation = CaptainController.cs

namespace DeckbuilderRTS
{
    public class PlayerController : MonoBehaviour
    {
        // UI elements are as follows. ~Liam
        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI DrawCooldownText;
        public GameObject VictoryText;
        public GameObject GameOverText;
        public GameObject LowHealthWarningText;
        [SerializeField] private int PlayerCurrentHP;
        [SerializeField] private int PlayerMaxHP;

        private Inventory PlayerInventory;
        private IPlayerCommand MoveUp;
        private IPlayerCommand MoveDown;
        private IPlayerCommand MoveLeft;
        private IPlayerCommand MoveRight;
        private IPlayerCommand DrawCard;
        private IPlayerCommand PlayCard1;
        private IPlayerCommand PlayCard2;
        private IPlayerCommand PlayCard3;

        private float DrawCardCoolDown = 0.0f;
        private float DRAW_CARD_COOL_DOWN_BASE = 5.0f;
        private int CurrentCooldownShown = 0;

        private bool LoadedResources = false;

        // NOTE: All methods are listed in alphabetical order with the exception of Start().

        // The start function will initialize our member variables.
        void Start()
        {
            this.PlayerInventory = this.gameObject.GetComponent<Inventory>();
            this.MoveUp = ScriptableObject.CreateInstance<MovePlayerUpCommand>();
            this.MoveDown = ScriptableObject.CreateInstance<MovePlayerDownCommand>();
            this.MoveLeft = ScriptableObject.CreateInstance<MovePlayerLeftCommand>();
            this.MoveRight = ScriptableObject.CreateInstance<MovePlayerRightCommand>();
            this.DrawCard = ScriptableObject.CreateInstance<PlayerDrawCardCommand>();
            this.PlayCard1 = ScriptableObject.CreateInstance<PlayerCard1Command>();
            this.PlayCard2 = ScriptableObject.CreateInstance<PlayerCard2Command>();
            this.PlayCard3 = ScriptableObject.CreateInstance<PlayerCard3Command>();

            

            // Initialize UI elements, player health, and player deck + cards. ~Liam
            this.VictoryText.SetActive(false);
            this.LowHealthWarningText.SetActive(false);
            this.GameOverText.SetActive(false);
            this.SetHealthText();
            this.SetDeckDrawCooldownText(0);
        }

        // UI FUNCTION: Displays the victory text when called. Should only be called when the player has achieved victory, as such. ~Liam
        public void DisplayVictoryText()
        {
            this.VictoryText.SetActive(true);
        }

        // UI & PLAYER FUNCTION: Modifies the player's current health by the parameter value. Passed in integer can be positive (for healing) or negative (for damage). ~Liam
        public void ModifyPlayerHealth(int amount)
        {
            // Add the parameter to the player's current health, keeping in mind max and min health values. ~Liam
            if (this.PlayerCurrentHP + amount > this.PlayerMaxHP)
            {
                this.PlayerCurrentHP = this.PlayerMaxHP;
            }
            else if (this.PlayerCurrentHP + amount < 0)
            {
                this.PlayerCurrentHP = 0;
            }
            else
            {
                this.PlayerCurrentHP = this.PlayerCurrentHP + amount;
            }

            // Update the UI health value. ~Liam
            this.SetHealthText();
        }

        // Navya, this is all you!
        void ProcessInput()
        {
            // Temporary testing code for casting cards. ~Jackson
            if (Input.GetButtonDown("Fire1"))
            {
                this.PlayerInventory.PlayCard1();
            }

            // Temporary code for drawing a card from the deck. ~Liam
            if (Input.GetButtonDown("Fire2"))
            {
                // Only draw a card if there is not currently a cooldown. ~Liam
                if(this.DrawCardCoolDown == 0)
                {
                    // Draw a card! This is placeholder text for the function call later. ~Liam
                    // Set the draw cooldown. ~Liam
                    this.DrawCardCoolDown = this.DRAW_CARD_COOL_DOWN_BASE;
                }
            }
        }

        // UI FUNCTION: Updates the draw cooldown text when called. Displays an int value between 1 and the max cooldown; if off cooldown, no number is displayed. ~Liam
        void SetDeckDrawCooldownText(int cooldown)
        {
            if(cooldown > 0 && cooldown <= (int)this.DRAW_CARD_COOL_DOWN_BASE)
            {
                // Update the cooldown text. ~Liam
                this.DrawCooldownText.text = cooldown.ToString();
            }
            else if (cooldown == 0)
            {
                this.DrawCooldownText.text = "";
            }
            else
            {
                Debug.Log("ERROR: Invalid cooldown display value requested.");
            }
        }

        // UI FUNCTION: Updates the health text when called. Also governs whether or not low HP or game over text is displayed. ~Liam
        void SetHealthText()
        {
            // Update the HP text. ~Liam
            this.HealthText.text = "HP: " + this.PlayerCurrentHP.ToString() + "/" + this.PlayerMaxHP.ToString();

            // Warn the player if their health drops below 25% of the maximum. ~Liam
            float percentHP = (float)this.PlayerCurrentHP / (float)this.PlayerMaxHP;
            Debug.Log(percentHP);
            if (percentHP <= 0.25f && this.PlayerCurrentHP != 0)
            {
                this.LowHealthWarningText.SetActive(true);
            }
            else
            {
                this.LowHealthWarningText.SetActive(false);
            }
            // If player's HP drops to 0, they die, and the UI should reflect this. ~Liam
            if (this.PlayerCurrentHP == 0)
            {
                this.GameOverText.SetActive(true);
            }
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("I took " + damage.ToString() + " damage!");
        }

        void Update()
        {
            if (!this.LoadedResources)
            {
                // Temporary code for testing fireball cards. ~Jackson
                var gameController = GameObject.Find("GameController");
                var fireballPrefab = gameController.GetComponent<GameController>().FireballPrefab;
                this.PlayerInventory.AddCardSlot1(new FireballCard(fireballPrefab));

                this.LoadedResources = true;
            }
            this.ProcessInput();

            // If the deck is on cooldown, update it.
            if (this.DrawCardCoolDown > 0.0f)
            {
                this.DrawCardCoolDown -= Time.deltaTime;
            }
            // If updating the cooldown set it to less than zero, fix it back to zero.
            if (this.DrawCardCoolDown < 0.0f)
            {
                this.DrawCardCoolDown = 0.0f;
            }

            var tempCooldown = Mathf.CeilToInt(this.DrawCardCoolDown);

            // When the cooldown timer reaches a new second threshold, update the UI. ~Liam
            if (this.CurrentCooldownShown != tempCooldown)
            {
                SetDeckDrawCooldownText(tempCooldown);
                this.CurrentCooldownShown = tempCooldown;
            }            
        }
    }
}
