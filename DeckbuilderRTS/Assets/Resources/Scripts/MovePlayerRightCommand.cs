using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class MovePlayerRightCommand : ScriptableObject, IPlayerCommand
    {

        // FIXME: Add option for speed changes?
        private float Speed = 1.5f;

        public void Execute(GameObject gameObject)
        {
            Debug.Log("Moving Right!");
            var rigidBody = gameObject.GetComponent<Rigidbody2D>();
            if (rigidBody != null)
            {
                // this.Speed is positive to move in Right direction
                rigidBody.velocity = new Vector2(this.Speed, rigidBody.velocity.y);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
}

