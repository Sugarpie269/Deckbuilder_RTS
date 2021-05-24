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
        private float Speed = 1.0f;

        private float SeekingRange = 50.0f;

        // SAP2d
        public SAP2DPathfindingConfig Config;
        public Vector2[] path;
        private SAP2DPathfinder Pathfinder;


        // The start function will initialize our member variables.
        void Start()
        {
            // this.CurrentCommand = ScriptableObject.CreateInstance<???>();
            this.CurrentHealth = this.MaxHealth;

            // Get the player's posiiton.
            Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        void OnEnable()
        {
            Pathfinder = SAP2DPathfinder.singleton;
        }

        void Update()
        {
            this.MoveSwarmling();

            this.UpdateRotation();
        }

        private void MoveSwarmling()
        {
            Debug.Log("Moving Swarmling");
            //Vector2 Distance = Vector2.Distance(transform.position, Target.position);

            // Case 1: Swarmling is within determined range of player. Enable seeking mode.
            if (true) // (Distance < this.SeekingRange)
            {
                float step = this.Speed * Time.deltaTime;
                // Move the swarmling towards the target's location.
                //transform.position = Vector2.MoveTowards(transform.position, Target.position, step);
                path = Pathfinder.FindPath(transform.position, Target.position, Config);
                Debug.Log("Path is " + path);
            }
            /*
            // Case 2: Swarmling is outside of determined range. Enable pathfinding mode.
            else 
            {
                float step = this.Speed * Time.deltaTime;
                // Move the swarmling towards the target's location.
                //transform.position = Vector2.MoveTowards(transform.position, Target.position, step);
                path = Pathfinder.FindPath(transform.position, Target.position, Config);
            }
            */
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

