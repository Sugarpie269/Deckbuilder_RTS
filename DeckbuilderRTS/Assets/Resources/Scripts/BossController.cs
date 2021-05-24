using UnityEngine;
using DeckbuilderRTS;


namespace DeckbuilderRTS
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private int CurrentHP;
        [SerializeField] private int MaxHP = 1000;
        [SerializeField] GameObject EnemyPlayer;
        //private PlayerController EnemyPlayerController;
        private bool LoadedData = false;

        private void Start()
        {
        }

        private void Update()
        {
            if (!this.LoadedData)
            {
                //this.EnemyPlayerController = this.EnemyPlayer.GetComponent<PlayerController>();
                this.LoadedData = true;
            }

            this.UpdateFacing();
        }

        private void UpdateFacing()
        {
            var dirVec = new Vector2(this.EnemyPlayer.transform.position.x - this.gameObject.transform.position.x, this.EnemyPlayer.transform.position.y - this.gameObject.transform.position.y);

            //dirVec.Normalize();

            var multiplier = 0f;
            if (dirVec.x > 0)
            {
                multiplier = 1f;
            }
            if (dirVec.x != 0)
            {
                transform.eulerAngles = new Vector3(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, multiplier * 180f + (180 / Mathf.PI) * Mathf.Atan(dirVec.y / dirVec.x) + 90);
            }
            else if (dirVec.y > 0)
            {
                transform.eulerAngles = new Vector3(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, 180);
            }
            //transform.SetPositionAndRotation(this.transform.position, new Quaternion(0f, 0f, Mathf.Atan(dirVec.y / dirVec.x), 0f));
            //transform.Rotate(Vector3.forward * Mathf.Atan(dirVec.y/dirVec.x));
        }
    }
}
