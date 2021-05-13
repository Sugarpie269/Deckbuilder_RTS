using UnityEngine;

namespace DeckbuilderRTS
{
    public interface IUnitCommand
    {
        public void Execute(GameObject gameObject);
    }
}