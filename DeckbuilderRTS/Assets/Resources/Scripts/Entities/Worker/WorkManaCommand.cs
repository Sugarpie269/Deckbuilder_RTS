using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class WorkManaCommand : IUnitCommand
    {
        private float GenerateManaCoolDown = 2.0f;
        private float CurrentGenerateManaCoolDown;
        private int GenerateManaAmount = 1;

        public WorkManaCommand()
        {
            this.CurrentGenerateManaCoolDown = this.GenerateManaCoolDown;
        }

        public void Execute(GameObject gameObject)
        {
            this.CurrentGenerateManaCoolDown -= Time.deltaTime;
            if (this.CurrentGenerateManaCoolDown <= 0)
            {
                this.CurrentGenerateManaCoolDown = this.GenerateManaCoolDown;
                var workerController = gameObject.GetComponent<WorkerController>();
                workerController.GetPlayerController().ModifyPlayerMana(this.GenerateManaAmount);
            }
        }
    }
}

