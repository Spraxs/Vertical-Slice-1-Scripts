using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FillBar { BAR_1, BAR_2, BAR_3 }

public class ProgressBar : MonoBehaviour {

    public FillBar fillBar;
    private Image image;

    public float addedAmount;

    private float maxAmount;

    public bool canFill;

    [SerializeField]
    private Image progressPoint1;
    [SerializeField]
    private Image progressPoint2;

    public bool filling;

    void Start()
    {
        fillBar = FillBar.BAR_1;

        image = GetComponent<Image>();
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Horizontal;
        image.fillAmount = 0.833f;
        maxAmount = image.fillAmount;
    }

    void FixedUpdate()
    {
        if (filling && canFill)
        {
            FillBall();
        }

        if (image.fillAmount < maxAmount)
        {
            if (!filling) filling = true;

            if (image.fillAmount + addedAmount > maxAmount)
            {
                UpdateDisplay(maxAmount);
            } else
            {
                UpdateDisplay(image.fillAmount + addedAmount);
            }
        } else if (image.fillAmount == maxAmount && !canFill)
        {
            canFill = true;
        }
    }

    public void UpdateDisplay(float percentage)
    {
        image.fillAmount = percentage;
    }


    public void FillNext()
    {
        if (filling) return;

        switch (fillBar)
        {
            case FillBar.BAR_1:
                fillBar = FillBar.BAR_2;

                maxAmount = 0.915f;

                canFill = false;

                break;

            case FillBar.BAR_2:
                fillBar = FillBar.BAR_3;

                maxAmount = 1;

                canFill = false;
                break;
        }
    }

    private void FillBall()
    {
        Image imageToFade = null;

        if (fillBar == FillBar.BAR_2) imageToFade = progressPoint1;
        if (fillBar == FillBar.BAR_3) imageToFade = progressPoint2;

        if (imageToFade == null) return;

        Color imageColour = imageToFade.color;

        if (imageColour.a < 1)
        {
            imageColour.a += 0.2f;

            imageToFade.color = imageColour;
        } else
        {
            filling = false;
        }
    }
}