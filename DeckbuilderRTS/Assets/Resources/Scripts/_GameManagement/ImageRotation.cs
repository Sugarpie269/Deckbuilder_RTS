using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRotation : MonoBehaviour
{
    private Image Picture;
    
    // Start is called before the first frame update
    void Start()
    {
        this.Picture = this.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.Picture.transform.Rotate(new Vector3(0, 0, 3f));
    }
}
