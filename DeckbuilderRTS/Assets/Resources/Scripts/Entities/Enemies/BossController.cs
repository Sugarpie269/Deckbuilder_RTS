using UnityEngine;
using DeckbuilderRTS;
using TMPro;

namespace DeckbuilderRTS
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private int CurrentHP;
        [SerializeField] private int MaxHP = 1000;
        [SerializeField] private int DamageResistance = 10;
        [SerializeField] GameObject EnemyPlayer;

        [SerializeField] public float SeekingRange;
        //private PlayerController EnemyPlayerController;
        private bool LoadedData = false;

        private Transform Target;
        private float ElapsedTime;

        // Attack content.
        [SerializeField] private float SummonDistance = 1.5f;
        private float AttackDirection;
        [SerializeField] private float AttackSpeed = 15.0f;

        // Boss projectiles issue more damage than swarmlings.
        [SerializeField] private float AttackDamage = 10.0f;

        [SerializeField] public Object AttackPrefab;

        // AttackRate denotes the time (in seconds) between each projectile.
        [SerializeField] public float AttackRate;

        private bool Disabled = false;

        private bool IsGameOver = false;

        public enum AttackPattern {None, Basic, Laser};
        private AttackPattern currentAttack = AttackPattern.None;

        [SerializeField] private float LaserAttackTime = 5f;
        private float CurrentLaserTime = 0f;
        private int LaserStage = 0;

        [SerializeField] private Object HealthTextPrefab;
        private GameObject HealthText;
        private bool loaded = false;

        [SerializeField] private float DisplayDamageTime = 1.5f;
        private float CurrentDisplayDamageTime = 0f;
        private bool DisplayingDamage = false;

        [SerializeField] private float RestoreHPDelay = 15f;
        [SerializeField] private float RestoreHPAmount = 5f;
        private float CurrentRestoreHPTime = 0f;

        // Audio objects. ~Liam
        [SerializeField] private GameObject HurtNoise;
        [SerializeField] private GameObject DeathNoise;

        private void UpdateAttackPattern()
        {
            var randInt = Random.Range(1, 101);

            if (randInt <= 60)
            {
                this.currentAttack = AttackPattern.Basic;
            }
            else
            {
                this.currentAttack = AttackPattern.Laser;
            }
        }

        public void SetDisabled()
        {
            this.Disabled = true;
        }
        
        private void Start()
        {
            // Get the player's posiiton.
            Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            // Time for intermittent attacks.
            this.ElapsedTime = 0;
            
        }

        public void TakeDamage(float damage)
        {
            float newDamage = damage - this.DamageResistance > 0 ? damage - this.DamageResistance : 0f;
            this.CurrentHP -= Mathf.FloorToInt(newDamage);

            Debug.Log("Boss TakeDamage for " + damage + " to become " + newDamage + " now health is " + this.CurrentHP);

            var text = this.HealthText.transform.GetChild(1);
            text.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(-newDamage).ToString();
            this.DisplayingDamage = true;
            if (this.CurrentHP <= 0 && !this.Disabled)
            {
                this.SetDisabled();
                var deadBoss = GameObject.Find("DeadBoss");
                deadBoss.transform.position = this.transform.position;
                deadBoss.transform.rotation = this.transform.rotation;
                this.transform.position = new Vector2(-10000f, -10000f);
                this.EnemyPlayer.GetComponent<PlayerController>().DisplayVictoryText();
                this.DeathNoise.GetComponent<AudioSource>().Play();
            }
            else if (newDamage > 0)
            {
                this.HurtNoise.GetComponent<AudioSource>().Play();
            }
        }

        private void Update()
        {
            if (!this.loaded)
            {
                this.loaded = true;
                //var sampleText = GameObject.Find("TestText");
                //var newCanvas = Object.Instantiate(this.)
                var canvas = GameObject.Find("Canvas");
                //sampleText.transform.position = new Vector3(canvas.transform.position.x - this.transform.position.x, canvas.transform.position.y - this.transform.position.y, this.transform.position.z);
                this.HealthText = Object.Instantiate(this.HealthTextPrefab, this.transform.parent) as GameObject;
                var damageText = this.HealthText.transform.GetChild(1);
                damageText.GetComponent<TextMeshProUGUI>().text = "";
                //this.HealthText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+5);
            }
            if (this.HealthText != null)
            {
                this.HealthText.transform.position = this.transform.position;
                var text = this.HealthText.transform.GetChild(0);
                text.GetComponent<TextMeshProUGUI>().text = this.CurrentHP.ToString() + "/" + this.MaxHP.ToString();
            }
            if (this.DisplayingDamage)
            {
                this.CurrentDisplayDamageTime += Time.deltaTime;
                if (this.CurrentDisplayDamageTime >= this.DisplayDamageTime)
                {
                    this.CurrentDisplayDamageTime = 0f;
                    this.DisplayingDamage = false;
                    var text = this.HealthText.transform.GetChild(1);
                    text.GetComponent<TextMeshProUGUI>().text = "";
                }
            }
            if (this.Disabled)
            {
                return;
            }
            if (!this.LoadedData)
            {
                //this.EnemyPlayerController = this.EnemyPlayer.GetComponent<PlayerController>();
                this.LoadedData = true;
            }

            this.CurrentRestoreHPTime += Time.deltaTime;
            if (this.CurrentRestoreHPTime >= this.RestoreHPDelay)
            {
                this.CurrentHP += Mathf.FloorToInt(this.RestoreHPAmount);
                if (this.CurrentHP >= this.MaxHP)
                {
                    this.CurrentHP = this.MaxHP;
                }
                this.CurrentRestoreHPTime = 0f;
            }

            this.UpdateFacing();

            // Get the path to the target.
            float Distance = Vector2.Distance(transform.position, Target.position);

            bool canAttack = false;

            // Player is within determined range of boss. Shoot projectiles.
            if (Distance < this.SeekingRange)
            {
                if (this.currentAttack == AttackPattern.None)
                {
                    this.UpdateAttackPattern();
                }
                canAttack = true;
                           
            }


            if (this.currentAttack == AttackPattern.Basic)
            {
                // Shoot projectiles if needed.
                this.ElapsedTime += Time.deltaTime;
                if (this.ElapsedTime >= this.AttackRate)
                {
                    this.ShootProjectiles();
                    this.ElapsedTime = 0;
                    if (canAttack)
                    {
                        this.UpdateAttackPattern();
                    }
                    else
                    {
                        this.currentAttack = AttackPattern.None;
                    }
                    
                }
            }
            else if (this.currentAttack == AttackPattern.Laser)
            {
                this.CurrentLaserTime += Time.deltaTime;
                if (this.CurrentLaserTime >= this.LaserAttackTime / 4 && this.LaserStage == 0)
                {
                    this.LaserStage++;
                    this.ShootProjectiles();
                }
                else if (this.CurrentLaserTime >= this.LaserAttackTime * 7 / 24 && this.LaserStage == 1)
                {
                    this.LaserStage++;
                    this.ShootProjectiles();
                }
                else if (this.CurrentLaserTime >= this.LaserAttackTime * 3 / 4 && this.LaserStage == 2)
                {
                    this.LaserStage++;
                    // Create laser
                    Debug.Log("get lasered!");
                }
                else if (this.CurrentLaserTime >= this.LaserAttackTime)
                {
                    this.LaserStage = 0;
                    this.CurrentLaserTime = 0f;
                    if (canAttack)
                    {
                        this.UpdateAttackPattern();
                    }
                    else
                    {
                        this.currentAttack = AttackPattern.None;
                    }
                }
            }
            else
            {
            }
        }

        private void UpdateFacing()
        {
            var dirVec = new Vector2(this.EnemyPlayer.transform.position.x - this.gameObject.transform.position.x, this.EnemyPlayer.transform.position.y - this.gameObject.transform.position.y);

            //dirVec.Normalize();

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
            //transform.SetPositionAndRotation(this.transform.position, new Quaternion(0f, 0f, Mathf.Atan(dirVec.y / dirVec.x), 0f));
            //transform.Rotate(Vector3.forward * Mathf.Atan(dirVec.y/dirVec.x));
        }

        private void ShootProjectiles()
        {
            var playerPos = Target.position;

            // Shoot from swarmling towards player.
            var AttackDirection = new Vector2(Target.position.x - transform.position.x, Target.position.y - transform.position.y);
            AttackDirection.Normalize();
         
            // Attack begins from swarmling's position.
            var AttackPos = new Vector2(transform.position.x + AttackDirection.x * this.SummonDistance, transform.position.y + AttackDirection.y * this.SummonDistance);

            var newAttack = Object.Instantiate(this.AttackPrefab) as GameObject;
            newAttack.transform.position = AttackPos;
            GameObject.Destroy(newAttack, 5f);
            var AttackController = newAttack.GetComponent<SwarmlingBulletController>();
            var AttackVelocity = new Vector2(this.AttackSpeed * AttackDirection.x, this.AttackSpeed * AttackDirection.y);
            AttackController.SetAttributes(this.AttackDamage, AttackVelocity);
            var collider = AttackController.GetComponent<BoxCollider2D>();
            var bossCollider = this.GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(collider, bossCollider);
        }
    }
}
