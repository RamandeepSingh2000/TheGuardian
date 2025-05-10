using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalCounter_UI : MonoBehaviour
{
    [Header("Portal Holder Configuration")]
    [SerializeField] private GameObject portalHolderPrefab;
    [SerializeField] private Texture2D portalOnImage;
    [SerializeField] private Texture2D portalOffImage;
    [SerializeField] private RectTransform portalHolderParent;

    private List<GameObject> portals;
    private List<RawImage> portalIcons;

    [Header("Portal Indicator Positioning")]
    [SerializeField] private float initialPosX = 25f;
    [SerializeField] private float posYOffset = -18f;
    [SerializeField] private float xSpacing = 20f;

    void Start()
    {
        portals = new List<GameObject>(GameObject.FindGameObjectsWithTag("Portal"));
        portalIcons = new List<RawImage>();

        // Injecting the portals with a deactivation script
        for (int i = 0; i < portals.Count; i++)
        {
            portals[i].AddComponent<DeactivatePortalScript>();

        }


        // Initialize each portalHolder
        for (int i = 0; i < portals.Count; i++)
        {
            GameObject portalHolder = Instantiate(portalHolderPrefab, portalHolderParent);
            RawImage portalIcon = portalHolder.GetComponent<RawImage>();

            // Moving the portal to a visible position
            RectTransform rectTransform = portalHolder.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(initialPosX + (i * xSpacing), posYOffset);

            portalIcon.texture = portals[i].activeSelf ? portalOnImage : portalOffImage;
            portalIcons.Add(portalIcon);

            // Subscribe to the DeactivatePortalScript
            DeactivatePortalScript deactivateScript = portals[i].GetComponent<DeactivatePortalScript>();
            if (deactivateScript != null)
            {
                deactivateScript.OnPortalDeactivated += () => UpdatePortalIcon(i, false);
                deactivateScript.OnPortalActivated += () => UpdatePortalIcon(i, true);
            }
        }
    }

    void UpdatePortalIcon(int index, bool isActive)
    {
        Debug.Log($"updating {index} of {portalIcons.Count}");
        if (index < 0 || index > portalIcons.Count) return;
        
        portalIcons[index-1].texture = isActive ? portalOnImage : portalOffImage;
    }
}
