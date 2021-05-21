using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class MovePlayerDownCommand : ScriptableObject, IPlayerCommand
    {

        // FIXME: Add option for speed changes?
        private float Speed = 5.0f;

        public void Execute(GameObject gameObject)
        {
            Debug.Log("Moving Down!");
            var rigidBody = gameObject.GetComponent<Rigidbody2D>();
            if (rigidBody != null)
            {
                if (gameObject.GetComponent<SpriteRenderer>().flipX == true) {
                    // Move Up Facing Left
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, -this.Speed);
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                else {
                    // Move Up Facing Right
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, -this.Speed);
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }
    }
}

