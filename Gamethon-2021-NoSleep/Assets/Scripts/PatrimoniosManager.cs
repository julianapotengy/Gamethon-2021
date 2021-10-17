using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PatrimoniosManager : MonoBehaviour
{
    public static PatrimoniosManager instance;

    public float storePrice, upgradeAPrice, upgradeBPrice;
    public float storeBonus;
    [HideInInspector] public string upgradeADesc;
    [HideInInspector] public string upgradeBDesc;
    public float updateABonus, updateBBonus;
    [HideInInspector] public bool hasStore = false;
    [HideInInspector] public bool hasUpgradeA = false;
    [HideInInspector] public bool hasUpgradeB = false;

    [Header(" REFERENCES ")]
    [SerializeField] TextMeshProUGUI storePriceText;
    [SerializeField] TextMeshProUGUI descUpAText;
    [SerializeField] TextMeshProUGUI descUpBText;
    [SerializeField] Button upgradeBButton;
    public Sprite[] storeImages;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        storePriceText.text = "R$" + storePrice;
        upgradeADesc = "Site\nAumento de " + updateABonus + " %\nPreço: R$" + upgradeAPrice;
        upgradeBDesc = "Entregas\nAumento de " + updateBBonus + " %\nPreço: R$" + upgradeBPrice;
        descUpAText.text = upgradeADesc;
        descUpBText.text = upgradeBDesc;

        if(!hasUpgradeA)
        {
            upgradeBButton.interactable = false;
        }
    }

    public void ReceiveMoney()
    {
        GameManager.instance.AddMoney(storeBonus);
    }

    public void BuyStore()
    {
        if (GameManager.instance.money >= storePrice && !hasStore)
        {
            hasStore = true;
            GameManager.instance.SubtractMoney(storePrice);
        }
    }

    public void UpgradeA()
    {
        if (GameManager.instance.money >= upgradeAPrice && hasStore && !hasUpgradeA)
        {
            storeBonus += updateABonus * (updateABonus / 100);
            GameManager.instance.SubtractMoney(upgradeAPrice);
            hasUpgradeA = true;
            upgradeBButton.interactable = true;
            ScreensControl.instance.ChangeStoreImage();
        }
    }

    public void UpgradeB()
    {
        if (GameManager.instance.money >= upgradeBPrice && hasStore && !hasUpgradeB && hasUpgradeA)
        {
            storeBonus += updateBBonus * (updateBBonus / 100);
            GameManager.instance.SubtractMoney(upgradeBPrice);
            hasUpgradeB = true;
            ScreensControl.instance.ChangeStoreImage();
        }
    }
}
