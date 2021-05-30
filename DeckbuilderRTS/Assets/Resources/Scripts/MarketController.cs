using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS {
    public class MarketController : MonoBehaviour
    {
        bool cardSold = false;
        GameController gameController;
        public GameObject player;
        public ICard card;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision collision)
        {
            //display UI
            if (cardSold == true) {
                card = gameController.GenerateCardFireball();
            }
            

        }
    }
}
