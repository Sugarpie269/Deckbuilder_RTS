using UnityEngine;
using DeckbuilderRTS;


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
        private float SummonDistance = 1.5f;
        private float AttackDirection;
        private float AttackSpeed = 15.0f;

        // Boss projectiles issue more damage than swarmlings.
        private float AttackDamage = 10.0f;

        [SerializeField] public Object AttackPrefab;

        // AttackRate denotes the time (in seconds) between each projectile.
        [SerializeField] public float AttackRate;

        private bool Disabled = false;

        private bool IsGameOver = false;

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
            if (this.CurrentHP <= 0 && !this.Disabled)
            {
                this.SetDisabled();
                var deadBoss = GameObject.Find("DeadBoss");
                deadBoss.transform.position = this.transform.position;
                deadBoss.transform.rotation = this.transform.rotation;
                this.transform.position = new Vector2(-10000f, -10000f);
                this.EnemyPlayer.GetComponent<PlayerController>().DisplayVictoryText();

            }
        }

        private void Update()
        {
            if (this.Disabled)
            {
                return;
            }
            if (!this.LoadedData)
            {
                //this.EnemyPlayerController = this.EnemyPlayer.GetComponent<PlayerController>();
                this.LoadedData = true;
            }

            this.UpdateFacing();

            // Get the path to the target.
            float Distance = Vector2.Distance(transform.position, Target.position);

            // Player is within determined range of boss. Shoot projectiles.
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
        }
    }
}
