using UnityEngine;

using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class LaserBeamController : MonoBehaviour
    {
        private float Damage;
        private float Delay; // delay in seconds before the bolt hurts things
        private float DelayCounter;
        private float TimeBetweenDamageTick;
        private float DamageDelayCounter;
        private float Lifetime;
        private bool CanDamage;

        public LaserBeamController()
        {
            this.Damage = 20.0f;
            this.Delay = 2.0f; // time before damage is dealt (charge up)
            this.DelayCounter = 0.0f;
            this.TimeBetweenDamageTick = 0.25f;
            this.DamageDelayCounter = 0.0f;
            this.Lifetime = 3.0f; // default amount of time the game object will allow damage to be dealt
            this.CanDamage = false;
        }

        // The start function will initialize our member variables.
        public void Start()
        {
        }

        public void SetAttributes(float damage, float delay, float lifetime, float angle)
        {
            this.Damage = damage;
            this.Delay = delay;
            this.Lifetime = lifetime;
            this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void Update()
        {
            // after [Delay] seconds, enable damage
            this.DelayCounter += Time.deltaTime;
            if (this.DelayCounter >= Delay && this.CanDamage == false)
            {
                this.CanDamage = true;
                Debug.Log("Laser can do Damage now.");
            }
            if (this.DelayCounter >= this.Lifetime)
            {
                this.CanDamage = false;
                Debug.Log("Laser cannot do Damage anymore.");
                this.gameObject.SetActive(false);
            }
            //this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(this.Velocity.x, this.Velocity.y);
        }

        // TODO change the collider to a trigger only?
        private void OnTriggerStay2D(Collider2D collision)
        {
            
        }

        // if the things touching it are still in the collider, and the charge up has passed, deal damage
        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("I am touching " + collision.collider.gameObject.name);
            // ensure that damage is only dealt every DamageDelay (0.25) seconds
            this.DamageDelayCounter += Time.deltaTime;
            if (this.DamageDelayCounter >= this.TimeBetweenDamageTick)
            {
                this.DamageDelayCounter -= this.TimeBetweenDamageTick;
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
