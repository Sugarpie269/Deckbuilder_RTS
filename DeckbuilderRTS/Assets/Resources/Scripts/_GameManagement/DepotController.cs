using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class DepotController : MonoBehaviour
    {
        private enum DepotType {Matter, Energy, Mana};
        [SerializeField] private DepotType CurrentType;
        private GameObject Worker = null;
        [SerializeField]
        private float ShineInterval = 2.0f;
        [SerializeField]
        private float upperBound = 1.0f; // between 0 and 1, > lowerbound
        [SerializeField]
        private float lowerBound = 0.75f; // between 0 and 1 <= uperbound
        //[SerializeField]
        //private GameObject Shiner;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine("ShineUp");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log(collision.collider.name + " is the thing");
            if (collision.gameObject.CompareTag("Worker"))
            {
                var workerController = collision.GetComponent<WorkerController>();
                if (this.Worker != null)
                {
                    workerController.SetWorkingBasic();
                    return;
                }

                this.Worker = workerController.gameObject;

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
            else
            {
                Physics2D.IgnoreCollision(collision, this.GetComponent<Collider2D>());
            }

        }

        IEnumerator ShineUp()
        {
            // todo change fadein effect to exponential?
            for (float ft = this.lowerBound; ft < this.upperBound; ft += 0.05f)
            {
                var renderer = this.gameObject.GetComponent<Renderer>();
                Color c = renderer.material.color;
                c.a = ft;
                renderer.material.color = c;
                yield return new WaitForSeconds(this.ShineInterval / 10f);
                // ^ functionally this should ba 1/10th of a second
            }
            StartCoroutine("ShineDown");
        }

        IEnumerator ShineDown()
        {
            // todo change fadein effect to exponential?
            for (float ft = this.upperBound; ft > this.lowerBound; ft -= 0.05f)
            {
                var renderer = this.gameObject.GetComponent<Renderer>();
                Color c = renderer.material.color;
                c.a = ft;
                renderer.material.color = c;
                yield return new WaitForSeconds(this.ShineInterval / 10f);
            }
            StartCoroutine("ShineUp");
        }

        IEnumerator ShineUpLow()
        {
            // todo change fadein effect to exponential?
            for (float ft = 0.0f; ft <= .25f; ft += 0.05f)
            {
                var renderer = this.gameObject.GetComponent<Renderer>();
                Color c = renderer.material.color;
                c.a = ft;
                renderer.material.color = c;
                yield return new WaitForSeconds(this.ShineInterval / 10f);
                // ^ functionally this should ba 1/10th of a second
            }
            StartCoroutine("ShineDownLow");
        }

        IEnumerator ShineDownLow()
        {
            // todo change fadein effect to exponential?
            for (float ft = 0.25f; ft >= 0.0f; ft -= 0.05f)
            {
                var renderer = this.gameObject.GetComponent<Renderer>();
                Color c = renderer.material.color;
                c.a = ft;
                renderer.material.color = c;
                yield return new WaitForSeconds(this.ShineInterval / 10f);
                // ^ functionally this should ba 1/10th of a second
            }
            StartCoroutine("ShineUpLow");
        }
    }
}

