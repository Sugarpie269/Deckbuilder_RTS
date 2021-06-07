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
        public TextMeshProUGUI HealthAddText;
        public TextMeshProUGUI HealthSubtractText;
        public TextMeshProUGUI DrawCooldownText;
        public TextMeshProUGUI PurchaseCooldownText;
        public GameObject PurchaseErrorText;
        public GameObject PurchaseSuccessText;
        public TextMeshProUGUI ManaText;
        public TextMeshProUGUI ManaAddText;
        public TextMeshProUGUI ManaSubtractText;
        public TextMeshProUGUI EnergyText;
        public TextMeshProUGUI EnergyAddText;
        public TextMeshProUGUI EnergySubtractText;
        public TextMeshProUGUI MatterText;
        public TextMeshProUGUI MatterAddText;
        public TextMeshProUGUI MatterSubtractText;
        public GameObject VictoryText;
        public GameObject GameOverText;
        public GameObject GameOverTipText;
        public int NumTips;
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
        public GameObject ExamineCardImage;
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
        private float PurchaseCardCooldown = 0.0f;
        public float PURCHASE_CARD_COOL_DOWN_BASE = 2.0f;
        private float PLAYER_ERROR_MESSAGE_DURATION = 1.5f;
        private float RESOURCE_UPDATE_MESSAGE_DURATION = 1.5f;
        private float DrawErrorMessageDuration = 0.0f;
        private float Slot1ErrorMessageDuration = 0.0f;
        private float Slot2ErrorMessageDuration = 0.0f;
        private float Slot3ErrorMessageDuration = 0.0f;
        private float PurchaseErrorMessageDuration = 0.0f;
        private float PurchaseSuccessMessageDuration = 0.0f;
        private float HealthUpdateMessageDuration = 0.0f;
        private float ManaUpdateMessageDuration = 0.0f;
        private float EnergyUpdateMessageDuration = 0.0f;
        private float MatterUpdateMessageDuration = 0.0f;
        private int CurrentCooldownShown = 0;
        private int CurrentPurchaseCooldownShown = 0;
        private bool IsGameOver = false;
        private bool PurchaseCooldown = false;
        private bool PurchaseSuccess = false;

        private bool LoadedResources = false;

        // Array of Market gameObjects for UI functionality. ~Liam
        private GameObject[] MarketList;

        // Audio clips used for UI sound effects. ~Liam
        [SerializeField] private GameObject DrawCardSound;
        [SerializeField] private GameObject ReplaceDiscardSound;
        [SerializeField] private GameObject PurchaseCardSound;
        [SerializeField] private GameObject GameOverFanfare;
        [SerializeField] private GameObject VictoryFanfare;
        [SerializeField] private GameObject ErrorSound;
        [SerializeField] private GameObject PlayerHurtSound;
        private AudioSource[] AudioSources;

        // Button objects for when the player wins or loses. ~Liam
        [SerializeField] private GameObject LoseButton;
        [SerializeField] private GameObject WinButton;

        public Vector2 CurrentDirection;

        private void Awake()
        {
            // Add all audio sources to a list. ~Liam
            AudioSources = FindObjectsOfType<AudioSource>();
        }

        // Shuts off all sounds currently playing. ~Liam
        private void StopAllSounds()
        {
            foreach (AudioSource audio in this.AudioSources)
            {
                audio.Stop();
            }
        }

        public void AddCard(ICard card) {
            this.PlayerInventory.GainCard(card);

            // Play audio corresponding to purchasing a card. ~Liam
            this.PurchaseCardSound.GetComponent<AudioSource>().Play();
        }
        public int GetMatter() {
            return this.PlayerCurrentMatter;
        }
        public int GetMana() {
            return this.PlayerCurrentMana;
        }
        public int GetEnergy() {
            return this.PlayerCurrentEnergy;
        }

        // These were used in MarketController but I updated it to use the previously integrated ModifyPlayerResource() functions. ~Liam
        /*
        public void DecMatter(int matter)
        {
            this.PlayerCurrentMatter = this.PlayerCurrentMatter - matter;
            this.SetMatterText();
        }
        public void DecMana(int mana)
        {
            this.PlayerCurrentMana = this.PlayerCurrentMana - mana;
            this.SetManaText();
        }
        public void DecEnery(int energy)
        {
            this.PlayerCurrentEnergy = this.PlayerCurrentEnergy - energy;
            this.SetEnergyText();
        }
        */
        public float GetCurrentSpeed()
        {
            var speedLen = this.GetPlayerSpeed().magnitude;
            return speedLen;
        }

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
            // Set the randomized game over tip text. ~Liam
            System.Random r = new System.Random();
            int textIndex = r.Next(0, this.NumTips - 1);
            this.GameOverTipText.GetComponent<TextMeshProUGUI>().text = GameObject.Find("DeathProtipLibrary").transform.GetChild(textIndex).gameObject.GetComponent<Text>().text;
            this.GameOverTipText.SetActive(false);
            this.DrawCardErrorText.SetActive(false);
            this.CardSlot1ErrorText.SetActive(false);
            this.CardSlot2ErrorText.SetActive(false);
            this.CardSlot3ErrorText.SetActive(false);
            this.PurchaseErrorText.SetActive(false);
            this.PurchaseSuccessText.SetActive(false);
            this.ExamineCardImage.SetActive(false);
            this.SetHealthText();
            this.SetDeckDrawCooldownText(0);
            this.SetPurchaseCooldownText(0);
            this.SetManaText();
            this.SetEnergyText();
            this.SetMatterText();

            // Set inactive the various resource modifier notifications. ~Liam
            this.HealthAddText.gameObject.SetActive(false);
            this.HealthSubtractText.gameObject.SetActive(false);
            this.ManaAddText.gameObject.SetActive(false);
            this.ManaSubtractText.gameObject.SetActive(false);
            this.EnergyAddText.gameObject.SetActive(false);
            this.EnergySubtractText.gameObject.SetActive(false);
            this.MatterAddText.gameObject.SetActive(false);
            this.MatterSubtractText.gameObject.SetActive(false);




            //Texture2D tempEmptyCardSlot = Resources.Load<Texture2D>("Sprites/EmptyCardSlot");
            this.EmptyCardSlotImage = GameObject.Find("EmptyCardSlot").GetComponent<Image>().sprite;
            //this.EmptyCardSlotImage = Sprite.Create(tempEmptyCardSlot, new Rect(0f, 0f, tempEmptyCardSlot.width, tempEmptyCardSlot.height), new Vector2(tempEmptyCardSlot.width / 2, tempEmptyCardSlot.height / 2));
            //Texture2D tempFacedownCard = Resources.Load<Texture2D>("Sprites/FacedownCard");
            this.FacedownCardImage = GameObject.Find("FacedownCard").GetComponent<Image>().sprite;
            //this.FacedownCardImage = Sprite.Create(tempFacedownCard, new Rect(0f, 0f, tempFacedownCard.width, tempFacedownCard.height), new Vector2(tempFacedownCard.width / 2, tempFacedownCard.height / 2));
            //Texture2D tempBlankCard = Resources.Load<Texture2D>("Sprites/card_base_1000x1500");
            //this.BlankCardTemplate = Sprite.Create(tempBlankCard, new Rect(0f, 0f, tempBlankCard.width, tempBlankCard.height), new Vector2(tempBlankCard.width / 2, tempBlankCard.height / 2));

            // Get all markets in the scene. ~Liam
            this.MarketList = GameObject.FindGameObjectsWithTag("Market");

            // Deactivate buttons. ~Liam
            this.LoseButton.SetActive(false);
            this.WinButton.SetActive(false);

            this.CurrentDirection = new Vector2(0f, 0f);
        }

        public bool RecentlyTookDamage()
        {
            return this.HealthSubtractText.IsActive();
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

            // Disable all previously playing sounds, and play the victory fanfare. ~Liam
            this.StopAllSounds();
            this.VictoryFanfare.GetComponent<AudioSource>().Play();

            // Disable controller input. ~Liam
            var controller = this.GameController.GetComponent<GameController>();
            controller.SetGameOver();

            // Disable tooltips in UI. ~Liam
            GameObject.Find("Card1").GetComponent<ExamineDisplay>().GameOver();
            GameObject.Find("Card2").GetComponent<ExamineDisplay>().GameOver();
            GameObject.Find("Card3").GetComponent<ExamineDisplay>().GameOver();
            GameObject.Find("DiscardPile").GetComponent<ExamineDisplay>().GameOver();
            this.ExamineCardImage.SetActive(false);

            // Display the button that loads the credits. ~Liam
            this.WinButton.SetActive(true);
        }

        // UI & PLAYER FUNCTION: Modifies the player's current Energy count by the paramter value. Passed in integer can be positive (for gaining) or negative (for spending). ~Liam
        // NOTE: This function will return true if the value passed in could be properly applied. However, if the value were to cause the energy to go below 0, then it will return false and fail to apply. ~Liam
        public bool ModifyPlayerEnergy(int amount)
        {
            // Check to ensure that if the amount is negative, the current energy count can handle it. ~Liam
            if (this.PlayerCurrentEnergy + amount >= 0)
            {
                this.PlayerCurrentEnergy += amount;

                // Update the UI energy value and display the proper update text. Make sure to remove previous update text if it is still on screen. ~Liam
                if (amount < 0)
                {
                    this.EnergyAddText.gameObject.SetActive(false);
                    this.EnergySubtractText.text = amount.ToString();
                    this.EnergySubtractText.gameObject.SetActive(true);
                }
                else
                {
                    this.EnergySubtractText.gameObject.SetActive(false);
                    this.EnergyAddText.text = "+" + amount.ToString();
                    this.EnergyAddText.gameObject.SetActive(true);
                }
                this.EnergyUpdateMessageDuration = this.RESOURCE_UPDATE_MESSAGE_DURATION;
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

            // Update the UI health value and display the proper update text. Make sure to remove previous update text if it is still on screen. ~Liam
            if (amount < 0)
            {
                this.HealthAddText.gameObject.SetActive(false);
                this.HealthSubtractText.text = amount.ToString();
                this.HealthSubtractText.gameObject.SetActive(true);
            }
            else
            {
                this.HealthSubtractText.gameObject.SetActive(false);
                this.HealthAddText.text = "+" + amount.ToString();
                this.HealthAddText.gameObject.SetActive(true);
            }
            this.HealthUpdateMessageDuration = this.RESOURCE_UPDATE_MESSAGE_DURATION;
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

                // Update the UI mana value and display the proper update text. Make sure to remove previous update text if it is still on screen. ~Liam
                if (amount < 0)
                {
                    this.ManaAddText.gameObject.SetActive(false);
                    this.ManaSubtractText.text = amount.ToString();
                    this.ManaSubtractText.gameObject.SetActive(true);
                }
                else
                {
                    this.ManaSubtractText.gameObject.SetActive(false);
                    this.ManaAddText.text = "+" + amount.ToString();
                    this.ManaAddText.gameObject.SetActive(true);
                }
                this.ManaUpdateMessageDuration = this.RESOURCE_UPDATE_MESSAGE_DURATION;
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

                // Update the UI matter value and display the proper update text. Make sure to remove previous update text if it is still on screen. ~Liam
                if (amount < 0)
                {
                    this.MatterAddText.gameObject.SetActive(false);
                    this.MatterSubtractText.text = amount.ToString();
                    this.MatterSubtractText.gameObject.SetActive(true);
                }
                else
                {
                    this.MatterSubtractText.gameObject.SetActive(false);
                    this.MatterAddText.text = "+" + amount.ToString();
                    this.MatterAddText.gameObject.SetActive(true);
                }
                this.MatterUpdateMessageDuration = this.RESOURCE_UPDATE_MESSAGE_DURATION;
                this.SetMatterText();
                return true;
            }
            return false;
        }

        // Called when the game is over via player failure. Displays proper text and shuts off most functionality. ~Liam
        private void GameOver()
        {
            // DEBUG
            //Debug.Log("GAME OVER!");

            // Disable controller input. ~Liam
            var controller = this.GameController.GetComponent<GameController>();
            controller.SetGameOver();

            // Disable all sounds, and play the game over fanfare. ~Liam
            this.StopAllSounds();
            this.GameOverFanfare.GetComponent<AudioSource>().Play();

            // Disable tooltips in UI. ~Liam
            GameObject.Find("Card1").GetComponent<ExamineDisplay>().GameOver();
            GameObject.Find("Card2").GetComponent<ExamineDisplay>().GameOver();
            GameObject.Find("Card3").GetComponent<ExamineDisplay>().GameOver();
            GameObject.Find("DiscardPile").GetComponent<ExamineDisplay>().GameOver();
            this.ExamineCardImage.SetActive(false);

            // Display game over text and lose button. ~Liam
            this.GameOverText.SetActive(true);
            this.GameOverTipText.SetActive(true);
            this.LoseButton.SetActive(true);
        }

        // The function gets the world mouse position and gets the direction.
        public Vector2 GetMousePosition() {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)((worldMousePos - transform.position));
            direction.Normalize();
            return direction;
        }

      
        private void UpdateRotation()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dirVec = new Vector2(mousePosition.x - this.gameObject.transform.position.x, mousePosition.y - this.gameObject.transform.position.y);

            var multiplier = 0f;
            
            if (dirVec.x > 0)
            {
                multiplier = 1f;
            }
            if (dirVec.x != 0)
            {
                transform.eulerAngles = new Vector3(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, multiplier * 180f + (180 / Mathf.PI) * Mathf.Atan(dirVec.y / dirVec.x) + 90);
            }
            
            else if (dirVec.y > 0)
            {
                transform.eulerAngles = new Vector3(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, 180);
            }
            
        }

        // Inputs for the game
        void ProcessInput()
        {
            // Input should only be recognized if the player has not died. ~Liam
            if (!this.IsGameOver)
            {
                // Basic input code for casting cards. ~Jackson
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

                // Code for drawing a card from the deck. ~Liam
                if (Input.GetButtonDown("Fire2"))
                {
                    // Only draw a card if there is not currently a cooldown. ~Liam
                    if (this.DrawCardCoolDown == 0)
                    {
                        // Set the draw cooldown if a card was drawn. ~Liam
                        if (this.PlayerInventory.DrawCard())
                        {
                            // Play audio for drawing a card. ~Liam
                            this.DrawCardSound.GetComponent<AudioSource>().Play();

                            this.DrawCardCoolDown = this.DRAW_CARD_COOL_DOWN_BASE;
                        }
                        else if (!this.PlayerInventory.GetDrawError())
                        {
                            // Play audio for replacing the deck. ~Liam
                            this.ReplaceDiscardSound.GetComponent<AudioSource>().Play();
                        }
                    }
                    else
                    {
                        // Play audio for attempting an invalid command. ~Liam
                        this.ErrorSound.GetComponent<AudioSource>().Play();

                        this.DrawErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
                        this.SetDrawErrorText(true);
                        this.PlayerInventory.SetDrawError(false);
                    }
                }

                if (Input.GetButtonDown("Jump"))
                {
                    var camera = GameObject.Find("Main Camera");
                    var cameraController = camera.GetComponent<CameraController>();
                    cameraController.ToggleCameraControl();
                }
                /*else if (Input.GetButtonUp("Fire1"))
                {
                    var camera = GameObject.Find("Main Camera");
                    var cameraController = camera.GetComponent<CameraController>();
                    cameraController.ClearCameraControl();
                }*/

                // Code for examining a card in the player's UI, or the market. ~Liam
                if (Input.GetButtonDown("ExamineCard"))
                {
                    CardInfo tempCInfo = new CardInfo();

                    // Check what card the player is hovering over. ~Liam
                    if (GameObject.Find("Card1").GetComponent<ExamineDisplay>().IsPointerHovering())
                    {
                        if (this.PlayerInventory.GetCardSlot1Info(ref tempCInfo))
                        {
                            this.RenderExamineCard(tempCInfo);
                        }
                    }
                    else if (GameObject.Find("Card2").GetComponent<ExamineDisplay>().IsPointerHovering())
                    {
                        if (this.PlayerInventory.GetCardSlot2Info(ref tempCInfo))
                        {
                            this.RenderExamineCard(tempCInfo);
                        }
                    }
                    else if (GameObject.Find("Card3").GetComponent<ExamineDisplay>().IsPointerHovering())
                    {
                        if (this.PlayerInventory.GetCardSlot3Info(ref tempCInfo))
                        {
                            this.RenderExamineCard(tempCInfo);
                        }
                    }
                    else if (GameObject.Find("DiscardPile").GetComponent<ExamineDisplay>().IsPointerHovering())
                    {
                        if (this.PlayerInventory.GetDiscardSlotImageInfo(ref tempCInfo))
                        {
                            this.RenderExamineCard(tempCInfo);
                        }
                    }
                    // If the player mouse is not over any UI object, check if they are over any market. ~Liam
                    else
                    {
                        for (int i = 0; i < this.MarketList.Length; i++)
                        {
                            if (this.MarketList[i].GetComponent<MarketController>().IsPointerHovering())
                            {
                                this.RenderExamineCard(this.MarketList[i].GetComponent<MarketController>().GetMarketCardInfo());
                                break;
                            }
                        }
                    }
                }

                if (Input.GetButtonUp("ExamineCard"))
                {
                    // Set the high quality card image to inactive when the player releases the key. ~Liam
                    this.ExamineCardImage.SetActive(false);
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
                this.HorizontalADSR.Update(movement.x, Time.fixedDeltaTime, this.VerticalADSR.IsSustaining());
                /*if (this.HorizontalADSR.IsSustaining() == false)
                {
                    Debug.Log("this ran " + Time.deltaTime.ToString());
                }*/
                this.VerticalADSR.Update(movement.y, Time.fixedDeltaTime, this.HorizontalADSR.IsSustaining());


                var rigidBody = gameObject.GetComponent<Rigidbody2D>();
                rigidBody.MovePosition(rigidBody.position + this.GetPlayerSpeed() * Time.fixedDeltaTime);


                //this.UpdateRotation();

            }
        }

        private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
        {
            Vector2 diference = vec2 - vec1;
            float sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
            return Vector2.Angle(Vector2.right, diference) * sign;
        }

        public static Vector2 rotate(Vector2 v, float delta) {
            delta = delta * Mathf.Rad2Deg;
            //Debug.Log("Delta is " + delta + " v is " + v);
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }

        // UI FUNCTION: Renders the card that is attempting to be examined more closely by the player. ~Liam
        void RenderExamineCard(CardInfo cINfo)
        {

            //Debug.Log("Attempted to examine card " + cINfo.CardName);

            // Set the examine card's properties to that of the passed-in CardInfo struct. ~Liam
            this.ExamineCardImage.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = cINfo.CardArt;
            this.ExamineCardImage.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.CardName;
            this.ExamineCardImage.transform.GetChild(3).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.CardType;
            this.ExamineCardImage.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.DescriptionHeader;
            this.ExamineCardImage.transform.GetChild(4).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.DescriptionContent;
            this.ExamineCardImage.transform.GetChild(4).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.FlavorText;
            this.ExamineCardImage.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.CardLevel.ToString();
            this.ExamineCardImage.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.CardPower.ToString();
            this.ExamineCardImage.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.CardStrength.ToString();
            this.ExamineCardImage.transform.GetChild(8).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.ManaCost.ToString();
            this.ExamineCardImage.transform.GetChild(8).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.EnergyCost.ToString();
            this.ExamineCardImage.transform.GetChild(8).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = cINfo.MatterCost.ToString();

            // Now that all the card info is in place, render the examined card. ~Liam
            this.ExamineCardImage.SetActive(true);
        }

        // UI FUNCTION: Updates the card image on slot 1 based on what is currently in the slot. ~Liam
        void SetCardSlot1Image()
        {
            // Get the CardInfo corresponding to the card in slot 1. If it is null, render the empty slot image instead. ~Liam
            CardInfo tempCInfo = new CardInfo();
            if (this.PlayerInventory.GetCardSlot1Info(ref tempCInfo))
            {
                // Render the various aspects of the card to the proper slot. ~Liam
                this.CardSlot1Image.transform.GetChild(0).GetChild(1).GetComponent<Image>().overrideSprite = tempCInfo.CardArt;
                this.CardSlot1Image.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = tempCInfo.CardName;
                this.CardSlot1Image.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = tempCInfo.CardLevel.ToString();

                // Set the card image to active. ~Liam
                this.CardSlot1Image.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                this.CardSlot1Image.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        // UI FUNCTION: Updates the card image on slot 1 based on what is currently in the slot. ~Liam
        void SetCardSlot2Image()
        {
            // Get the CardInfo corresponding to the card in slot 1. If it is null, render the empty slot image instead. ~Liam
            CardInfo tempCInfo = new CardInfo();
            if (this.PlayerInventory.GetCardSlot2Info(ref tempCInfo))
            {
                // Render the various aspects of the card to the proper slot. ~Liam
                this.CardSlot2Image.transform.GetChild(0).GetChild(1).GetComponent<Image>().overrideSprite = tempCInfo.CardArt;
                this.CardSlot2Image.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = tempCInfo.CardName;
                this.CardSlot2Image.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = tempCInfo.CardLevel.ToString();

                // Set the card image to active. ~Liam
                this.CardSlot2Image.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                this.CardSlot2Image.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        // UI FUNCTION: Updates the card image on slot 1 based on what is currently in the slot. ~Liam
        void SetCardSlot3Image()
        {
            // Get the CardInfo corresponding to the card in slot 1. If it is null, render the empty slot image instead. ~Liam
            CardInfo tempCInfo = new CardInfo();
            if (this.PlayerInventory.GetCardSlot3Info(ref tempCInfo))
            {
                // Render the various aspects of the card to the proper slot. ~Liam
                this.CardSlot3Image.transform.GetChild(0).GetChild(1).GetComponent<Image>().overrideSprite = tempCInfo.CardArt;
                this.CardSlot3Image.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = tempCInfo.CardName;
                this.CardSlot3Image.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = tempCInfo.CardLevel.ToString();

                // Set the card image to active. ~Liam
                this.CardSlot3Image.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                this.CardSlot3Image.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        // UI FUNCTION: Updates the discard pile image when called. Displays the card on top of the discard pile, or the empty slot if the pile is empty. ~Liam
        void SetDiscardSlotImage()
        {
            // Check if the discard pile has cards, or is empty, and set the image accordingly. ~Liam
            CardInfo tempCInfo = new CardInfo();
            if (this.PlayerInventory.IsDiscardEmpty())
            {
                this.DiscardSlotImage.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                bool temp = this.PlayerInventory.GetDiscardSlotImageInfo(ref tempCInfo);

                // Render the various aspects of the card to the proper slot. ~Liam
                this.DiscardSlotImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().overrideSprite = tempCInfo.CardArt;
                this.DiscardSlotImage.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = tempCInfo.CardName;
                this.DiscardSlotImage.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = tempCInfo.CardLevel.ToString();

                // Set the card image on the discard pile to active. ~Liam
                this.DiscardSlotImage.transform.GetChild(0).gameObject.SetActive(true);
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

        // Instantiate the purchase cooldown with the given parameter. ~Liam
        public void SetPurchaseCooldown(float cooldown)
        {
            this.PurchaseCardCooldown = cooldown;
            this.PurchaseCooldown = true;
        }

        // Getter function for IsPurchaseCooldown. ~Liam
        public bool IsPurchaseCooldown()
        {
            return this.PurchaseCooldown;
        }

        // UI FUNCTION: Updates the purchase cooldown text when called. Displays an int value between 1 and the max cooldown; if off cooldown, no number is displayed. ~Liam
        void SetPurchaseCooldownText(int cooldown)
        {
            if (cooldown > 0 && cooldown <= (int)this.PURCHASE_CARD_COOL_DOWN_BASE)
            {
                // Update the cooldown text. ~Liam
                this.PurchaseCooldownText.text = cooldown.ToString();
            }
            else if (cooldown == 0)
            {
                this.PurchaseCooldownText.text = "";
                this.PurchaseCooldown = false;
            }
            else
            {
                Debug.Log("ERROR: Invalid purchase cooldown display value requested.");
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
                this.GameOver();
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
        public void SetPurchaseErrorText()
        {
            // Only show this error if the success message isn't present. ~Liam
            if (!this.PurchaseSuccess)
            {
                // Play the error noise. ~Liam
                this.ErrorSound.GetComponent<AudioSource>().Play();

                this.PurchaseErrorText.SetActive(true);
                this.PurchaseErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
            }
        }
        public void SetPurchaseSuccessText()
        {
            // Deactivate any purchase error text, if present. ~Liam
            this.PurchaseErrorText.SetActive(false);
            this.PurchaseSuccessText.SetActive(true);
            this.PurchaseSuccessMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
            this.PurchaseSuccess = true;
        }

        public void TakeDamage(float damage)
        {
            if (this.IsGameOver)
            {
                return;
            }
            this.ModifyPlayerHealth(Mathf.FloorToInt(damage * -1));
            var camera = GameObject.Find("Main Camera");
            var cameraController = camera.GetComponent<CameraController>();
            cameraController.SetShaking();

            // Play injured noise. ~Liam
            this.PlayerHurtSound.GetComponent<AudioSource>().Play();
        }

        public void ApplyHealing(float healing)
        {
            if (this.IsGameOver)
            {
                return;
            }
            this.ModifyPlayerHealth(Mathf.FloorToInt(healing));
        }

        public void SetGameOver()
        {
            this.IsGameOver = true;
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

                // Code for starting hand/deck. This should be how cards are gained and created in future code. ~Jackson
                var gameControllerObject = this.GameController.GetComponent<GameController>();
                this.PlayerInventory.AddCardSlot1(gameControllerObject.GenerateCardForceBolt());
                //this.PlayerInventory.AddCardSlot2(gameControllerObject.GenerateCardLeafblade());

                this.PlayerInventory.AddCardSlot2(gameControllerObject.GenerateCardLightningStriker());
                this.PlayerInventory.AddCardSlot3(gameControllerObject.GenerateCardSummonWorker());

                this.SetCardSlot1Image();
                this.PlayerInventory.SetCardSlot1Updated(false);
                this.SetCardSlot2Image();
                this.PlayerInventory.SetCardSlot2Updated(false);
                this.SetCardSlot3Image();
                this.PlayerInventory.SetCardSlot3Updated(false);
                this.SetDrawSlotImage();
                this.PlayerInventory.SetDrawSlotUpdated(false);
                this.SetDiscardSlotImage();
                this.PlayerInventory.SetDiscardSlotUpdated(false);

                // Add remaining cards in player's deck to the discard. ~Liam

                this.PlayerInventory.GainCard(gameControllerObject.GenerateCardForceBolt());
                this.PlayerInventory.GainCard(gameControllerObject.GenerateCardInstantHeal());
                this.PlayerInventory.GainCard(gameControllerObject.GenerateCardForceBolt());
                this.PlayerInventory.GainCard(gameControllerObject.GenerateCardLeafblade());
                this.PlayerInventory.GainCard(gameControllerObject.GenerateCardForceBolt());
                this.PlayerInventory.GainCard(gameControllerObject.GenerateCardInstantHeal());
                this.PlayerInventory.GainCard(gameControllerObject.GenerateCardForceBolt());

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
                // TODO: Play audio for attempting an invalid command. ~Liam
                this.ErrorSound.GetComponent<AudioSource>().Play();

                this.Slot1ErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
                this.SetCard1ErrorText(true);
                this.PlayerInventory.SetErrorCardSlot1(false);
            }
            if (this.PlayerInventory.GetErrorCardSlot2())
            {
                // TODO: Play audio for attempting an invalid command. ~Liam
                this.ErrorSound.GetComponent<AudioSource>().Play();

                this.Slot2ErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
                this.SetCard2ErrorText(true);
                this.PlayerInventory.SetErrorCardSlot2(false);
            }
            if (this.PlayerInventory.GetErrorCardSlot3())
            {
                // TODO: Play audio for attempting an invalid command. ~Liam
                this.ErrorSound.GetComponent<AudioSource>().Play();

                this.Slot3ErrorMessageDuration = PLAYER_ERROR_MESSAGE_DURATION;
                this.SetCard3ErrorText(true);
                this.PlayerInventory.SetErrorCardSlot3(false);
            }
            if (this.PlayerInventory.GetDrawError())
            {
                // TODO: Play audio for attempting an invalid command. ~Liam
                this.ErrorSound.GetComponent<AudioSource>().Play();

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
            if (this.PurchaseErrorMessageDuration > 0.0f)
            {
                this.PurchaseErrorMessageDuration -= Time.deltaTime;
                if (this.PurchaseErrorMessageDuration <= 0.0f)
                {
                    this.PurchaseErrorMessageDuration = 0.0f;
                    this.PurchaseErrorText.SetActive(false);
                }
            }
            if (this.PurchaseSuccessMessageDuration > 0.0f)
            {
                this.PurchaseSuccessMessageDuration -= Time.deltaTime;
                if (this.PurchaseSuccessMessageDuration <= 0.0f)
                {
                    this.PurchaseSuccessMessageDuration = 0.0f;
                    this.PurchaseSuccessText.SetActive(false);
                    this.PurchaseSuccess = false;
                }
            }
            if (this.HealthUpdateMessageDuration > 0.0f)
            {
                this.HealthUpdateMessageDuration -= Time.deltaTime;
                if (this.HealthUpdateMessageDuration <= 0.0f)
                {
                    this.HealthUpdateMessageDuration = 0.0f;
                    this.HealthAddText.gameObject.SetActive(false);
                    this.HealthSubtractText.gameObject.SetActive(false);
                }
            }
            if (this.ManaUpdateMessageDuration > 0.0f)
            {
                this.ManaUpdateMessageDuration -= Time.deltaTime;
                if (this.ManaUpdateMessageDuration <= 0.0f)
                {
                    this.ManaUpdateMessageDuration = 0.0f;
                    this.ManaAddText.gameObject.SetActive(false);
                    this.ManaSubtractText.gameObject.SetActive(false);
                }
            }
            if (this.EnergyUpdateMessageDuration > 0.0f)
            {
                this.EnergyUpdateMessageDuration -= Time.deltaTime;
                if (this.EnergyUpdateMessageDuration <= 0.0f)
                {
                    this.EnergyUpdateMessageDuration = 0.0f;
                    this.EnergyAddText.gameObject.SetActive(false);
                    this.EnergySubtractText.gameObject.SetActive(false);
                }
            }
            if (this.MatterUpdateMessageDuration > 0.0f)
            {
                this.MatterUpdateMessageDuration -= Time.deltaTime;
                if (this.MatterUpdateMessageDuration <= 0.0f)
                {
                    this.MatterUpdateMessageDuration = 0.0f;
                    this.MatterAddText.gameObject.SetActive(false);
                    this.MatterSubtractText.gameObject.SetActive(false);
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

            // If purchasing is on cooldown, update it.
            if (this.PurchaseCardCooldown > 0.0f)
            {
                this.PurchaseCardCooldown -= Time.deltaTime;
            }
            // If updating the purchase cooldown set it to less than zero, fix it back to zero. ~Liam
            if (this.PurchaseCardCooldown < 0.0f)
            {
                this.PurchaseCardCooldown = 0.0f;
            }

            var tempPurchaseCooldown = Mathf.CeilToInt(this.PurchaseCardCooldown);

            // When the purchase timer reaches a new second threshold, update the UI. ~Liam
            if (this.CurrentPurchaseCooldownShown != tempPurchaseCooldown)
            {
                this.SetPurchaseCooldownText(tempPurchaseCooldown);
                this.CurrentPurchaseCooldownShown = tempPurchaseCooldown;
            }

            // Update the Resource text (change to every so often or use Fixed update perhaps?)
            //this.SetManaText();
            //this.SetEnergyText();
            //this.SetMatterText();
        }
    }
}
