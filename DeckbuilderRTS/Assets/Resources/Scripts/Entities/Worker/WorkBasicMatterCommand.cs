using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class WorkBasicMatterCommand : IUnitCommand
    {
        private float GenerateBasicMatterCoolDown = 5.0f;
        private float CurrentGenerateBasicMatterCoolDown;
        private int GenerateBasicMatterAmount = 1;

        public WorkBasicMatterCommand()
        {
            this.CurrentGenerateBasicMatterCoolDown = this.GenerateBasicMatterCoolDown;
        }

        public void Execute(GameObject gameObject)
        {
        }
    }
}

