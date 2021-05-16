using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class SwarmlingController : MonoBehaviour
    {
        private IUnitCommand CurrentCommand;

        // The start function will initialize our member variables.
        void Start()
        {
            // this.CurrentCommand = ScriptableObject.CreateInstance<???>();
        }

        void Update()
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(-5.0f, 2.5f);
        }
    }
}

