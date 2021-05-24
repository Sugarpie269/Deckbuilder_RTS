using UnityEngine;

namespace DeckbuilderRTS
{
    public interface IUnitCommand
    {
        void Execute(GameObject gameObject);
    }
}