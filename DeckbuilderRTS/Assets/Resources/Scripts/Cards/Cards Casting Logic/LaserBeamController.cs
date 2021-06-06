using UnityEngine;

using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class LaserBeamController : MonoBehaviour
    {
        private float Damage;
        private float Delay; // delay in seconds before the bolt hurts things
        private float DamageDelay;
        private float DamageDelayCounter;
        public LaserBeamController()
        {
            this.Damage = 20.0f;
            this.Delay = 2.0f; // time before damage is dealt (charge up)
            this.DamageDelay = 0.25f;
            this.DamageDelayCounter = 0.0f;
        }

        // The start function will initialize our member variables.
        public void Start()
        {
        }

        public void SetAttributes(float damage, float delay)
        {
            this.Damage = damage;
            this.Delay = delay;
        }

        public void Update()
        {
            
            //this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(this.Velocity.x, this.Velocity.y);
        }

        // if the things touching it are still in the collider, and the charge up has passed, deal damage
        private void OnCollisionStay2D(Collision2D collision)
        {
            // ensure that damage is only dealt every DamageDelay (0.25) seconds
            this.DamageDelayCounter += Time.deltaTime;
            if (this.DamageDelayCounter >= this.DamageDelay)
            {
                this.DamageDelayCounter -= this.DamageDelay;
                // If the fireball collides with a swarmling, the swarmling takes damage and knocks it back. Otherwise, the forcebolt is destroyed.
                if (collision.collider.tag == "Swarmling")
                {
                    collision.collider.GetComponent<SwarmlingController>().TakeDamage(this.Damage);
                    // TODO: Add knockback to it
                    Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
                    //GameObject.Destroy(this.gameObject);
                }
                /*else if (collision.collider.tag == "Player")
                {
                    collision.collider.GetComponent<PlayerController>().TakeDamage(this.Damage);
                    GameObject.Destroy(this.gameObject);
                }*/
                else if (collision.collider.tag == "Boss")
                {
                    collision.collider.GetComponent<BossController>().TakeDamage(this.Damage);
                    Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
                    //GameObject.Destroy(this.gameObject);
                }
                else if (collision.collider.tag == "Obstacle")
                {
                    //GameObject.Destroy(collision.gameObject);
                    collision.gameObject.SetActive(false);
                }
                else
                {
                    //Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
                }
            }
        }

        
            
    }
}
