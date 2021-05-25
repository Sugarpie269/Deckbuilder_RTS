using UnityEngine;

using DeckbuilderRTS;

using SAP2D;

namespace DeckbuilderRTS
{
    public class SwarmlingController : MonoBehaviour
    {
        private IUnitCommand CurrentCommand;
        private int CurrentHealth;
        private int MaxHealth = 50; // 10;

        private Transform Target;

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


        // The start function will initialize our member variables.
        void Start()
        {
            // this.CurrentCommand = ScriptableObject.CreateInstance<???>();
            this.CurrentHealth = this.MaxHealth;

            // Get the player's posiiton.
            Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            float step = this.Speed * Time.deltaTime;

            // Set Default values for swarmling movement from SerializeField Values.
            DefaultPositions = new Vector2[2];
            DefaultPositions[0] = DefaultPosition0;  //new Vector2(9.0f, 2.5f);
            DefaultPositions[1] = DefaultPosition1;  //new Vector2(1.5f, 9.5f);
            DefaultIdx = 0;

            // Default path: move towards Default 0.
            this.Destination = DefaultPositions[DefaultIdx];
            path = Pathfinder.FindPath(transform.position, this.Destination, Config);
        }

        void OnEnable()
        {
            Pathfinder = SAP2DPathfinder.singleton;
        }

        void Update()
        {
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
                    this.DefaultIdx = 0;
                    this.Destination = DefaultPositions[this.DefaultIdx];
                    path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                    this.destPoint = 0;
                }
                // Case 2: Reached end point of Default 0. Swap to Default 1.
                else if (this.DefaultIdx == 0 && this.destPoint == path.Length - 1) 
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
                }
            }

            // Move the swarmling.
            this.MoveSwarmling();

            // Rotate the swarmling.
            this.UpdateRotation();
        }

        private void MoveSwarmling()
        {
            // Ignore if invaild path.
            if (path == null)
            {
                Debug.Log("NO PATH!");
            }
            if (path.Length == 0)
            {
                return;
            }

            float step = this.Speed * Time.deltaTime;
            
            // Move towards the current step in the path.
            transform.position = Vector2.MoveTowards(transform.position, path[destPoint], step);
            
            // Update to the following step in the path if has reached the current step.
            if (Vector2.Distance(transform.position, path[destPoint]) < 1.0f) 
            {
                destPoint = (destPoint + 1) % path.Length;
            }
        }

        private void UpdateRotation()
        {
            var dirVec = new Vector2(this.Target.position.x - this.gameObject.transform.position.x, this.Target.position.y - this.gameObject.transform.position.y);
            

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
    }
}

