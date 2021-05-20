using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public interface ICard
    {
        void OnCardPlayed(GameObject player, Vector2 target);
        bool ShouldBeDestroyed();
        bool CanBeDestroyed();
        Texture2D GetCardImage();
    }
}

