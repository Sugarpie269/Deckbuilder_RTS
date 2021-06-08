using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class WorkMatterCommand : IUnitCommand
    {
        private float GenerateMatterCoolDown = 5.0f;
        private float CurrentGenerateMatterCoolDown;
        private int GenerateMatterAmount = 1;

        public WorkMatterCommand()
        {
            this.CurrentGenerateMatterCoolDown = this.GenerateMatterCoolDown;
        }

        public void Execute(GameObject gameObject)
        {
            this.CurrentGenerateMatterCoolDown -= Time.deltaTime;
            if (this.CurrentGenerateMatterCoolDown <= 0)
            {
                this.CurrentGenerateMatterCoolDown = this.GenerateMatterCoolDown;
                var workerController = gameObject.GetComponent<WorkerController>();
                workerController.GetPlayerController().ModifyPlayerMatter(this.GenerateMatterAmount);
            }
        }
    }
}

