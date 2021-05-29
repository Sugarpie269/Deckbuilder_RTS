using UnityEngine;

using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class SwarmlingBulletController : MonoBehaviour
    {
        private float Damage;
        private Vector2 Velocity;

        public SwarmlingBulletController()
        {
            this.Damage = 10.0f;
            this.Velocity = new Vector2(10.0f, 0.0f);
        }

        // The start function will initialize our member variables.
        public void Start()
        {
        }

        public void SetAttributes(float damage, Vector2 velocity)
        {
            this.Damage = damage;
            this.Velocity = velocity;

        }

        public void Update()
        {
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(this.Velocity.x, this.Velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Player")
            {
                collision.collider.GetComponent<PlayerController>().TakeDamage(this.Damage);
                GameObject.Destroy(this.gameObject);
            }
            else if (collision.collider.tag == "Worker")
            {
                collision.collider.GetComponent<WorkerController>().TakeDamage(this.Damage);
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
