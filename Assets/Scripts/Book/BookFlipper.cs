using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using System.IO;

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

    public UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button inputButtonRight;
    public UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button inputButtonLeft;

    public XRNode inputSource; // Set this to the right controller input source

    private bool isJoystickInUse = false;
    private float joystickThreshold = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Show the first page of spells
        ShowSpells();
    }

    public void Update()
    {
        // Get the joystick input value
        float joystickInput = Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal"); // Replace "JoystickHorizontal" with your joystick axis name

        // Check if the joystick input exceeds the threshold and handle page flipping
        if (joystickInput > joystickThreshold && !isJoystickInUse)
        {
            ShowNextPage();
            isJoystickInUse = true;
        }
        else if (joystickInput < -joystickThreshold && !isJoystickInUse)
        {
            ShowPreviousPage();
            isJoystickInUse = true;
        }
        else if (joystickInput >= -joystickThreshold && joystickInput <= joystickThreshold)
        {
            isJoystickInUse = false;
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
        if (currentPage < Mathf.CeilToInt((float)spellNames.Length / spellsPerPage) - 1)
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
