using UnityEngine;

using DeckbuilderRTS;

// Temporary citation = CaptainController.cs

namespace DeckbuilderRTS
{
    public class PlayerController : MonoBehaviour
    {
        private Inventory PlayerInventory;
        private IPlayerCommand MoveUp;
        private IPlayerCommand MoveDown;
        private IPlayerCommand MoveLeft;
        private IPlayerCommand MoveRight;
        private IPlayerCommand PlayCard1;
        private IPlayerCommand PlayCard2;
        private IPlayerCommand PlayCard3;

        // The start function will initialize our member variables.
        void Start()
        {
            this.PlayerInventory = this.gameObject.GetComponent<Inventory>();
            this.MoveUp = ScriptableObject.CreateInstance<MovePlayerUpCommand>();
            this.MoveDown = ScriptableObject.CreateInstance<MovePlayerDownCommand>();
            this.MoveLeft = ScriptableObject.CreateInstance<MovePlayerLeftCommand>();
            this.MoveRight = ScriptableObject.CreateInstance<MovePlayerRightCommand>();
            this.PlayCard1 = ScriptableObject.CreateInstance<PlayerCard1Command>();
            this.PlayCard2 = ScriptableObject.CreateInstance<PlayerCard2Command>();
            this.PlayCard3 = ScriptableObject.CreateInstance<PlayerCard3Command>();
        }

        // Navya, this is all you!
        void ProcessInput()
        {

        }

        void Update()
        {
            this.ProcessInput();
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("I took " + damage.ToString() + " damage!");
        }
    }
}
