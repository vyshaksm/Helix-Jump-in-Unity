using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int totalCoin = 0;
    private int score = 0;
    private int highScore = 0;


    private void Awake()
    {
        instance = this;
    }

    public void Score(int value)
    {
        score += value;
    }


    public void CollectCoint(int count)
    {
        totalCoin += count;
        if (count > 0)
        {
            Bridge.GetInstance().CollectCoins(count);
        }
        else
        {
            Bridge.GetInstance().UpdateCoins(count);
        }
    }



    public int GetHighScore()
    {
        return highScore;
    }

    public int GetCoinCount()
    {
        return totalCoin;
    }

    public int UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            Bridge.GetInstance().thisPlayerInfo.highScore = highScore;
        }

        return highScore ;
    }

    public int GetScore()
    {
        return score;
    }
}
