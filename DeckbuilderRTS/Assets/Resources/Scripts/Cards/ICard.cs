using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public interface ICard
    {
        // GameObject getCardPrefab();
        void OnCardPlayed(GameObject player, Vector2 target);
        bool ShouldBeDestroyed();
        bool CanBeDestroyed();
        CardInfo GetCardInfo();

        // void DebugInfo(); // prints debug info for debugging
    }
}

