using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class MovePlayerLeftCommand : ScriptableObject, IPlayerCommand
    {
        // FIXME: Add option for speed changes?
        private float Speed = 5.0f;
        
        public void Execute(GameObject gameObject)
        {
            var rigidBody = gameObject.GetComponent<Rigidbody2D>();

            if (rigidBody != null)
            {
                // this.Speed is positive to move in Left direction
                Vector2 velocity = new Vector2(-this.Speed, rigidBody.velocity.y);
                rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);

                //gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
}

