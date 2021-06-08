using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class WorkEnergyCommand : IUnitCommand
    {
        private float GenerateEnergyCoolDown = 2.0f;
        private float CurrentGenerateEnergyCoolDown;
        private int GenerateEnergyAmount = 1;

        public WorkEnergyCommand()
        {
            this.CurrentGenerateEnergyCoolDown = this.GenerateEnergyCoolDown;
        }

        public void Execute(GameObject gameObject)
        {
            this.CurrentGenerateEnergyCoolDown -= Time.deltaTime;
            if (this.CurrentGenerateEnergyCoolDown <= 0)
            {
                this.CurrentGenerateEnergyCoolDown = this.GenerateEnergyCoolDown;
                var workerController = gameObject.GetComponent<WorkerController>();
                workerController.GetPlayerController().ModifyPlayerEnergy(this.GenerateEnergyAmount);
            }
        }
    }
}

