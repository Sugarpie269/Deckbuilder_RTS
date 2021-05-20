using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class SwarmlingController : MonoBehaviour
    {
        private IUnitCommand CurrentCommand;
        private int CurrentHealth;
        private int MaxHealth = 10;

        // The start function will initialize our member variables.
        void Start()
        {
            // this.CurrentCommand = ScriptableObject.CreateInstance<???>();
            this.CurrentHealth = this.MaxHealth;
        }

        void Update()
        {

        }

        public void TakeDamage(float damage)
        {
            this.CurrentHealth -= Mathf.FloorToInt(damage);

            // If the swarmling dies, destroy the game object.
            if (this.CurrentHealth < 0)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}

