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

        // The start function will initialize our member variables.
        void Start()
        {
            this.PlayerInventory = this.gameObject.GetComponent<Inventory>();
            this.MoveUp = ScriptableObject.CreateInstance<MovePlayerUpCommand>();
            this.MoveDown = ScriptableObject.CreateInstance<MovePlayerDownCommand>();
            this.MoveLeft = ScriptableObject.CreateInstance<MovePlayerLeftCommand>();
            this.MoveRight = ScriptableObject.CreateInstance<MovePlayerRightCommand>();
        }

        // Navya, this is all you!
        void ProcessInput()
        {

        }

        void Update()
        {
            this.ProcessInput();
        }
    }
}
