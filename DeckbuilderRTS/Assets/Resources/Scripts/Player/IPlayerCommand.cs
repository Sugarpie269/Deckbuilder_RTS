using UnityEngine;

namespace DeckbuilderRTS
{
    public interface IPlayerCommand
    {
        public void Execute(GameObject gameObject);
    }
}