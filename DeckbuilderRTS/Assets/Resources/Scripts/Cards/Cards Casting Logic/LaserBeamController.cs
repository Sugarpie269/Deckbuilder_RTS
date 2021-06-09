using UnityEngine;

using DeckbuilderRTS;
using System.Collections;

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
        private bool EnableTickCounter;
        private bool HurtPlayers;

        public LaserBeamController()
        {
            this.Damage = 20.0f;
            this.Delay = 1.0f; // time before damage is dealt (charge up)
            this.DelayCounter = 0.0f;
            this.TimeBetweenDamageTick = 0.25f;
            this.DamageDelayCounter = this.TimeBetweenDamageTick;
            this.Lifetime = 3.0f; // default amount of time the game object will allow damage to be dealt
            this.CanDamage = false;
            this.EnableTickCounter = false;
            this.HurtPlayers = false;
        }

        // The start function will initialize our member variables.
        public void Start()
        {
            StartCoroutine("FadeIn");
        }

        public void SetAttributes(float damage, float delay, float lifetime, float angle, bool hurtPlayers = false)
        {
            this.Damage = damage;
            this.Delay = delay;
            this.Lifetime = lifetime;
            this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            this.HurtPlayers = hurtPlayers;
        }

        public void Update()
        {
            if (this.EnableTickCounter)
            {
                this.DamageDelayCounter -= Time.deltaTime;
                if (this.DamageDelayCounter <= 0.0f)
                {
                    this.DamageDelayCounter += this.TimeBetweenDamageTick;
                    this.CanDamage = true;
                }
            }

            // after [Delay] seconds, enable damage
            if (!this.EnableTickCounter)
            {
                this.DelayCounter += Time.deltaTime;
                if (this.DelayCounter >= Delay)
                {
                    GameObject.Find("Card_LaserBeam").transform.GetChild(9).gameObject.GetComponent<AudioSource>().Play();

                    this.EnableTickCounter = true;
                    this.DelayCounter -= Delay;
                    this.DamageDelayCounter -= this.DelayCounter;
                }
            }
            
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
                yield return new WaitForSeconds(this.Delay/10f); 
                // ^ functionally this should ba 1/10th of a second
            }
        }

        IEnumerator DamageTick()
        {
            yield return 0;
            this.CanDamage = false;
        }

        public void LateUpdate()
        {
            // once delay counter reaches threshold, enable damage for this frame
            if (this.CanDamage)
            {
                
            }
        }
        

        private void OnTriggerStay2D(Collider2D other)
        {
            if (this.CanDamage)
            {
                if (other.gameObject.CompareTag("Swarmling"))
                {
                    other.gameObject.GetComponent<SwarmlingController>().TakeDamage(this.Damage);
                    Physics2D.IgnoreCollision(other, this.gameObject.GetComponent<Collider2D>());

                }
                else if (other.CompareTag("Slimeling"))
                {
                    other.GetComponent<SlimelingController>().TakeDamage(this.Damage);
                    Physics2D.IgnoreCollision(other, this.GetComponent<Collider2D>());
                }
                else if (other.gameObject.CompareTag("Boss"))
                {
                    other.gameObject.GetComponent<BossController>().TakeDamage(this.Damage);
                    Physics2D.IgnoreCollision(other, this.gameObject.GetComponent<Collider2D>());

                }
                else if (other.gameObject.CompareTag("Worker"))
                {
                    other.gameObject.GetComponent<WorkerController>().TakeDamage(this.Damage);
                    Physics2D.IgnoreCollision(other, this.gameObject.GetComponent<Collider2D>());

                }
                else if (other.gameObject.CompareTag("Player") && this.HurtPlayers)
                {
                    other.gameObject.GetComponent<PlayerController>().TakeDamage(this.Damage);
                    Physics2D.IgnoreCollision(other, this.gameObject.GetComponent<Collider2D>());
                }
                else if (other.gameObject.CompareTag("Obstacle"))
                {
                    other.gameObject.SetActive(false);
                }
            }
        }
    }
}
