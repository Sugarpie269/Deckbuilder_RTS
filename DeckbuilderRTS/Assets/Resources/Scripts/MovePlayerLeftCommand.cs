using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DeckbuilderRTS;

namespace DeckbuilderRTS
{
    public class MovePlayerLeftCommand : ScriptableObject, IPlayerCommand
    {

        // FIXME: Add option for speed changes?
        private float Speed = 1.5f;
        
        public void Execute(GameObject gameObject)
        {
            Debug.Log("Left.Execute()");
            var rigidBody = gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("Rigidbody is " + rigidBody.velocity);

            if (rigidBody != null)
            {
                rigidBody.velocity = new Vector2(-this.Speed, rigidBody.velocity.y);
                //rigidBody.position = new Vector2((-this.Speed) * Time.deltaTime, rigidBody.position.y);
                Debug.Log("Moving Left to " + rigidBody.velocity);

                //rigidBody.velocity = new Vector2(0, 0);
                //Debug.Log("Moving Left to " + rigidBody.velocity);
                /*
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                */
            }

            //rigidBody.velocity = new Vector2(0.0f, 0.0f);
        }
    }
}

