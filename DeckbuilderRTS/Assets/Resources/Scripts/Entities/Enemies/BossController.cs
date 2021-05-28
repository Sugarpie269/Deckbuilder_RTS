using UnityEngine;
using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private int CurrentHP;
        [SerializeField] private int MaxHP = 1000;
        [SerializeField] GameObject EnemyPlayer;

        [SerializeField] public float SeekingRange;
        //private PlayerController EnemyPlayerController;
        private bool LoadedData = false;

        private Transform Target;
        private float ElapsedTime;

        // Fireball content.
        private float SummonDistance = 1.5f;
        private float FireballDirection;
        private float FireballSpeed = 15.0f;

        // Boss projectiles issue more damage than swarmlings.
        private float FireballDamage = 10.0f;

        [SerializeField] public Object FireballPrefab;

        // AttackRate denotes the time (in seconds) between each projectile.
        [SerializeField] public float AttackRate;


        private bool IsGameOver = false;
        
        private void Start()
        {
            // Get the player's posiiton.
            Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            // Time for intermittent attacks.
            this.ElapsedTime = 0;
        }

        private void Update()
        {
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
            var fireballDirection = new Vector2(Target.position.x - transform.position.x, Target.position.y - transform.position.y);
            fireballDirection.Normalize();
         
            // Fireball begins from swarmling's position.
            var fireballPos = new Vector2(transform.position.x + fireballDirection.x * this.SummonDistance, transform.position.y + fireballDirection.y * this.SummonDistance);

            var newFireball = Object.Instantiate(this.FireballPrefab) as GameObject;
            newFireball.transform.position = fireballPos;
            GameObject.Destroy(newFireball, 5f);
            var fireballController = newFireball.GetComponent<FireballController>();
            var fireballVelocity = new Vector2(this.FireballSpeed * fireballDirection.x, this.FireballSpeed * fireballDirection.y);
            fireballController.SetAttributes(this.FireballDamage, fireballVelocity);
        }
    }
}
