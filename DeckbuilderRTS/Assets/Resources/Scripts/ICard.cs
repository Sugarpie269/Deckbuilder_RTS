using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public interface ICard
    {
        string GetName();
        Vector3 GetCost();
        int GetPower();
        int GetStrength();
        void OnCardPlayed(GameObject player, Vector2 target);
        bool ShouldBeDestroyed();
        bool CanBeDestroyed();
        Sprite GetCardImage();

        // void DebugInfo(); // prints debug info for debugging
    }
}

