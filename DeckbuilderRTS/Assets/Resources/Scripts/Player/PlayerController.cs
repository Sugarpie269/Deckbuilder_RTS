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
        [SerializeField] private GameObject GameController;

        private Inventory PlayerInventory;
        private IPlayerCommand MoveUp;
        private IPlayerCommand MoveDown;
        private IPlayerCommand MoveLeft;
        private IPlayerCommand MoveRight;
        private IPlayerCommand DrawCard;
        private IPlayerCommand PlayCard1;
        private IPlayerCommand PlayCard2;
        private IPlayerCommand PlayCard3;

        // FIXME: Add option for speed changes?
        [SerializeField] private float Speed = 5.0f;

        //private enum MovementADSR {None, Attack, Sustain, Release};
        //private float CurrentMovementAttackTime = 0.0f;
        [SerializeField] private float MaxMovementAttackTime = 1f;
        //private float CurrentMovementReleaseTime = 0.0f;
        [SerializeField] private float MaxMovementReleaseTime = 0.1f;
        private PlayerADSR HorizontalADSR = null;
        private PlayerADSR VerticalADSR = null;
        //private MovementADSR CurrentMovementMode = MovementADSR.None;
        //private Vector2 LastDirectionVec = new Vector2(0.0f, 0.0f);

        private Sprite EmptyCardSlotImage;
        private Sprite FacedownCardImage;
        private Sprite BlankCardTemplate;
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

            this.HorizontalADSR = new PlayerADSR(this.MaxMovementAttackTime, this.MaxMovementReleaseTime);
            this.VerticalADSR = new PlayerADSR(this.MaxMovementAttackTime, this.MaxMovementReleaseTime);
            
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
            Texture2D tempEmptyCardSlot = Resources.Load<Texture2D>("Sprites/EmptyCardSlot");
            this.EmptyCardSlotImage = Sprite.Create(tempEmptyCardSlot, new Rect(0f, 0f, tempEmptyCardSlot.width, tempEmptyCardSlot.height), new Vector2(tempEmptyCardSlot.width / 2, tempEmptyCardSlot.height / 2));
            Texture2D tempFacedownCard = Resources.Load<Texture2D>("Sprites/FacedownCard");
            this.FacedownCardImage = Sprite.Create(tempFacedownCard, new Rect(0f, 0f, tempFacedownCard.width, tempFacedownCard.height), new Vector2(tempFacedownCard.width / 2, tempFacedownCard.height / 2));
            Texture2D tempBlankCard = Resources.Load<Texture2D>("Sprites/card_base_1000x1500");
        }

        private Vector2 GetPlayerSpeed()
        {
            float horiSpeed = 0.0f;
            float vertiSpeed = 0.0f;
            if (this.HorizontalADSR != null)
            {
                horiSpeed = this.HorizontalADSR.GetSpeedModifier() * this.Speed;
            }
            if (this.VerticalADSR != null)
            {
                vertiSpeed = this.VerticalADSR.GetSpeedModifier() * this.Speed;
            }
            return new Vector2(horiSpeed, vertiSpeed);
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

            Debug.Log("Modified health to " + this.PlayerCurrentHP);
            // Update the UI health value. ~Liam
            this.SetHealthText();
        }

        // UI & PLAYER FUNCTION: Modifies the player's current Mana count by the paramter value. Passed in integer can be positive (for gaining) or negative (for spending).
        // Returns true if modification was applied, false if not (e.g. when the mana cost of something is higher than the player's reserves). ~Liam
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

        // UI & PLAYER FUNCTION: Modifies the player's current Matter count by the parameter value. Passed in integer can be positive (for gaining) or negative (for spending).
        // Returns true if modification was applied, false if not (e.g. when the matter cost of something is higher than the player's reserves). ~Liam
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

        // The function gets the world mouse position and gets the direction.
        public Vector2 GetMousePosition() {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)((worldMousePos - transform.position));
            direction.Normalize();
            return direction;
        }

      

        // Inputs for the game
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


                
                /*
                // Code for player movement.
                if (Input.GetKey(KeyCode.A))
                {
                    Debug.Log("Left!");
                    this.MoveLeft.Execute(this.gameObject);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    this.MoveRight.Execute(this.gameObject);
                }

                if (Input.GetKey(KeyCode.W))
                {
                    Debug.Log("Up!");
                    this.MoveUp.Execute(this.gameObject);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    this.MoveDown.Execute(this.gameObject);
                }
                */
                

                // Movement using WASD Keys. 
                // Movement is implemented in this file to allow for simultaneous horizontal and vertical movment
                // instead of separate horizontal or vertical movement in individual movement scripts.
                Vector2 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                // Jackson's ADSR code handles the speed modifiers.~Jackson
                this.HorizontalADSR.Update(movement.x, Time.fixedDeltaTime);
                this.VerticalADSR.Update(movement.y, Time.fixedDeltaTime);
                

                var rigidBody = gameObject.GetComponent<Rigidbody2D>();
                rigidBody.MovePosition(rigidBody.position + this.GetPlayerSpeed() * Time.fixedDeltaTime);
            }
        }

        // UI FUNCTION: Renders the card on slot 1 based on what is currently in the slot. The card is modular and individual parts can be tweaked as needed. ~Liam
        void RenderCardSlot1()
        {
            Sprite tempArt = this.PlayerInventory.GetCardSlot1Image();
            /* 
             * NOTE FOR TOMORROW: Implementing this is basically gonna require overhauling the entire way in which the UI displays cards. Here's the steps I'll need:
             * 
             * 0. Re-implement the CardDisplayLibrary object in the Canvas (or just in the game, that's probably fine too. It'll store each card with the hierarchy of the Card_Fireball in the zPrefab scene.
             * 
             * 1. Does the card slot have a card in it? If so, go to 2. Otherwise, render the blank card sprite by going to step A (will hopefully be some transparent outline of a card) and return.
             * 
             * 2. Load the blank card template sprite (empty image/card name/etc.) into the Canvas->Hand->Cards->CardX. The gameobject hierarchy in CardX should be 
             *    identical to that of the Card_Fireball object.
             * 
             * 3. Load the cardArt sprite into the transparent portion of the template sprite. To do this, each Card script will need to have a [SerializeField] gameobject
             *    for their corresponding card in the CardDisplayLibrary, and the GetImage() functions should be reworked to return the gameobject instead of a sprite. Then
             *    this function can just call GetComponent<>() for the various image and text inputs.
             *    
             * 4. You'll need to use trial and error to determine where to place the TextMeshProUGUIs on the card such that they line up properly, for each slot on the screen.
             * 
             * A. Load the empty card slot image to the slot, and deactivate any related text in the slot.
             * 
             * There's probably more I'm forgetting, but that's all for now. This will have to be done manually for each of the card slots. Painstaking but worth it in the end.
             * NOTE: This process will probably also need to be used for the "Hold ALT to examine" feature, when rendering the card in the center of the screen.
            */
        }

        // UI FUNCTION: Updates the card image on slot 1 based on what is currently in the slot. ~Liam
        void SetCardSlot1Image()
        {
            // Get the image corresponding to the card in slot 1. If it is null, render the empty slot image instead. ~Liam
            Sprite tempImage = this.PlayerInventory.GetCardSlot1Image();
            if (tempImage)
            {
                this.CardSlot1Image.GetComponent<Image>().overrideSprite = tempImage;
            }
            else
            {
                this.CardSlot1Image.GetComponent<Image>().overrideSprite = this.EmptyCardSlotImage;
            }
        }

        // UI FUNCTION: Updates the card image on slot 2 based on what is currently in the slot. ~Liam
        void SetCardSlot2Image()
        {
            // Get the image corresponding to the card in slot 2. If it is null, render the empty slot image instead. ~Liam
            Sprite tempImage = this.PlayerInventory.GetCardSlot2Image();
            if (tempImage)
            {
                this.CardSlot2Image.GetComponent<Image>().overrideSprite = tempImage;
            }
            else
            {
                this.CardSlot2Image.GetComponent<Image>().overrideSprite = this.EmptyCardSlotImage;
            }
        }

        // UI FUNCTION: Updates the card image on slot 3 based on what is currently in the slot. ~Liam
        void SetCardSlot3Image()
        {
            // Get the image corresponding to the card in slot 3. If it is null, render the empty slot image instead. ~Liam
            Sprite tempImage = this.PlayerInventory.GetCardSlot3Image();
            if (tempImage)
            {
                this.CardSlot3Image.GetComponent<Image>().overrideSprite = tempImage;
            }
            else
            {
                this.CardSlot3Image.GetComponent<Image>().overrideSprite = this.EmptyCardSlotImage;
            }
        }

        // UI FUNCTION: Updates the discard pile image when called. Displays the card on top of the discard pile, or the empty slot if the pile is empty. ~Liam
        void SetDiscardSlotImage()
        {
            // Check if the discard pile has cards, or is empty, and set the image accordingly. ~Liam
            if (this.PlayerInventory.IsDiscardEmpty())
            {
                this.DiscardSlotImage.GetComponent<Image>().overrideSprite = this.EmptyCardSlotImage;
            }
            else
            {
                this.DiscardSlotImage.GetComponent<Image>().overrideSprite = this.PlayerInventory.GetDiscardSlotImage();
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
                this.DrawSlotImage.GetComponent<Image>().overrideSprite = this.EmptyCardSlotImage;
            }
            else
            {
                this.DrawSlotImage.GetComponent<Image>().overrideSprite = this.FacedownCardImage;
            }
        }

        

        // UI FUNCTION: Updates the health text when called. Also governs whether or not low HP or game over text is displayed. ~Liam
        void SetHealthText()
        {
            // Update the HP text. ~Liam
            this.HealthText.text = "HP: " + this.PlayerCurrentHP.ToString() + "/" + this.PlayerMaxHP.ToString();

            // Warn the player if their health drops below 25% of the maximum. ~Liam
            float percentHP = (float)this.PlayerCurrentHP / (float)this.PlayerMaxHP;
            Debug.Log("Current HP " + this.PlayerCurrentHP + " vs max " + this.PlayerMaxHP);
            Debug.Log("Health Percentage:" + percentHP);
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
                Debug.Log("GAME OVER!");
                this.GameOverText.SetActive(true);
                this.IsGameOver = true;
            }
        }

        // UI FUNCTION: Updates the mana text when called. ~Liam
        void SetManaText()
        {
            this.ManaText.text = "Mana:    " + this.PlayerCurrentMana.ToString();
        }

        // UI FUNCTION: Updates the energy text when called. ~Liam
        void SetEnergyText()
        {
            this.EnergyText.text = "Energy: " + this.PlayerCurrentEnergy.ToString();
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
            if (this.IsGameOver)
            {
                return;
            }
            this.ModifyPlayerHealth(Mathf.FloorToInt(damage * -1));
            Debug.Log("I took " + damage.ToString() + " damage!");
        }

        public void ApplyHealing(float healing)
        {
            if (this.IsGameOver)
            {
                return;
            }
            this.ModifyPlayerHealth(Mathf.FloorToInt(healing));
            Debug.Log("I healed " + healing.ToString() + " health!");
        }

        void Update()
        {
            if (!this.LoadedResources)
            {
                // Temporary code for testing fireball cards. ~Jackson
                /*var gameController = GameObject.Find("GameController");
                var fireballPrefab = gameController.GetComponent<GameController>().FireballPrefab;
                //this.PlayerInventory.AddCardSlot1(new FireballCard(fireballPrefab));
                this.PlayerInventory.AddCardSlot1(new InstantHealCard());
                this.PlayerInventory.AddCardSlot2(new InstantHealCard());
                this.PlayerInventory.AddCardSlot3(new InstantHealCard());

                this.PlayerInventory.GainCard(new FireballCard(fireballPrefab));
                //this.PlayerInventory.GainCard(new InstantHealCard());
                this.PlayerInventory.GainCard(new FireballCard(fireballPrefab));
                this.PlayerInventory.GainCard(new FireballCard(fireballPrefab));
                this.PlayerInventory.GainCard(new FireballCard(fireballPrefab));
                this.PlayerInventory.GainCard(new FireballCard(fireballPrefab));*/

                // Temporary code for starting hand/deck. This should be how cards are gained and created in future code. ~Jackson
                var gameControllerObject = this.GameController.GetComponent<GameController>();
                this.PlayerInventory.AddCardSlot1(gameControllerObject.GenerateCardFireball());
                this.PlayerInventory.AddCardSlot2(gameControllerObject.GenerateCardInstantHeal());
                this.PlayerInventory.AddCardSlot3(gameControllerObject.GenerateCardSummonWorker());

                this.PlayerInventory.GainCard(gameControllerObject.GenerateCardFireball());
                this.PlayerInventory.GainCard(gameControllerObject.GenerateCardFireball());

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

            // Update the Resource text (change to every so often or use Fixed update perhaps?)
            this.SetManaText();
            this.SetEnergyText();
            this.SetMatterText();
        }
    }
}
