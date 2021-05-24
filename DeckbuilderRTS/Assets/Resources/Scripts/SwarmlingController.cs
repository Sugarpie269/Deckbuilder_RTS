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
        private float Speed = 2.0f;

        private Vector2 Destination;

        private float SeekingRange = 3.0f;
        private int destPoint;

        private Vector2[] DefaultPositions;
        private int DefaultIdx;

        // SAP2d
        public SAP2DPathfindingConfig Config;
        public Vector2[] path;
        private SAP2DPathfinder Pathfinder;


        // The start function will initialize our member variables.
        void Start()
        {
            Debug.Log("Start! at " + transform.position);
            // this.CurrentCommand = ScriptableObject.CreateInstance<???>();
            this.CurrentHealth = this.MaxHealth;

            // Get the player's posiiton.
            Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            float step = this.Speed * Time.deltaTime;

            DefaultPositions = new Vector2[2];
            DefaultPositions[0] = new Vector2(9.0f, 2.5f);
            DefaultPositions[1] = new Vector2(3.5f, 9.5f);
            DefaultIdx = 0;

            // Default path: move towards Default 0.
            this.Destination = DefaultPositions[DefaultIdx];
            Debug.Log("Path from " + transform.position + " to " + this.Destination);
            path = Pathfinder.FindPath(transform.position, this.Destination, Config);
            

            // Move the swarmling towards the target's location.
            //transform.position = Vector2.MoveTowards(transform.position, Target.position, step);
            
            // Find the path from swarmling to target.
            //path = Pathfinder.FindPath(transform.position, Target.position, Config);
            //Debug.Log("Path is " + path + " with " + path.Length);
            
            foreach (Vector2 item in path) {
                Debug.Log("Its" + item);
            }
             
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
                Debug.Log("Seeking");
                this.Destination = Target.position;
                path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                this.destPoint = 0;

                this.DefaultIdx = -1;
            }
            // Case 2: Swarmling is outside of determined range. Enable pathfinding mode.
            else 
            {
                Debug.Log("Pathfinding for " + this.DefaultIdx);
                // Pick a random destination point within the given boundaries.
                
                // Case 1: Returning from Seeking stage.
                if (this.DefaultIdx == -1) 
                {
                    Debug.Log("Case 1");
                    this.DefaultIdx = 0;
                    this.Destination = DefaultPositions[this.DefaultIdx];
                    path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                    this.destPoint = 0;
                }
                // Case 2: Reached end point of Default 0. Swap to Default 1.
                else if (this.DefaultIdx == 0 && this.destPoint == path.Length - 1) 
                {
                    Debug.Log("Case 2");
                    this.DefaultIdx = 1;
                    this.Destination = DefaultPositions[this.DefaultIdx];
                    path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                    this.destPoint = 0;
                }
                // Case 3: Reached end point of Default 1. Swap to Default 0.
                else if (this.DefaultIdx == 1 && this.destPoint == path.Length - 1)
                {
                    Debug.Log("Case 3");
                    this.DefaultIdx = 0;
                    this.Destination = DefaultPositions[this.DefaultIdx];
                    path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                    this.destPoint = 0;
                }
                else 
                {
                    //Debug.Log("Else");
                    //path = Pathfinder.FindPath(transform.position, this.Destination, Config);
                    //this.destPoint = 0;
                }
            }

            this.MoveSwarmling();

            this.UpdateRotation();
        }

        private void MoveSwarmling()
        {
            Debug.Log("Moving Swarmling towards" + this.Destination + " from " + transform.position + " for path " + path.Length);
        
            if (path.Length == 0)
            {
                Debug.Log("Length is 0");
                return;
            }

            float step = this.Speed * Time.deltaTime;
            Debug.Log("Destpoint is " + destPoint + " and " + path[destPoint]);
            
            transform.position = Vector2.MoveTowards(transform.position, path[destPoint], step);
            //transform.position = path[destPoint];
            
            if (Vector2.Distance(transform.position, path[destPoint]) < 1.0f) 
            {
                destPoint = (destPoint + 1) % path.Length;
                Debug.Log("At " + transform.position + " updating to " + destPoint);
            }

            Debug.Log("Position is " + transform.position);
        
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

