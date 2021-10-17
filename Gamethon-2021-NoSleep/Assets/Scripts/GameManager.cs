using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float money = 0;
    private float profit;
    public int day = 0;
    private int daysPassed;

    [Space(10)]
    [Header(" REFERENCES ")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI profitTextOnSell;
    [SerializeField] private TextMeshProUGUI dayText;

    [Space(10)]
    [Header(" COMPANIES ")]
    [SerializeField] public List<Company> companiesList;
    [HideInInspector] public Company selectedCompany;

    [Space(10)]
    [Header(" GRAPH ")]
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private Sprite dotSprite;
    [HideInInspector] public List<GameObject> selectedGraphObj;
    [SerializeField] private TextMeshProUGUI priceText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < companiesList.Count; i++)
        {
            companiesList[i].ChangeStateOnStart();
            companiesList[i].descText.text = companiesList[i].companyDescription;
        }
        PassDay();
    }

    void Update()
    {
        moneyText.text = "Carteira: " + money;
    }

    #region Time Pass
    public void PassDay()
    {
        day += 1;
        PassTime(1);
    }

    public void PassWeek()
    {
        day += 7;
        PassTime(7);
    }

    public void PassMonth()
    {
        day += 30;
        PassTime(30);
    }

    private void PassTime(int daysToPass)
    {
        for (int d = 0; d < daysToPass; d++)
        {
            for (int i = 0; i < companiesList.Count; i++)
            {
                companiesList[i].ChangeState();
                //companiesList[i].ChangePriceValue();
            }

            NewsManager.instance.RandomizeCompanyNews();

            for (int i = 0; i < companiesList.Count; i++)
            {
                companiesList[i].ChangePriceValue();
            }

            ChangeGraph(selectedCompany.lastWeekPrice);
        }
        dayText.text = "Dia: " + day;
        NewsManager.instance.ShowTodayNews();

        if(PatrimoniosManager.instance.hasStore)
        {
            daysPassed += daysToPass;
            while (daysPassed >= 7)
            {
                PatrimoniosManager.instance.ReceiveMoney();
                daysPassed -= 7;
            }
        }

        ScreensControl.instance.ChangeSellInfo();
        ScreensControl.instance.ChangeBuyInfo();
    }
    #endregion

    #region Graph
    private GameObject CreateDot(Vector2 anchoredPos)
    {
        GameObject gameObj = new GameObject("dot", typeof(Image));
        selectedGraphObj.Add(gameObj);
        gameObj.transform.SetParent(graphContainer, false);
        gameObj.GetComponent<Image>().sprite = dotSprite;
        gameObj.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        RectTransform rectTrans = gameObj.GetComponent<RectTransform>();
        rectTrans.anchoredPosition = anchoredPos;
        rectTrans.sizeDelta = new Vector2(15, 15);
        rectTrans.anchorMin = new Vector2(0, 0);
        rectTrans.anchorMax = new Vector2(0, 0);
        return gameObj;
    }

    public void ChangeGraph(List<float> positionsValue)
    {
        while(selectedGraphObj.Count > 0)
        {
            Destroy(selectedGraphObj[0]);
            selectedGraphObj.RemoveAt(0);
        }
        float yMin = Mathf.Min(selectedCompany.lastWeekPrice.ToArray());
        float graphHeight = graphContainer.sizeDelta.y;
        
        float yMax = Mathf.Max(selectedCompany.lastWeekPrice.ToArray());
        //Debug.Log(yMax);
        float xSize = 30;

        GameObject lastDotObj = null;
        for(int i = 0; i < positionsValue.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (positionsValue[i] / yMax) * graphHeight;
            GameObject dotGameObject = CreateDot(new Vector2(xPosition, yPosition));

            if(lastDotObj != null)
            {
                CreateDotConnection(lastDotObj.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastDotObj = dotGameObject;
        }

        priceText.text = "R$" + selectedCompany.price;
    }
    
    private void CreateDotConnection(Vector2 positionA, Vector2 positionB)
    {
        GameObject dotConnection = new GameObject("dotConnection", typeof(Image));
        selectedGraphObj.Add(dotConnection);
        dotConnection.transform.SetParent(graphContainer, false);
        dotConnection.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        RectTransform dotRectTransform = dotConnection.GetComponent<RectTransform>();
        Vector2 dir = (positionB - positionA).normalized;
        float distance = Vector2.Distance(positionA, positionB);
        dotRectTransform.anchorMin = new Vector2(0, 0);
        dotRectTransform.anchorMax = new Vector2(0, 0);
        dotRectTransform.sizeDelta = new Vector2(distance, 3);
        dotRectTransform.anchoredPosition = positionA + dir * distance / 2;
        dotRectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
    #endregion

    private void UpdateProfitText()
    {
        string newText = "";

        if(profit > 0) // pra depois: texto ficar verde
        {
            newText = "+" + profit;
        }
        else if(profit < 0) // pra depois: texto ficar vermelho
        {
            newText = "" + profit;
        }
        else if(profit == 0) // pra depois: texto ficar em alguma cor neutra (cinza, preto, branco, depende do fundo)
        {
            newText = "+" + profit;
        }
        profitTextOnSell.text = newText;
    }

    public void ConfirmBuy()
    {
        if(money >= selectedCompany.price)
        {
            selectedCompany.qntBought += ScreensControl.instance.qntToBuy();
            for (int i = 0; i < ScreensControl.instance.qntToBuy(); i++)
            {
                selectedCompany.priceBought.Add(selectedCompany.price);
                SubtractMoney(selectedCompany.price);
            }
        }

        ScreensControl.instance.ChangeBuyInfo();
    }

    public void ConfirmSell()
    {
        for(int i = 0; i < ScreensControl.instance.qntToSell(); i++)
        {
            if (selectedCompany.priceBought[0] >= 0)
            {
                AddMoney(selectedCompany.price);
            }
            else // se o jogador vender quando o lucro for negativo, vai diminuir o dinheiro
            {
                SubtractMoney(selectedCompany.price);
            }
            selectedCompany.priceBought.RemoveAt(0);
        }
        selectedCompany.qntBought -= ScreensControl.instance.qntToSell();

        ScreensControl.instance.ChangeSellInfo();
    }

    public void AddMoney(float valueToAdd)
    {
        money += valueToAdd;
    }
    public void SubtractMoney(float valueToSubtract)
    {
        money -= valueToSubtract;
    }
}
