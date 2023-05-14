using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookFlipper : MonoBehaviour
{
    public TMP_Text leftPage;
    public TMP_Text rightPage;
    public Image leftImage;
    public Image rightImage;

    public string[] spellNames;
    public Sprite[] spriteName;

    private int currentPage = 0;
    private int spellsPerPage = 2;

    public Image HealthBar;

    public Image ManaBar;


    // Start is called before the first frame update
    void Start()
    {
    // Show the first page of spells
        ShowSpells();

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShowNextPage();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ShowPreviousPage();
        }
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        HealthBar.fillAmount = currentHealth / maxHealth;
    }

    public void UpdateManaBar(float maxMana, float currentMana)
    {
        ManaBar.fillAmount = currentMana / maxMana;
    }

     public void ShowNextPage()
    {
    if (currentPage <= spellNames.Length) 
        {
            currentPage++;
            ShowSpells();
         }
    }

    public void ShowPreviousPage()
    {
    if (currentPage > 0) 
        {
            currentPage--;
            ShowSpells();
        }
    }

    private void ShowSpells()
    {
        int startIndex = currentPage * spellsPerPage;

        // Update the left page
        leftPage.text = "";
        for (int i = startIndex; i < startIndex + spellsPerPage / 2 && i < spellNames.Length; i++)
        {
            leftPage.text += spellNames[i] + "\n";
            leftImage.sprite = spriteName[i];
        }

        // Update the right page
        rightPage.text = "";
        for (int i = startIndex + spellsPerPage / 2; i < startIndex + spellsPerPage && i < spellNames.Length; i++)
        {
            rightPage.text += spellNames[i] + "\n";
            rightImage.sprite = spriteName[i];

        }
    }
}