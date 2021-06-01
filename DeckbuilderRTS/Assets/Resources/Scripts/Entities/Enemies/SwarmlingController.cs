using UnityEngine;

using DeckbuilderRTS;

using SAP2D;

namespace DeckbuilderRTS
{
    public class SwarmlingController : MonoBehaviour
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

        // Allow user to choose properties for swarmlings.
        [SerializeField]
        public Vector2 DefaultPosition0;
        [SerializeField]
        public Vector2 DefaultPosition1;
        [SerializeField]
        public float Speed;
        [SerializeField]
        public float SeekingRange;
        private bool Disabled = false;


        // Fireball content.
        private float SummonDistance = 1.5f;
        private float AttackDirection;
        private float AttackSpeed = 15.0f;

        // Swarmling projectiles issue fewer damage than Boss.
        private float AttackDamage = 2.0f;


        [SerializeField] public Object AttackPrefab;
        [SerializeField] public float AttackRate;

        private float ElapsedTime;

        private bool IsGameOver = false;

        // The start function will initialize our member variables.
        void Start()
        {
            var depots = GameObject.FindGameObjectsWithTag("Depot");
            this.DefaultPositions = new Vector2[depots.Length];
            for (var i = 0; i < depots.Length; i++)
            {
                this.DefaultPositions[i] = new Vector2(depots[i].transform.position.x, depots[i].transform.position.y);
            }
            // this.CurrentCommand = ScriptableObject.CreateInstance<???>();
            this.CurrentHealth = this.MaxHealth;

            // Get the player's posiiton.
            this.PlayerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            this.Target = this.PlayerTarget;

            float step = this.Speed * Time.deltaTime;

            // Set Default values for swarmling movement from SerializeField Values.
            /*DefaultPositions = new Vector2[2];
            DefaultPositions[0] = DefaultPosition0;  //new Vector2(9.0f, 2.5f);
            DefaultPositions[1] = DefaultPosition1;*/  //new Vector2(1.5f, 9.5f);

            // Default path: move towards Default 0.
            DefaultIdx = this.GetRandomPositionID();
            this.Destination = DefaultPositions[DefaultIdx];
            this.destPoint = 0;
            this.path = null;
            path = Pathfinder.FindPath(transform.position, this.Destination, Config);
            Debug.Log("path is " + this.path);

            // Time for intermittent attacks.
            this.ElapsedTime = 0;
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

        public void SetDisabled()
        {
            this.Disabled = true;
        }

        public void SetTarget(Transform newTarget)
        {
            this.Target = newTarget;
        }

        public void ClearTarget()
        {
            this.Target = this.PlayerTarget;
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
            if (this.Target == null)
            {
                this.Target = this.PlayerTarget;
            }

            // Get the path to the target.
            float Distance = Vector2.Distance(transform.position, Target.position);

            // Case 1: Swarmling is within determined range of player. Enable seeking mode.
            if (Distance < this.SeekingRange)
            {
                // Switch Target position to be the player's position.
                this.Destination = Target.position;
                var PossiblePath = Pathfinder.FindPath(transform.position, this.Destination, Config);
                
                // Check that this new path is valid.
                if (PossiblePath != null) 
                {
                    path = PossiblePath;
                }
                this.destPoint = 0;

                // Disable Default pathfinding.
                this.DefaultIdx = -1;
            }
            // Case 2: Swarmling is outside of determined range. Enable pathfinding mode.
            else 
            {   
                // Case 1: Returning to pathfinding from Seeking stage.
                if (this.DefaultIdx == -1) 
                {
                    this.DefaultIdx = this.GetRandomPositionID();//0;
                    this.Destination = DefaultPositions[this.DefaultIdx];
                    path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                    this.destPoint = 0;
                }
                else if (this.path == null)
                {

                }
                // Case 2: Reached end point of Default position. Swap to another Default.
                else if (this.destPoint == path.Length - 1)
                {
                    this.DefaultIdx = this.GetRandomPositionID();
                    this.Destination = DefaultPositions[this.DefaultIdx];
                    path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                    this.destPoint = 0;
                }
                // Case 2: Reached end point of Default 0. Swap to Default 1.
                /*else if (this.DefaultIdx == 0 && this.destPoint == path.Length - 1) 
                {
                    this.DefaultIdx = 1;
                    this.Destination = DefaultPositions[this.DefaultIdx];
                    path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                    this.destPoint = 0;
                }
                // Case 3: Reached end point of Default 1. Swap to Default 0.
                else if (this.DefaultIdx == 1 && this.destPoint == path.Length - 1)
                {
                    this.DefaultIdx = 0;
                    this.Destination = DefaultPositions[this.DefaultIdx];
                    path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                    this.destPoint = 0;
                }*/
            }

            // Move the swarmling.
            this.MoveSwarmling();

            // Rotate the swarmling.
            this.UpdateRotation();

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
            // If the swarmling dies, destroy the game object.
            if (this.CurrentHealth <= 0)
            {
                GameObject.Destroy(this.gameObject);
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
            var playerPos = Target.position;

            // Shoot from swarmling towards player.
            var attackDirection = new Vector2(Target.position.x - transform.position.x, Target.position.y - transform.position.y);
            attackDirection.Normalize();
         
            // Fireball begins from swarmling's position.
            var attackPos = new Vector2(transform.position.x + attackDirection.x * this.SummonDistance, transform.position.y + attackDirection.y * this.SummonDistance);

            var newAttack = Object.Instantiate(this.AttackPrefab) as GameObject;
            newAttack.transform.position = attackPos;
            GameObject.Destroy(newAttack, 5f);
            var attackController = newAttack.GetComponent<SwarmlingBulletController>();
            var attackVelocity = new Vector2(this.AttackSpeed * attackDirection.x, this.AttackSpeed * attackDirection.y);
            attackController.SetAttributes(this.AttackDamage, attackVelocity);
        }
    }
}

