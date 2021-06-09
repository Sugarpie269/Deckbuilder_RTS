using UnityEngine;

using DeckbuilderRTS;
using TMPro;
using System.Collections.ObjectModel;
using SAP2D;

namespace DeckbuilderRTS
{
    // TODO: Ideally, this, swarmling, and worker should inherit from a more generic entity class

    public class SlimelingController : MonoBehaviour
    {
        private IUnitCommand CurrentCommand;
        private int CurrentHealth;
        [SerializeField] private int MaxHealth = 50; // 10;

        private Transform Target;
        private Transform PlayerTarget;

        private Vector2 Destination;

        private int destPoint;

        private Vector2[] DefaultPositions;
        private int DefaultIdx;

        // SAP2d
        public SAP2DPathfindingConfig Config;
        public Vector2[] path;
        private SAP2DPathfinder Pathfinder;

        // Allow editor to choose properties for swarmlings.
        [SerializeField]
        public Vector2 DefaultPosition0;
        [SerializeField]
        public Vector2 DefaultPosition1;
        [SerializeField]
        public float Speed;
        [SerializeField]
        public float SeekingRange;
        private bool Disabled = false;
        [SerializeField] private float ResetPathTime = 2f;
        private float CurrentResetPathTime = 0f;
        private int PrevDefaultIdx;

        // Attack Prefab Settings
        private float SummonDistance = .5f;
        private float AttackDirection;
        private float AttackVelocity = 10.0f;
        [SerializeField]
        private float AttackDamage = 5.0f;
        [SerializeField] 
        public Object AttackPrefab;
        [SerializeField] 
        public float AttackRate;

        private float ElapsedTime;
        private bool IsGameOver = false;

        private SAP2DAgent Agent;

        private bool SearchingDepot = true;
        private int CurrentDepot = 0;
        private Collection<Transform> Depots;

        [SerializeField] private Object HealthTextPrefab;
        private GameObject HealthText;
        private bool loaded = false;

        [SerializeField] private float DisplayDamageTime = 1.5f;
        private float CurrentDisplayDamageTime = 0f;
        private bool DisplayingDamage = false;

        // Audio objects. ~Liam
        [SerializeField] private GameObject HurtNoise;
        [SerializeField] private GameObject DeathNoise;
        [SerializeField] private AudioSource LaserNoise;

        // The start function will initialize our member variables.
        void Start()
        {
            // Get the game objects for hurt and death noises.
            this.HurtNoise = GameObject.Find("SwarmlingHurtNoise");
            this.DeathNoise = GameObject.Find("SwarmlingDeathNoise");
            this.LaserNoise = gameObject.GetComponent<AudioSource>();

            this.Depots = new Collection<Transform>();
            this.Agent = this.gameObject.GetComponent<SAP2DAgent>();
            var depots = GameObject.FindGameObjectsWithTag("Depot");
            this.DefaultPositions = new Vector2[depots.Length];
            for (var i = 0; i < depots.Length; i++)
            {
                this.DefaultPositions[i] = new Vector2(depots[i].transform.position.x, depots[i].transform.position.y);
                this.Depots.Add(depots[i].transform);
            }
            // this.CurrentCommand = ScriptableObject.CreateInstance<???>();
            this.CurrentHealth = this.MaxHealth;

            // Get the player's posiiton.
            this.PlayerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            Debug.Assert(this.PlayerTarget != null);
            this.Target = this.PlayerTarget;

            float step = this.Speed * Time.deltaTime;

            // Set Default values for swarmling movement from SerializeField Values.
            /*DefaultPositions = new Vector2[2];
            DefaultPositions[0] = DefaultPosition0;  //new Vector2(9.0f, 2.5f);
            DefaultPositions[1] = DefaultPosition1;*/  //new Vector2(1.5f, 9.5f);

            // Default path: move towards Default 0.
            DefaultIdx = this.GetRandomPositionID();
            this.PrevDefaultIdx = this.DefaultIdx;
            this.Destination = DefaultPositions[DefaultIdx];
            this.destPoint = 0;
            this.path = null;
            path = Pathfinder.FindPath(transform.position, this.Destination, Config);

            // Time for intermittent attacks.
            this.ElapsedTime = 0;

            this.CurrentDepot = this.GetRandomDepot();
            this.Agent.Target = this.Depots[this.CurrentDepot];
            this.SearchingDepot = true;
        }

        public void AddMaxHealth(int maxHealth)
        {
            this.MaxHealth += maxHealth;
            this.CurrentHealth += maxHealth;
        }

        void OnEnable()
        {
            Pathfinder = SAP2DPathfinder.singleton;
        }

        private int GetRandomDepot()
        {
            return Random.Range(0, this.Depots.Count);
        }

        public void SetDisabled()
        {
            this.Disabled = true;
        }

        public void SetTarget(Transform newTarget)
        {
            this.Target = newTarget;
            if (this.Target == null)
            {
                return;
            }
            if (this.Agent == null)
            {
                return;
            }
            this.Agent.Target = this.Target;
        }

        public void ClearTarget()
        {
            this.Target = this.PlayerTarget;
            this.Agent.Target = this.Target;
        }

        private int GetRandomPositionID()
        {
            return Random.Range(0, this.DefaultPositions.Length);
        }

        void Update()
        {
            if (this.Disabled)
            {
                return;
            }
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
                text.GetComponent<TextMeshProUGUI>().text = this.CurrentHealth.ToString() + "/" + this.MaxHealth.ToString();
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

            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            if (this.Disabled)
            {
                return;
            }
            if (this.Target == null)
            {
                this.Target = this.PlayerTarget;
                this.Agent.Target = this.Target;
            }
            

            // Get the path to the target.
            float Distance = Vector2.Distance(transform.position, Target.position);
            float depotDistance = Vector2.Distance(transform.position, this.Depots[this.CurrentDepot].position);

            if (this.SearchingDepot == false)
            {
                if (Distance > this.SeekingRange)
                {
                    this.SearchingDepot = true;
                    this.ClearTarget();
                    this.Agent.Target = this.Depots[this.CurrentDepot];
                }
            }
            else
            {
                if (depotDistance < this.SeekingRange / 2)
                {
                    this.CurrentDepot = this.GetRandomDepot();
                    this.Agent.Target = this.Depots[this.CurrentDepot];
                }
                if (Distance < this.SeekingRange)
                {
                    this.SearchingDepot = false;
                    this.Agent.Target = this.Target;
                }
            }

            // Swarmling is within determined range of player. Shoot projectiles.
            if (Distance < this.SeekingRange)
            {
                // Shoot projectiles if needed.
                this.ElapsedTime += Time.deltaTime;
                if (this.ElapsedTime >= this.AttackRate) 
                {
                    this.ShootProjectiles();
                    this.ElapsedTime = 0;
                }
            }
        }

        private void MoveSwarmling()
        {
            // Ignore if invaild path.
            if (path == null)
            {
                Debug.Log("NO PATH!");
            }
            else if (path.Length == 0)
            {
                return;
            }

            float step = this.Speed * Time.deltaTime;
            
            // Move towards the current step in the path.
            if (path != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, path[destPoint], step);

                // Update to the following step in the path if has reached the current step.
                if (Vector2.Distance(transform.position, path[destPoint]) < 1.0f)
                {
                    destPoint = (destPoint + 1) % path.Length;
                }
            }
            
            
            
        }

        private void UpdateRotation()
        {
            
            var dirVec = new Vector2(this.Target.position.x - this.gameObject.transform.position.x, this.Target.position.y - this.gameObject.transform.position.y);
            if (this.path != null && this.DefaultIdx != -1 && this.path.Length > 0)
            {
                dirVec = new Vector2(this.path[destPoint].x - this.gameObject.transform.position.x, this.path[destPoint].y - this.gameObject.transform.position.y);
            }
            
            

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

        public void TakeDamage(float damage)
        {
            this.CurrentHealth -= Mathf.FloorToInt(damage);
            var text = this.HealthText.transform.GetChild(1);
            text.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(-damage).ToString();
            this.DisplayingDamage = true;
            // If the swarmling dies, destroy the game object.
            if (this.CurrentHealth <= 0)
            {
                // Play audio corresponding to the swarmling's death. ~Liam
                this.DeathNoise.GetComponent<AudioSource>().Play();

                // Give the player 2-3 of a random resource, 50% of the time.
                System.Random r = new System.Random();
                int resourceIndex = r.Next(1, 6);
                GameObject player = GameObject.Find("Player");
                switch(resourceIndex)
                {
                    case 1:
                        player.GetComponent<PlayerController>().ModifyPlayerMana(r.Next(2,3));
                        break;
                    case 2:
                        player.GetComponent<PlayerController>().ModifyPlayerEnergy(r.Next(2, 3));
                        break;
                    case 3:
                        player.GetComponent<PlayerController>().ModifyPlayerMatter(r.Next(2, 3));
                        break;
                    default:
                        break;
                }

                GameObject.Destroy(this.HealthText);
                GameObject.Destroy(this.gameObject);

            }
            else
            {
                // If the swarmling didn't die, play the hurt noise. ~Liam
                this.HurtNoise.GetComponent<AudioSource>().Play();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Player" || collision.collider.tag == "Worker" || collision.collider.tag == "Swarmling")
            {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            }
        }

        private void ShootProjectiles()
        {   
            var targetPos = Target.position;

            // Shoot from swarmling towards player.
            var attackDirection = new Vector2(Target.position.x - transform.position.x, Target.position.y - transform.position.y);
            attackDirection.Normalize();
         
            // Fireball begins from swarmling's position.
            var attackPos = new Vector2(transform.position.x + attackDirection.x * this.SummonDistance, transform.position.y + attackDirection.y * this.SummonDistance);

            var newAttack = Object.Instantiate(this.AttackPrefab) as GameObject;
            newAttack.transform.position = attackPos;
            GameObject.Destroy(newAttack, 5f);
            var attackController = newAttack.GetComponent<SwarmlingBulletController>();
            var attackVelocity = new Vector2(this.AttackVelocity * attackDirection.x, this.AttackVelocity * attackDirection.y);
            var vec = Target.position - this.gameObject.transform.position;
            var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg) + 90;

            attackController.SetAttributes(this.AttackDamage, attackVelocity, angle);

            var collider = attackController.GetComponent<BoxCollider2D>();
            var swarmCollider = this.GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(collider, swarmCollider);

            if(this.LaserNoise)
            {
                this.LaserNoise.Play();
            }
        }
    }
}

