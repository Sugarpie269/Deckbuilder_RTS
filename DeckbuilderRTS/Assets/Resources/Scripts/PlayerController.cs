using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DeckbuilderRTS;

using System;

// Temporary citation = CaptainController.cs

namespace DeckbuilderRTS
{
    public class PlayerController : MonoBehaviour
    {
        // UI elements are as follows. ~Liam
        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI DrawCooldownText;
        public TextMeshProUGUI ManaText;
        public TextMeshProUGUI EnergyText;
        public TextMeshProUGUI MatterText;
        public GameObject VictoryText;
        public GameObject GameOverText;
        public GameObject LowHealthWarningText;
        public GameObject DrawCardErrorText;
        public GameObject CardSlot1ErrorText;
        public GameObject CardSlot2ErrorText;
        public GameObject CardSlot3ErrorText;
        public GameObject CardSlot1Image;
        public GameObject CardSlot2Image;
        public GameObject CardSlot3Image;
        public GameObject DrawSlotImage;
        public GameObject DiscardSlotImage;
        [SerializeField] private int PlayerCurrentHP;
        [SerializeField] private int PlayerMaxHP;
        [SerializeField] private int PlayerCurrentMana;
        [SerializeField] private int PlayerCurrentEnergy;
        [SerializeField] private int PlayerCurrentMatter;

        private Inventory PlayerInventory;
        private IPlayerCommand MoveUp;
        private IPlayerCommand MoveDown;
        private IPlayerCommand MoveLeft;
        private IPlayerCommand MoveRight;
        private IPlayerCommand DrawCard;
        private IPlayerCommand PlayCard1;
        private IPlayerCommand PlayCard2;
        private IPlayerCommand PlayCard3;

        private Texture2D EmptyCardSlotImage;
        private Texture2D FacedownCardImage;
        private float DrawCardCoolDown = 0.0f;
        private float DRAW_CARD_COOL_DOWN_BASE = 2.0f;
        private float PLAYER_ERROR_MESSAGE_DURATION = 1.5f;
        private float DrawErrorMessageDuration = 0.0f;
        private float Slot1ErrorMessageDuration = 0.0f;
        private float Slot2ErrorMessageDuration = 0.0f;
        private float Slot3ErrorMessageDuration = 0.0f;
        private int CurrentCooldownShown = 0;
        private bool IsGameOver = false;

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
            this.DrawCardErrorText.SetActive(false);
            this.CardSlot1ErrorText.SetActive(false);
            this.CardSlot2ErrorText.SetActive(false);
            this.CardSlot3ErrorText.SetActive(false);
            this.SetHealthText();
            this.SetDeckDrawCooldownText(0);
            this.SetManaText();
            this.SetEnergyText();
            this.SetMatterText();
            this.EmptyCardSlotImage = Resources.Load<Texture2D>("Sprites/EmptyCardSlot");
            this.FacedownCardImage = Resources.Load<Texture2D>("Sprites/FacedownCard");
        }

        // UI FUNCTION: Displays the victory text when called. Should only be called when the player has achieved victory, as such. ~Liam
        public void DisplayVictoryText()
        {
            this.VictoryText.SetActive(true);
        }

        // UI & PLAYER FUNCTION: Modifies the player's current Energy count by the paramter value. Passed in integer can be positive (for gaining) or negative (for spending). ~Liam
        // NOTE: This function will return true if the value passed in could be properly applied. However, if the value were to cause the energy to go below 0, then it will return false and fail to apply. ~Liam
        public bool ModifyPlayerEnergy(int amount)
        {
            // Check to ensure that if the amount is negative, the current energy count can handle it. ~Liam
            if (this.PlayerCurrentEnergy + amount >= 0)
            {
                this.PlayerCurrentEnergy += amount;
                this.SetEnergyText();
                return true;
            }
            return false;
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

        // UI & PLAYER FUNCTION: Modifies the player's current Mana count by the paramter value. Passed in integer can be positive (for gaining) or negative (for spending). ~Liam
        public bool ModifyPlayerMana(int amount)
        {
            // Check to ensure that if the amount is negative, the current mana count can handle it. ~Liam
            if (this.PlayerCurrentMana + amount >= 0)
            {
                this.PlayerCurrentMana += amount;
                this.SetManaText();
                return true;
            }
            return false;
        }

        // UI & PLAYER FUNCTION: Modifies the player's current Matter count by the paramter value. Passed in integer can be positive (for gaining) or negative (for spending). ~Liam
        public bool ModifyPlayerMatter(int amount)
        {
            // Check to ensure that if the amount is negative, the current matter count can handle it. ~Liam
            if (this.PlayerCurrentMatter + amount >= 0)
            {
                this.PlayerCurrentMatter += amount;
                this.SetMatterText();
                return true;
            }
            return false;
        }

        // Navya, this is all you!
        void ProcessInput()
        {
            // Input should only be recognized if the player has not died. ~Liam
            if (!this.IsGameOver)
            {
                // Temporary testing code for casting cards. ~Jackson
                if (Input.GetButtonDown("PlayCard1"))
                {
                    this.PlayerInventory.PlayCard1();
                }

                if (Input.GetButtonDown("PlayCard2"))
                {
                    this.PlayerInventory.PlayCard2();
                }

                if (Input.GetButtonDown("PlayCard3"))
                {
                    this.PlayerInventory.PlayCard3();
                }

                // Temporary code for drawing a card from the deck. ~Liam
                if (Input.GetButtonDown("Fire2"))
                {
                    // Only draw a card if there is not currently a cooldown. ~Liam
                    if (this.DrawCardCoolDown == 0)
                    {
                        // Set the draw cooldown if a card was drawn. ~Liam
                        if (this.PlayerInventory.DrawCard())
                        {
                            this.DrawCardCoolDown = this.DRAW_CARD_COOL_DOWN_BASE;
                        }
                    }
                    else
                    {
                        this.DrawErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
                        this.SetDrawErrorText(true);
                        this.PlayerInventory.SetDrawError(false);
                    }
                }

                
                //code for player movement
                if (Input.GetKey(KeyCode.A))
                {
                    this.MoveLeft.Execute(this.gameObject);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    this.MoveRight.Execute(this.gameObject);
                }

                if (Input.GetKey(KeyCode.W))
                {
                    this.MoveUp.Execute(this.gameObject);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    this.MoveDown.Execute(this.gameObject);
                }
            }
        }

        // UI FUNCTION: Updates the card image on slot 1 based on what is currently in the slot. ~Liam
        void SetCardSlot1Image()
        {
            // Get the image corresponding to the card in slot 1. If it is null, render the empty slot image instead. ~Liam
            Texture2D tempImage = this.PlayerInventory.GetCardSlot1Image();
            if (tempImage)
            {
                Sprite newImage = Sprite.Create(tempImage, new Rect(0f, 0f, tempImage.width, tempImage.height), new Vector2(0.5f, 0.5f));
                this.CardSlot1Image.GetComponent<Image>().overrideSprite = newImage;
            }
            else
            {
                Sprite newImage = Sprite.Create(this.EmptyCardSlotImage, new Rect(0f, 0f, this.EmptyCardSlotImage.width, this.EmptyCardSlotImage.height), new Vector2(this.EmptyCardSlotImage.width / 2, this.EmptyCardSlotImage.height / 2));
                this.CardSlot1Image.GetComponent<Image>().overrideSprite = newImage;
            }
        }

        // UI FUNCTION: Updates the card image on slot 2 based on what is currently in the slot. ~Liam
        void SetCardSlot2Image()
        {
            // Get the image corresponding to the card in slot 2. If it is null, render the empty slot image instead. ~Liam
            Texture2D tempImage = this.PlayerInventory.GetCardSlot2Image();
            if (tempImage)
            {
                Sprite newImage = Sprite.Create(tempImage, new Rect(0f, 0f, tempImage.width, tempImage.height), new Vector2(0.5f, 0.5f));
                this.CardSlot2Image.GetComponent<Image>().overrideSprite = newImage;
            }
            else
            {
                Sprite newImage = Sprite.Create(this.EmptyCardSlotImage, new Rect(0f, 0f, this.EmptyCardSlotImage.width, this.EmptyCardSlotImage.height), new Vector2(this.EmptyCardSlotImage.width / 2, this.EmptyCardSlotImage.height / 2));
                this.CardSlot2Image.GetComponent<Image>().overrideSprite = newImage;
            }
        }

        // UI FUNCTION: Updates the card image on slot 3 based on what is currently in the slot. ~Liam
        void SetCardSlot3Image()
        {
            // Get the image corresponding to the card in slot 3. If it is null, render the empty slot image instead. ~Liam
            Texture2D tempImage = this.PlayerInventory.GetCardSlot3Image();
            if (tempImage)
            {
                Sprite newImage = Sprite.Create(tempImage, new Rect(0f, 0f, tempImage.width, tempImage.height), new Vector2(0.5f, 0.5f));
                this.CardSlot3Image.GetComponent<Image>().overrideSprite = newImage;
            }
            else
            {
                Sprite newImage = Sprite.Create(this.EmptyCardSlotImage, new Rect(0f, 0f, this.EmptyCardSlotImage.width, this.EmptyCardSlotImage.height), new Vector2(this.EmptyCardSlotImage.width / 2, this.EmptyCardSlotImage.height / 2));
                this.CardSlot3Image.GetComponent<Image>().overrideSprite = newImage;
            }
        }

        // UI FUNCTION: Updates the discard pile image when called. Displays the card on top of the discard pile, or the empty slot if the pile is empty. ~Liam
        void SetDiscardSlotImage()
        {
            // Check if the discard pile has cards, or is empty, and set the image accordingly. ~Liam
            if (this.PlayerInventory.IsDiscardEmpty())
            {
                Sprite newImage = Sprite.Create(this.EmptyCardSlotImage, new Rect(0f, 0f, this.EmptyCardSlotImage.width, this.EmptyCardSlotImage.height), new Vector2(this.EmptyCardSlotImage.width / 2, this.EmptyCardSlotImage.height / 2));
                this.DiscardSlotImage.GetComponent<Image>().overrideSprite = newImage;
            }
            else
            {
                Texture2D tempImage = this.PlayerInventory.GetDiscardSlotImage();
                Sprite newImage = Sprite.Create(tempImage, new Rect(0f, 0f, tempImage.width, tempImage.height), new Vector2(tempImage.width / 2, tempImage.height / 2));
                this.DiscardSlotImage.GetComponent<Image>().overrideSprite = newImage;
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

        // UI FUNCTION: Updates the card image on the draw pile based on if there are cards in the deck. ~Liam
        void SetDrawSlotImage()
        {
            // Check if the draw pile has cards, or is empty, and set the image accordingly. ~Liam
            if (this.PlayerInventory.IsDeckEmpty())
            {
                Sprite newImage = Sprite.Create(this.EmptyCardSlotImage, new Rect(0f, 0f, this.EmptyCardSlotImage.width, this.EmptyCardSlotImage.height), new Vector2(this.EmptyCardSlotImage.width / 2, this.EmptyCardSlotImage.height / 2));
                this.DrawSlotImage.GetComponent<Image>().overrideSprite = newImage;
            }
            else
            {
                Sprite newImage = Sprite.Create(this.FacedownCardImage, new Rect(0f, 0f, this.FacedownCardImage.width, this.FacedownCardImage.height), new Vector2(this.FacedownCardImage.width / 2, this.FacedownCardImage.height / 2));
                this.DrawSlotImage.GetComponent<Image>().overrideSprite = newImage;
            }
        }

        // UI FUNCTION: Updates the energy text when called. ~Liam
        void SetEnergyText()
        {
            this.EnergyText.text = "Energy: " + this.PlayerCurrentEnergy.ToString();
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
                this.IsGameOver = true;
            }
        }

        // UI FUNCTION: Updates the mana text when called. ~Liam
        void SetManaText()
        {
            this.ManaText.text = "Mana:    " + this.PlayerCurrentMana.ToString();
        }

        // UI FUNCTION: Updates the matter text when called. ~Liam
        void SetMatterText()
        {
            this.MatterText.text = "Matter:  " + this.PlayerCurrentMatter.ToString();
        }

        // UI FUNCTION: Sets the various error messages for the card UI to active or inactive. ~Liam
        void SetDrawErrorText(bool value)
        {
            this.DrawCardErrorText.SetActive(value);
        }
        void SetCard1ErrorText(bool value)
        {
            this.CardSlot1ErrorText.SetActive(value);
        }
        void SetCard2ErrorText(bool value)
        {
            this.CardSlot2ErrorText.SetActive(value);
        }
        void SetCard3ErrorText(bool value)
        {
            this.CardSlot3ErrorText.SetActive(value);
        }

        public void TakeDamage(float damage)
        {
            this.ModifyPlayerHealth(Mathf.FloorToInt(damage));
            Debug.Log("I took " + damage.ToString() + " damage!");
        }

        public void ApplyHealing(float healing)
        {
            this.ModifyPlayerHealth(Mathf.FloorToInt(healing));
            Debug.Log("I healed " + healing.ToString() + " health!");
        }

        void Update()
        {
            if (!this.LoadedResources)
            {
                // Temporary code for testing fireball cards. ~Jackson
                var gameController = GameObject.Find("GameController");
                var fireballPrefab = gameController.GetComponent<GameController>().FireballPrefab;
                //this.PlayerInventory.AddCardSlot1(new FireballCard(fireballPrefab));
                this.PlayerInventory.AddCardSlot1(new InstantHealCard());
                this.PlayerInventory.AddCardSlot2(new InstantHealCard());
                this.PlayerInventory.AddCardSlot3(new InstantHealCard());

                this.PlayerInventory.GainCard(new FireballCard(fireballPrefab));
                this.PlayerInventory.GainCard(new InstantHealCard());
                this.PlayerInventory.GainCard(new FireballCard(fireballPrefab));

                this.LoadedResources = true;
            }

            // Check if any of the cards or error messages in the UI need updating, and do so if necessary. ~Liam
            if(this.PlayerInventory.GetCardSlot1Updated())
            {
                this.SetCardSlot1Image();
                this.PlayerInventory.SetCardSlot1Updated(false);
            }
            if (this.PlayerInventory.GetCardSlot2Updated())
            {
                this.SetCardSlot2Image();
                this.PlayerInventory.SetCardSlot2Updated(false);
            }
            if (this.PlayerInventory.GetCardSlot3Updated())
            {
                this.SetCardSlot3Image();
                this.PlayerInventory.SetCardSlot3Updated(false);
            }
            if (this.PlayerInventory.GetDrawSlotUpdated())
            {
                this.SetDrawSlotImage();
                this.PlayerInventory.SetDrawSlotUpdated(false);
            }
            if (this.PlayerInventory.GetDiscardSlotUpdated())
            {
                this.SetDiscardSlotImage();
                this.PlayerInventory.SetDiscardSlotUpdated(false);
            }
            if (this.PlayerInventory.GetErrorCardSlot1())
            {
                this.Slot1ErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
                this.SetCard1ErrorText(true);
                this.PlayerInventory.SetErrorCardSlot1(false);
            }
            if (this.PlayerInventory.GetErrorCardSlot2())
            {
                this.Slot2ErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
                this.SetCard2ErrorText(true);
                this.PlayerInventory.SetErrorCardSlot2(false);
            }
            if (this.PlayerInventory.GetErrorCardSlot3())
            {
                this.Slot3ErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
                this.SetCard3ErrorText(true);
                this.PlayerInventory.SetErrorCardSlot3(false);
            }
            if (this.PlayerInventory.GetDrawError())
            {
                this.DrawErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
                this.SetDrawErrorText(true);
                this.PlayerInventory.SetDrawError(false);
            }

            // If any error messages are currently on screen, decrement their duration. ~Liam
            if (this.Slot1ErrorMessageDuration > 0.0f)
            {
                this.Slot1ErrorMessageDuration -= Time.deltaTime;
                if(this.Slot1ErrorMessageDuration <= 0.0f)
                {
                    this.Slot1ErrorMessageDuration = 0.0f;
                    this.SetCard1ErrorText(false);
                }
            }
            if (this.Slot2ErrorMessageDuration > 0.0f)
            {
                this.Slot2ErrorMessageDuration -= Time.deltaTime;
                if (this.Slot2ErrorMessageDuration <= 0.0f)
                {
                    this.Slot2ErrorMessageDuration = 0.0f;
                    this.SetCard2ErrorText(false);
                }
            }
            if (this.Slot3ErrorMessageDuration > 0.0f)
            {
                this.Slot3ErrorMessageDuration -= Time.deltaTime;
                if (this.Slot3ErrorMessageDuration <= 0.0f)
                {
                    this.Slot3ErrorMessageDuration = 0.0f;
                    this.SetCard3ErrorText(false);
                }
            }
            if (this.DrawErrorMessageDuration > 0.0f)
            {
                this.DrawErrorMessageDuration -= Time.deltaTime;
                if (this.DrawErrorMessageDuration <= 0.0f)
                {
                    this.DrawErrorMessageDuration = 0.0f;
                    this.SetDrawErrorText(false);
                }
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
