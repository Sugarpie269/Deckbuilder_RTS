using UnityEngine;

using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class ForceBoltController : MonoBehaviour
    {
        private float Damage;
        private Vector2 Velocity;

        public ForceBoltController()
        {
            this.Damage = 10.0f;
            this.Velocity = new Vector2(10.0f, 0.0f);
        }

        // The start function will initialize our member variables.
        public void Start()
        {
            
        }

        public void SetAttributes(float damage, Vector2 velocity, float angle)
        {
            this.Damage = damage;
            this.Velocity = velocity;

            this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void Update()
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(this.Velocity.x, this.Velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // If the forcebolt collides with a swarmling, the swarmling takes damage and gets knocked back. Otherwise, the forcebolt is destroyed.
            if (collision.collider.tag == "Swarmling")
            {
                collision.collider.GetComponent<SwarmlingController>().TakeDamage(this.Damage);
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
                GameObject.Destroy(this.gameObject);
            }
            else if (collision.collider.tag == "Slimeling")
            {
                collision.collider.GetComponent<SlimelingController>().TakeDamage(this.Damage);
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
                GameObject.Destroy(this.gameObject);
            }
            else if (collision.collider.tag == "Boss")
            {
                collision.collider.GetComponent<BossController>().TakeDamage(this.Damage);
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
                GameObject.Destroy(this.gameObject);
            }
            else if (collision.collider.tag == "Obstacle")
            {
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            }

        }
    }
}
