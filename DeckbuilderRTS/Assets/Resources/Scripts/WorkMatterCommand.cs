using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class WorkMatterCommand : IUnitCommand
    {
        private float GenerateBasicMatterCoolDown = 5.0f;
        private float CurrentGenerateBasicMatterCoolDown;
        private int GenerateBasicMatterAmount = 3;

        public WorkMatterCommand()
        {
            this.CurrentGenerateBasicMatterCoolDown = this.GenerateBasicMatterCoolDown;
        }

        public void Execute(GameObject gameObject)
        {
            this.CurrentGenerateBasicMatterCoolDown -= Time.deltaTime;
            if (this.CurrentGenerateBasicMatterCoolDown <= 0)
            {
                this.CurrentGenerateBasicMatterCoolDown = this.GenerateBasicMatterCoolDown;
                var workerController = gameObject.GetComponent<WorkerController>();
                workerController.GetPlayerController().ModifyPlayerMatter(this.GenerateBasicMatterAmount);
            }
        }
    }
}

