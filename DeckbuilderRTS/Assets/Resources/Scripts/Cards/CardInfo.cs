using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public struct CardInfo
    {
        public GameObject CardReference;
        public Sprite CardArt;
        public string CardName;
        public string CardType; // POTENTIALLY UNNEEDED? ~Liam
        public string DescriptionHeader;
        public string DescriptionContent;
        public string FlavorText;

        public int CardLevel;
        public int CardPower;
        public int CardStrength;

        public int ManaCost;
        public int EnergyCost;
        public int MatterCost;

        public AudioSource PlaySound;
    }
}