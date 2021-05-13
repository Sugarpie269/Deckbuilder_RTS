using UnityEngine;

namespace DeckbuilderRTS
{
    public class Inventory : MonoBehaviour
    {
        private ICard CardSlot1;
        private ICard CardSlot2;
        private ICard CardSlot3;

        // Start is called before the first frame update
        void Start()
        {
            this.CardSlot1 = null;
            this.CardSlot2 = null;
            this.CardSlot3 = null;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

