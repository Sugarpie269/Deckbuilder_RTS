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
            var rigidBody = gameObject.GetComponent<Rigidbody2D>();
            
            if (rigidBody != null)
            {
                if (gameObject.GetComponent<SpriteRenderer>().flipX == true) {
                    // Move Down Facing Left
                    Vector2 velocity = new Vector2(rigidBody.velocity.x, -this.Speed);
                    rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
                    
                    //gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                else {
                    // Move Down Facing Right
                    Vector2 velocity = new Vector2(rigidBody.velocity.x, -this.Speed);
                    rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
                    
                    //gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }
    }
}

