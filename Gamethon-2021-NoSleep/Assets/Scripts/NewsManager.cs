using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewsManager : MonoBehaviour
{
    public static NewsManager instance;

    [SerializeField] public List<News> newsList;
    [HideInInspector] public string todayNews;
    [SerializeField] private TextMeshProUGUI todayNewsText;
    [HideInInspector] public bool hasNews = false;
    [HideInInspector] public int lastNewsDay;
    private bool isShowingFastNews = false;

    private void Awake()
    {
        instance = this;
    }

    public void RandomizeCompanyNews()
    {
        int rand = Random.Range(0, 9); // tem 50% de chance de ter noticia no dia
        if(rand < GameManager.instance.companiesList.Count)
        {
            todayNews = GameManager.instance.companiesList[rand].randomNews();
            //GameManager.instance.companiesList[rand].ChangePriceValue();
            hasNews = true;
        }
        else if(rand == GameManager.instance.companiesList.Count)
        {
            int randNew = Random.Range(0, newsList.Count);
            todayNews = newsList[randNew].news;
            if(newsList[randNew].isGood)
            {
                if(newsList[randNew].affectCompanyA)
                {
                    GameManager.instance.companiesList[0].ChangeState(0);
                }
                if (newsList[randNew].affectCompanyB)
                {
                    GameManager.instance.companiesList[1].ChangeState(0);
                }
                if (newsList[randNew].affectCompanyC)
                {
                    GameManager.instance.companiesList[2].ChangeState(0);
                }
            }
            else
            {
                if (newsList[randNew].affectCompanyA)
                {
                    GameManager.instance.companiesList[0].ChangeState(2);
                }
                if (newsList[randNew].affectCompanyB)
                {
                    GameManager.instance.companiesList[1].ChangeState(2);
                }
                if (newsList[randNew].affectCompanyC)
                {
                    GameManager.instance.companiesList[2].ChangeState(2);
                }
            }
            hasNews = true;
        }
        else
        {
            hasNews = false;
        }
    }

    public void ShowTodayNews()
    {
        StopAllCoroutines();
        if(isShowingFastNews)
        {
            todayNewsText.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("End");
            isShowingFastNews = false;
        }
        if(hasNews)
        {
            todayNewsText.text = todayNews;
            lastNewsDay = GameManager.instance.day;
            ScreensControl.instance.ChangeNewsInfo();
            StartCoroutine(TodayNewsFade());
        }
    }

    private IEnumerator TodayNewsFade()
    {
        todayNewsText.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("Start");
        isShowingFastNews = true;
        yield return new WaitForSeconds(3);
        todayNewsText.gameObject.transform.parent.GetComponent<Animator>().SetTrigger("End");
        isShowingFastNews = false;
    }
}
