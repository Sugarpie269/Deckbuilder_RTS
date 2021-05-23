using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class DepotController : MonoBehaviour
    {
        private enum DepotType {Matter, Energy, Mana};
        [SerializeField] private DepotType CurrentType;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //Debug.Log(collision.collider.name + " is the thing");
            if (collision.collider.tag == "Worker")
            {
                var workerController = collision.collider.GetComponent<WorkerController>();

                // If the worker isn't working matter, switch it to work matter.
                if (this.CurrentType == DepotType.Matter && !workerController.IsWorkingMatter())
                {
                    workerController.SetWorkingMatter();
                }
                else if (this.CurrentType == DepotType.Energy && !workerController.IsWorkingEnergy())
                {
                    workerController.SetWorkingEnergy();
                }
                else if (this.CurrentType == DepotType.Mana && !workerController.IsWorkingMana())
                {
                    workerController.SetWorkingMana();
                }
                
                //Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
                
            }

        }
    }
}

