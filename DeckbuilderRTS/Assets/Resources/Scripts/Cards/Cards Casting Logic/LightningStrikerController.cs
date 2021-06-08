using UnityEngine;

using DeckbuilderRTS;
using System.Collections;

namespace DeckbuilderRTS
{
    public class LightningStrikerController : MonoBehaviour
    {
        private float Damage;
        private float Delay;
        private float DamageDelay;
        private float DamageDelayCounter;
        private float Lifetime;
        private bool CanDamage;

        private Vector2 Velocity;

        public LightningStrikerController()
        {
            this.Damage = 10.0f;
            this.Delay = 2.0f;
            this.DamageDelay = 0.25f;
            this.DamageDelayCounter = 0.0f;
            this.Lifetime = 3.0f;
            this.CanDamage = false;

            this.Velocity = new Vector2(10.0f, 0.0f);
        }

        // The start function will initialize our member variables.
        public void Start()
        {
            StartCoroutine("FadeIn");
        }

        public void SetAttributes(float damage, float delay, float lifetime, float angle)
        {
            this.Damage = damage;
            this.Delay = delay;

            this.Lifetime = lifetime;
            //this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
            //this.Velocity = velocity;
        }

        public void Update()
        {
            this.DamageDelayCounter += Time.deltaTime;
            if (this.DamageDelayCounter >= Delay && this.CanDamage == false)
            {
                this.CanDamage = true;
                GameObject.Find("Card_LightningStriker").transform.GetChild(9).gameObject.GetComponent<AudioSource>().Play();
            }
            if (this.DamageDelayCounter >= this.Lifetime)
            {
                this.CanDamage = false;
                this.gameObject.SetActive(false);
            }
            //this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(this.Velocity.x, this.Velocity.y);
        }

        IEnumerator FadeIn()
        {
            // todo change fadein effect to exponential?
            for (float ft = 0.0f; ft <= 1f; ft += 0.1f)
            {
                var renderer = this.gameObject.GetComponent<Renderer>();
                Color c = renderer.material.color;
                c.a = ft;
                renderer.material.color = c;
                yield return new WaitForSeconds(this.Delay / 10f);
                // ^ functionally this should ba 1/5th of a second
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            
            if (this.CanDamage)
            {
                // If the fireball collides with a swarmling, the swarmling takes damage and knocks it back. Otherwise, the forcebolt is destroyed.
                if (collision.CompareTag("Swarmling"))
                {
                    //Debug.Log("Hit Swarmling for " + this.Damage);
                    collision.GetComponent<SwarmlingController>().TakeDamage(this.Damage);
                    // TODO: Add knockback to it
                    Physics2D.IgnoreCollision(collision, this.GetComponent<Collider2D>());
                    //GameObject.Destroy(this.gameObject);
                }
                /*else if (collision.collider.tag == "Player")
                {
                    collision.collider.GetComponent<PlayerController>().TakeDamage(this.Damage);
                    GameObject.Destroy(this.gameObject);
                }*/
                else if (collision.CompareTag("Boss"))
                {
                    //Debug.Log("Hit Boss for " + this.Damage);
                    collision.GetComponent<BossController>().TakeDamage(this.Damage);
                    Physics2D.IgnoreCollision(collision, this.GetComponent<Collider2D>());
                    //GameObject.Destroy(this.gameObject);
                }
                else if (collision.CompareTag("Obstacle"))
                {
                    //GameObject.Destroy(collision.gameObject);
                    collision.gameObject.SetActive(false);
                }
                else
                {
                    Physics2D.IgnoreCollision(collision, this.GetComponent<Collider2D>());
                }
            }

        }
    }
}
