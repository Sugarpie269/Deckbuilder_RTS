using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class WorkerController : MonoBehaviour
    {
        private IUnitCommand CurrentCommand;
        [SerializeField] private GameObject Player;

        // The start function will initialize our member variables.
        void Start()
        {
            // this.CurrentCommand = ScriptableObject.CreateInstance<???>();
        }

        void Update()
        {
            transform.Rotate(Vector3.forward * (90 * Time.deltaTime));
            
        }

        public void SetPlayer(GameObject player)
        {
            this.Player = player;
        }
    }
}

