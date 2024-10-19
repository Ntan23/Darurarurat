using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using TMPro;
using System;

public class BuyPanelUI : MonoBehaviour
{
    [SerializeField] private LocalizedString buyString;
    [SerializeField] private TextMeshProUGUI buyText;
    private ShopManager sm;
    private bool isOpen;

    void Start() => sm = ShopManager.instance;

    void OnEnable()
    {
        buyString.Arguments = new string[2];
        buyString.StringChanged += UpdateBuyText;
    }

    void OnDisable() => buyString.StringChanged -= UpdateBuyText;

    private void UpdateBuyText(string value) => buyText.text = value;

    private void UpdateBackgroundAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void OpenBuyPanel() 
    {
        isOpen = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateBackgroundAlpha, 0.0f, 1.0f, 0.8f);
    }

    public void CloseBuyPanel()
    {
        LeanTween.value(gameObject, UpdateBackgroundAlpha, 1.0f, 0.0f, 0.8f).setOnComplete(() => {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            sm.SetCanInput(true);
            isOpen = false;
        });
    }

    public void UpdateText(string name, float price)
    {
        buyString.Arguments[0] = name.ToString();
        buyString.Arguments[1] = price.ToString("0.00");
        buyString.RefreshString();
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }
}