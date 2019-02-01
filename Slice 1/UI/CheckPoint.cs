using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform icon;
    private Image iconImage;
    private Canvas mainCanvas;
    private Vector3 cameraOffsetUp;
    private Vector3 cameraOffsetRight;
    private Vector3 cameraOffsetForward;
    [SerializeField]
    private Sprite checkPoint;
    [SerializeField]
    private Sprite CheckPointDone;

    [Space]
    [Range(0, 100), SerializeField]
    private float edgeBuffer;

    [SerializeField]
    private float checkpointScale;
    [Space]
    public bool ShowDebugLines;

    private float distance;

    private CheckPointState checkPointState;

    [SerializeField]
    private float transition;

    private RectTransform distanceObject;
    private Text distanceText;

    private RectTransform stateObject;
    private Text stateText;

    public Font font;


    void Start()
    {
        mainCamera = Camera.main;
        mainCanvas = FindObjectOfType<Canvas>();
        Debug.Assert((mainCanvas != null), "There needs to be a Canvas object in the scene for the OTI to display");
        InstainateTargetIcon();
        InstainateDistance();
        InstainateState();

        checkPointState = CheckPointState.OPEN;
    }

    void Update()
    {

        UpdateTargetIconPosition();
        distance = Vector3.Distance(gameObject.transform.position, mainCamera.transform.position);

        if (distance < 20 && checkPointState == CheckPointState.OPEN)
        {
            checkPointState = CheckPointState.SMALLER;
            stateText.text = "REACHED";
        }
    }

    void FixedUpdate()
    {

        distanceText.text = Mathf.Round(distance) + "m";

        if (checkPointState == CheckPointState.SMALLER)
        {
            if (icon.transform.localScale.x > 0)
            {
                Vector3 scale = icon.transform.localScale;

                scale.x -= transition;
                scale.y -= transition;
                scale.z -= transition;

                icon.transform.localScale = scale;

            } else
            {
                iconImage.sprite = CheckPointDone;
                checkPointState = CheckPointState.BIGGER;
            }
        } else

        if (checkPointState == CheckPointState.BIGGER)
        {
            if (icon.transform.localScale.x < 1)
            {
                Vector3 scale = icon.transform.localScale;

                scale.x += transition;
                scale.y += transition;
                scale.z += transition;

                icon.transform.localScale = scale;
            } else
            {
                icon.transform.localScale = new Vector3(checkpointScale, checkpointScale);

                checkPointState = CheckPointState.CLOSING;

            }
        } else

        if (checkPointState == CheckPointState.CLOSING)
        {
            if (icon.transform.localScale.x > 0)
            {
                Vector3 scale = icon.transform.localScale;

                scale.x -= transition;
                scale.y -= transition;
                scale.z -= transition;

                icon.transform.localScale = scale;
            } else
            {

                distanceObject.SetParent(null);
                stateObject.SetParent(null);
                icon.transform.SetParent(null);

                Destroy(icon.gameObject);
                Destroy(distanceObject.gameObject);
                Destroy(stateObject.gameObject);

                Destroy(gameObject);

                //TODO NEXT Checkpoint
            }
        }
    }

    private void InstainateState()
    {
        stateObject = new GameObject().AddComponent<RectTransform>();
        stateObject.transform.SetParent(mainCanvas.transform);
        stateObject.localScale = new Vector3(.2f, .2f);
        stateObject.name = name + ": OTI Text";
        stateText = stateObject.gameObject.AddComponent<Text>();
        stateText.font = font;
        stateText.horizontalOverflow = HorizontalWrapMode.Overflow;
        stateText.verticalOverflow = VerticalWrapMode.Overflow;
        stateText.text = "CHECKPOINT";
        stateText.fontSize = 100;
        stateText.alignment = TextAnchor.LowerCenter;
    }

    private void InstainateDistance()
    {
        distanceObject = new GameObject().AddComponent<RectTransform>();
        distanceObject.transform.SetParent(mainCanvas.transform);
        distanceObject.localScale = new Vector3(.2f, .2f);
        distanceObject.name = name + ": OTI Text";
        distanceText = distanceObject.gameObject.AddComponent<Text>();
        distanceText.font = font;
        distanceText.horizontalOverflow = HorizontalWrapMode.Overflow;
        distanceText.verticalOverflow = VerticalWrapMode.Overflow;
        distanceText.text = "0m";
        distanceText.fontSize = 100;
        distanceText.alignment = TextAnchor.LowerCenter;
    }


    private void InstainateTargetIcon()
    {
        icon = new GameObject().AddComponent<RectTransform>();
        icon.transform.SetParent(mainCanvas.transform);
        icon.localScale = new Vector3(checkpointScale, checkpointScale);
        icon.name = name + ": OTI icon";
        iconImage = icon.gameObject.AddComponent<Image>();
        iconImage.sprite = checkPoint;
    }


    private void UpdateTargetIconPosition()
    {
        Vector3 newPos = transform.position;
        newPos = mainCamera.WorldToViewportPoint(newPos);
        if (newPos.z < 0)
        {
            newPos.x = 1f - newPos.x;
            newPos.y = 1f - newPos.y;
            newPos.z = 0;
            newPos = Vector3Maxamize(newPos);
        }
        newPos = mainCamera.ViewportToScreenPoint(newPos);
        newPos.x = Mathf.Clamp(newPos.x, edgeBuffer, Screen.width - edgeBuffer);
        newPos.y = Mathf.Clamp(newPos.y, edgeBuffer, Screen.height - edgeBuffer);
        icon.transform.position = newPos;

        newPos.y -= 40;
        
        stateObject.transform.position = newPos;

        newPos.y -= 25;

        distanceObject.transform.position = newPos;


    }


    public Vector3 Vector3Maxamize(Vector3 vector)
    {
        Vector3 returnVector = vector;
        float max = 0;
        max = vector.x > max ? vector.x : max;
        max = vector.y > max ? vector.y : max;
        max = vector.z > max ? vector.z : max;
        returnVector /= max;
        return returnVector;
    }
}

public enum CheckPointState
{
    OPEN, SMALLER, BIGGER, CLOSING
}