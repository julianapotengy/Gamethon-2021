using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScreensControl : MonoBehaviour
{
    public static ScreensControl instance;

    [Space(10)]
    [Header(" BUY PANEL ")]
    [SerializeField] GameObject buyPanel;
    [SerializeField] TextMeshProUGUI qntTextOnBuy;
    [SerializeField] TextMeshProUGUI hundredTextOnBuy;
    [SerializeField] TextMeshProUGUI tenTextOnBuy;
    [SerializeField] TextMeshProUGUI unitTextOnBuy;

    [Space(10)]
    [Header(" SELL PANEL ")]
    [SerializeField] GameObject sellPanel;
    [SerializeField] TextMeshProUGUI qntTextOnSell;
    [SerializeField] TextMeshProUGUI hundredTextOnSell;
    [SerializeField] TextMeshProUGUI tenTextOnSell;
    [SerializeField] TextMeshProUGUI unitTextOnSell;

    [Space(10)]
    [Header(" NEWS PANEL ")]
    [SerializeField] GameObject newsPanel;
    [SerializeField] TextMeshProUGUI newsText;
    [SerializeField] TextMeshProUGUI lastNewsDayText;

    [Space(10)]
    [Header(" SELECT COMPANY ")]
    [SerializeField] GameObject selectCompanyPanel;
    [SerializeField] GameObject followPanel;
    [SerializeField] TextMeshProUGUI companyDescText;
    [SerializeField] Image companyImage;

    [Space(10)]
    [Header(" PATRIMONIOS ")]
    [SerializeField] GameObject patrimoniosPanel;
    [SerializeField] GameObject storePanel;
    [SerializeField] Image storeImages;

    private void Awake()
    {
        instance = this;
    }

    public void OpenPatrimonios()
    {
        patrimoniosPanel.SetActive(true);
        newsPanel.SetActive(false);
        storePanel.SetActive(false);
    }

    public void CloseStore()
    {
        OpenPatrimonios();
    }

    public void OpenStore()
    {
        patrimoniosPanel.SetActive(false);
        storePanel.SetActive(true);
        newsPanel.SetActive(false);

        ChangeStoreImage();
    }

    public void ChangeStoreImage()
    {
        if (PatrimoniosManager.instance.hasStore)
        {
            if (PatrimoniosManager.instance.hasUpgradeA && !PatrimoniosManager.instance.hasUpgradeB)
            {
                storeImages.sprite = PatrimoniosManager.instance.storeImages[1];
            }
            else if (PatrimoniosManager.instance.hasUpgradeB)
            {
                storeImages.sprite = PatrimoniosManager.instance.storeImages[2];
            }
            else
            {
                storeImages.sprite = PatrimoniosManager.instance.storeImages[0];
            }
        }
    }

    public void FollowCompany(int companyIndex)
    {
        GameManager.instance.selectedCompany = GameManager.instance.companiesList[companyIndex];
        companyDescText.text = GameManager.instance.companiesList[companyIndex].companyDescription;
        companyImage.sprite = GameManager.instance.companiesList[companyIndex].companyIcon;
        followPanel.SetActive(true);
        GameManager.instance.ChangeGraph(GameManager.instance.selectedCompany.lastWeekPrice);
        ChangeSellInfo();
        ChangeBuyInfo();
    }

    public void OpenNewsPanel()
    {
        ChangeNewsInfo();
        newsPanel.SetActive(true);
    }

    public void ChangeNewsInfo()
    {
        newsText.text = NewsManager.instance.todayNews;
        lastNewsDayText.text = "Dia " + NewsManager.instance.lastNewsDay;
    }

    public void OpenSellPanel()
    {
        sellPanel.SetActive(true);
        buyPanel.SetActive(false);
        ChangeSellInfo();
        ChangeBuyInfo();
    }

    public void OpenBuyPanel()
    {
        buyPanel.SetActive(true);
        sellPanel.SetActive(false);
        ChangeBuyInfo();
        ChangeSellInfo();
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    #region Buy
    public void ChangeBuyInfo()
    {
        float qntBought = GameManager.instance.selectedCompany.qntBought;
        qntTextOnBuy.text = "Quantidade: " + qntBought;
        hundredTextOnBuy.text = "" + 0;
        tenTextOnBuy.text = "" + 0;
        unitTextOnBuy.text = "" + 0;
    }

    public int qntToBuy()
    {
        return int.Parse(hundredTextOnBuy.text + tenTextOnBuy.text + unitTextOnBuy.text);
    }

    public void AddNumerOnBuy(int whatIs)
    {
        switch (whatIs)
        {
            case 1:
                if (canAddNumberOnBuy(0, 0, 1) && int.Parse(unitTextOnBuy.text) + 1 < 10)
                {
                    unitTextOnBuy.text = int.Parse(unitTextOnBuy.text) + 1 + "";
                }
                break;
            case 10:
                if (canAddNumberOnBuy(0, 1, 0) && int.Parse(tenTextOnBuy.text) + 1 < 10)
                {
                    tenTextOnBuy.text = int.Parse(tenTextOnBuy.text) + 1 + "";
                }
                break;
            case 100:
                if (canAddNumberOnBuy(1, 0, 0) && int.Parse(hundredTextOnBuy.text) + 1 < 10)
                {
                    hundredTextOnBuy.text = int.Parse(hundredTextOnBuy.text) + 1 + "";
                }
                break;
        }
    }

    private bool canAddNumberOnBuy(int hundred, int ten, int unit)
    {
        int hundredNumber = int.Parse(hundredTextOnBuy.text) + hundred;
        int tenNumber = int.Parse(tenTextOnBuy.text) + ten;
        int unitNumber = int.Parse(unitTextOnBuy.text) + unit;
        string finalNumber = hundredNumber.ToString() + tenNumber.ToString() + unitNumber.ToString();
        
        if (int.Parse(finalNumber) * GameManager.instance.selectedCompany.price <= GameManager.instance.money)
        {
            return true;
        }
        else return false;
    }

    public void RemoveNumerOnBuy(int whatIs)
    {
        switch (whatIs)
        {
            case 1:
                if (int.Parse(unitTextOnBuy.text) - 1 >= 0)
                {
                    unitTextOnBuy.text = int.Parse(unitTextOnBuy.text) - 1 + "";
                }
                break;
            case 10:
                if (int.Parse(tenTextOnBuy.text) - 1 >= 0)
                {
                    tenTextOnBuy.text = int.Parse(tenTextOnBuy.text) - 1 + "";
                }
                break;
            case 100:
                if (int.Parse(hundredTextOnBuy.text) - 1 >= 0)
                {
                    hundredTextOnBuy.text = int.Parse(hundredTextOnBuy.text) - 1 + "";
                }
                break;
        }
    }
    #endregion

    #region Sell
    public void ChangeSellInfo()
    {
        float qntBought = GameManager.instance.selectedCompany.qntBought;
        qntTextOnSell.text = "Quantidade: " + qntBought;
        hundredTextOnSell.text = "" + 0;
        tenTextOnSell.text = "" + 0;
        unitTextOnSell.text = "" + 0;
    }

    public int qntToSell()
    {
        return int.Parse(hundredTextOnSell.text + tenTextOnSell.text + unitTextOnSell.text);
    }

    public void AddNumer(int whatIs)
    {
        switch(whatIs)
        {
            case 1:
                if(canAddNumber(0, 0, 1) && int.Parse(unitTextOnSell.text) + 1 < 10)
                {
                    unitTextOnSell.text = int.Parse(unitTextOnSell.text) + 1 + "";
                }
                break;
            case 10:
                if (canAddNumber(0, 1, 0) && int.Parse(tenTextOnSell.text) + 1 < 10)
                {
                    tenTextOnSell.text = int.Parse(tenTextOnSell.text) + 1 + "";
                }
                break;
            case 100:
                if (canAddNumber(1, 0, 0) && int.Parse(hundredTextOnSell.text) + 1 < 10)
                {
                    hundredTextOnSell.text = int.Parse(hundredTextOnSell.text) + 1 + "";
                }
                break;
        }
    }

    private bool canAddNumber(int hundred, int ten, int unit)
    {
        int hundredNumber = int.Parse(hundredTextOnSell.text) + hundred;
        int tenNumber = int.Parse(tenTextOnSell.text) + ten;
        int unitNumber = int.Parse(unitTextOnSell.text) + unit;
        string finalNumber = hundredNumber.ToString() + tenNumber.ToString() + unitNumber.ToString();
        
        if (int.Parse(finalNumber) <= GameManager.instance.selectedCompany.qntBought)
        {
            return true;
        }
        else return false;
    }

    public void RemoveNumer(int whatIs)
    {
        switch (whatIs)
        {
            case 1:
                if (int.Parse(unitTextOnSell.text) - 1 >= 0)
                {
                    unitTextOnSell.text = int.Parse(unitTextOnSell.text) - 1 + "";
                }
                break;
            case 10:
                if (int.Parse(tenTextOnSell.text) - 1 >= 0)
                {
                    tenTextOnSell.text = int.Parse(tenTextOnSell.text) - 1 + "";
                }
                break;
            case 100:
                if(int.Parse(hundredTextOnSell.text) - 1 >= 0)
                {
                    hundredTextOnSell.text = int.Parse(hundredTextOnSell.text) - 1 + "";
                }
                break;
        }
    }
    #endregion
}
