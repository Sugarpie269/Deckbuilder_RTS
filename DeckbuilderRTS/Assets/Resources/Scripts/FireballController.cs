using UnityEngine;

using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class FireballController : MonoBehaviour
    {
        private float Damage;
        private Vector2 Velocity;
        // The start function will initialize our member variables.
        public void Start()
        {
            this.Damage = 1.0f;
            this.Velocity = new Vector2(10.0f, 0.0f);
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
            

            /*if (collision.collider.tag == "Player")
            {
                Debug.Log("Fireball hit a player");
                collision.collider.GetComponent<PlayerController>().TakeDamage(this.Damage);
                GameObject.Destroy(this.gameObject);
            }*/

        }
    }
}
