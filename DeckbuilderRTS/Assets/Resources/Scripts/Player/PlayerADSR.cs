using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeckbuilderRTS
{
    public class PlayerADSR
    {
        public enum MovementADSR { None, Attack, Sustain, Release };
        private float CurrentMovementAttackTime = 0.0f;
        private float MaxMovementAttackTime = 1f;
        private float CurrentMovementReleaseTime = 0.0f;
        private float MaxMovementReleaseTime = 0.1f;
        private MovementADSR CurrentMovementMode = MovementADSR.None;
        private float Multiplier = 1f;

        public PlayerADSR(float maxAttack, float maxRelease)
        {
            this.MaxMovementAttackTime = maxAttack;
            this.MaxMovementReleaseTime = maxRelease;
        }

        public float GetSpeedModifier()
        {
            if (this.CurrentMovementMode == MovementADSR.None)
            {
                //Debug.Log("none");
                return 0.0f;
            }
            else if (this.CurrentMovementMode == MovementADSR.Attack)
            {
                //Debug.Log("attack");
                // We are using a linear curve.~Jackson
                var attackRatio = this.CurrentMovementAttackTime / this.MaxMovementAttackTime;
                var speedPercentage = attackRatio;//Mathf.Log(attackRatio + 1f, 2f);
                return speedPercentage * this.Multiplier;
            }
            else if (this.CurrentMovementMode == MovementADSR.Sustain)
            {
                //Debug.Log("sustain");
                return this.Multiplier;
            }
            else if (this.CurrentMovementMode == MovementADSR.Release)
            {
                //Debug.Log("release");
                // We are using a linear curve again.~Jackson
                var releaseRatio = 1 - this.CurrentMovementReleaseTime / this.MaxMovementReleaseTime;
                var speedPercentage = releaseRatio;//Mathf.Log(releaseRatio + 1f, 2f);
                return speedPercentage * this.Multiplier;
            }
            return 0.0f;
        }

        public void Update(float input, float time)
        {
            if (this.CurrentMovementMode == MovementADSR.None)
            {
                if (input != 0.0f)
                {
                    this.CurrentMovementMode = MovementADSR.Attack;
                    this.SetDirection(input);
                }
            }
            else if (this.CurrentMovementMode == MovementADSR.Attack)
            {
                if (input != 0.0f)
                {
                    this.CurrentMovementAttackTime += time;
                    this.SetDirection(input);
                    if (this.CurrentMovementAttackTime >= this.MaxMovementAttackTime)
                    {
                        // If we have finished the attack time, then it is time to switch to sustain mode.
                        this.CurrentMovementAttackTime = 0.0f;
                        this.CurrentMovementMode = MovementADSR.Sustain;
                    }
                }
                else
                {
                    this.CurrentMovementAttackTime = 0.0f;
                    this.CurrentMovementMode = MovementADSR.None;
                }
            }
            else if (this.CurrentMovementMode == MovementADSR.Sustain)
            {
                if (input != 0.0f)
                {
                    this.SetDirection(input);
                }
                else
                {
                    this.CurrentMovementMode = MovementADSR.Release;
                }
            }
            else if (this.CurrentMovementMode == MovementADSR.Release)
            {
                if (input != 0.0f)
                {
                    this.CurrentMovementMode = MovementADSR.Attack;
                    this.SetDirection(input);
                }
                else
                {
                    this.CurrentMovementReleaseTime += Time.fixedDeltaTime;
                    if (this.CurrentMovementReleaseTime >= this.MaxMovementReleaseTime)
                    {
                        this.CurrentMovementReleaseTime = 0.0f;
                        this.CurrentMovementMode = MovementADSR.None;
                    }
                }
            }
        }

        public void SetDirection(float dir)
        {
            if (dir < 0.0f)
            {
                this.Multiplier = -1f;
            }
            else
            {
                this.Multiplier = 1f;
            }
        }

    }
}

