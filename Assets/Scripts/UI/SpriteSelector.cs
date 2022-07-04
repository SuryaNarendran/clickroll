using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteSelector : MonoBehaviour
{
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite unselectedSprite;
    [SerializeField] Image targetImage;

    public void SetSelectedSprite()
    {
        targetImage.sprite = selectedSprite;
    }
    public void SetUnselectedSprite()
    {
        targetImage.sprite = unselectedSprite;
    }
}
