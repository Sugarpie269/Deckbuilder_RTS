using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExamineDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject ExamineText;

    private bool PointerHovering;
    private bool isGameOver;

    void Start()
    {
        // Set the examine text to inactive. ~Liam
        this.ExamineText.SetActive(false);
        this.PointerHovering = false;
        this.isGameOver = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Removes the examine text related to this object when the pointer moves away. ~Liam
        if(!this.isGameOver)
        {
            this.ExamineText.SetActive(false);
            this.PointerHovering = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Displays the examine text related to this object while the pointer is hovering over it and the game is not over. ~Liam
        if(!this.isGameOver)
        {
            this.ExamineText.SetActive(true);
            this.PointerHovering = true;
        }
    }

    // Returns true while the pointer is hovering over the game object in question. ~Liam
    public bool IsPointerHovering()
    {
        return this.PointerHovering;
    }
    
    // Called when the player's game ends, to turn off tooltips and functionality. ~Liam
    public void GameOver()
    {
        this.isGameOver = true;
    }
}
