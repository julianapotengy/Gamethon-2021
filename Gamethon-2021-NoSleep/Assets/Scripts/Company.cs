using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Company
{
    public enum Trend { Up, Stabilized, Down}

    public string companyName;
    public string companyDescription;
    public Sprite companyIcon;

    [Space(10)]
    public Trend companyState;
    public float price;
    public List<float> lastWeekPrice;

    [Space(10)]
    public int qntBought;
    public List<float> priceBought = new List<float>();

    [Space(10)]
    [Header(" NEWS ")]
    public List<string> upNews;
    public List<string> downNews;

    [Space(10)]
    [Header(" OBJECTS REFERENCES ")]
    public TextMeshProUGUI descText;

    public void ChangePriceValue()
    {
        switch(companyState)
        {
            case Trend.Up:
                price += Random.Range(0f, 0.75f);
                break;
            case Trend.Stabilized:
                price += Random.Range(-0.3f, 0.3f);
                break;
            case Trend.Down:
                price += Random.Range(0, -0.75f);
                break;
        }

        if(lastWeekPrice.Count >= 15)
        {
            lastWeekPrice.RemoveAt(0);
        }
        lastWeekPrice.Add(price);
    }

    public void ChangeStateOnStart()
    {
        int randToChange = Random.Range(0, 3);
        switch (randToChange)
        {
            case 0:
                companyState = Trend.Up;
                break;
            case 1:
                companyState = Trend.Stabilized;
                break;
            case 2:
                companyState = Trend.Down;
                break;
        }
    }

    public void ChangeState()
    {
        int rand = Random.Range(0, 3);
        if(rand == 0) // tem 1/3 de chance de mudar de estado
        {
            int randToChange = Random.Range(0, 3);
            switch (randToChange)
            {
                case 0:
                    companyState = Trend.Up;
                    break;
                case 1:
                    companyState = Trend.Stabilized;
                    break;
                case 2:
                    companyState = Trend.Down;
                    break;
            }
        }
    }

    public void ChangeState(int stateIndex)
    {
        switch (stateIndex)
        {
            case 0:
                companyState = Trend.Up;
                break;
            case 1:
                companyState = Trend.Stabilized;
                break;
            case 2:
                companyState = Trend.Down;
                break;
        }
    }

    public string randomNews()
    {
        int randTrend = Random.Range(0, 2);
        randTrend = 0;
        switch (randTrend)
        {
            case 0:
                companyState = Trend.Up;
                return upNews[Random.Range(0, upNews.Count)];
            case 1:
                companyState = Trend.Down;
                return downNews[Random.Range(0, downNews.Count)];
        }
        return "";
    }

    public string randomUpNews()
    {
        companyState = Trend.Up;
        return upNews[Random.Range(0, upNews.Count)];
    }

    public string randomDownNews()
    {
        companyState = Trend.Down;
        return downNews[Random.Range(0, downNews.Count)];
    }
}
