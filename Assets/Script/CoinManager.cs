using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CoinManager : MonoBehaviour
{
    public int startingCoins;
    private int currentPoint;

    private const string PlayerSolarPoint = "PlayerSolarPoint";

    private int pointsToRealCoin = 1000;
    private int lifeCost = 100;
    private float pointValue = 0.10f;
    private float realCoinValue = 1.0f;
    private float RealCoin;

    //...............................API Intergerate...........................................
    public string saassyApiUrl; // Assign this in the Unity Inspector
    private const string FilePath = "Assets/API/casino game.postman_collection.json"; // Update this with your file path
   
    IEnumerator PointsToCoins(int points)
    {
        string url = $"{saassyApiUrl}/game/points-to-coins";
        string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI5YTJmMTliNy0zNGVlLTQ5N2UtYmIyZC00YzNkN2MzOWMzMzgiLCJqdGkiOiIxMDNlNzBjODNlN2E2YjYzZTA2NjhiOTJlZGI2NWY2Y2YzNTNjMzg0YmVjMzE1MTkwODNjOGVjNTAxOWY0ZmY4ODM4ZGNjNTYzOGIxNDdkOSIsImlhdCI6MTcwMTgzMzk2MS4yMTEwMzYsIm5iZiI6MTcwMTgzMzk2MS4yMTEwMzksImV4cCI6MTczMzQ1NjM2MS4xNjQ4OSwic3ViIjoiMTQ0Iiwic2NvcGVzIjpbXX0.PfbgfvhqB1tCDusqKhJ9zxiDcBViF5wyM50tjSD2MxvgUlSd_G25u6iBhUyaZF_rwRL5_7uhBxMv7s6Ozn3g7vrw1jqbaNnSYJd3x9WSTWDiy_2wpf1vQX2eg5BuYv9LLqJ_qW1_p1Xj8HJX3iTmnl4xv2lbUOYRq_WxEns5GYuadw_sO6Nu3OJ2_VnC1qSiSCEuO9m5n6mrRDE111E__b_41fZSzTFYPwjjU4KRpJqfWiDVlHm9YoMBKOvapweYuiWZ8SDG6LQoR6Z5x_vRoqDjZuNregO8JoIJHK6pR3-d2J8-5LiwklBmO20o6Z4rGgWwdsl4rSG44g4ZKUbz9AXedmC-fgse5SAbDVpEm8VtEp4F9I2qvnbg8WQEuH5rAWYeF38NFjKItEP6t0AbJ7BHzeVnoGY-n4aqh7eVWPBS2ibhbpb4GXpkYq9YZleg2wCp7Ef4m_dcGR2fGTuuh9I2EIdm9OtDsa1MbhmFdrFAmLeIjam9HbQs8eNphGlu_AL3O7EgkaKBuamHEHPDnjnQvZUYZs2tYd98HamMAas7sGOo1qIiDjDloKle_0c7ogl_7hIrKtYy5X-pBT8HAXEgAyFqWRKzuwIc6JIvEBQDyUVxkXOkSDRlUdAtslrIXiv3OhbKjCZKEqabB54WDZaPX8RWVOngk9A7baWYCFw"; // Replace with your actual bearer token

        WWWForm form = new WWWForm();
        form.AddField("points", points.ToString());

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.SetRequestHeader("Authorization", $"Bearer {token}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Handle successful response
            Debug.Log("Points converted to coins successfully");
            // Example: Update CoinManager script with the received data
            // CoinManager.Instance.UpdateCoins(); // Replace UpdateCoins with your method to update coin count
        }
        else
        {
            // Handle error
            Debug.Log("Error: " + request.error);
        }

       
    }

  


   
    //...............................API Intergerate...........................................



    IEnumerator UpdateRealCoinCoroutine(float updateInterval)
    {
        while (true)
        {
            UpdateRealCoinValue(); // Update RealCoin value based on current points
            yield return new WaitForSeconds(updateInterval); // Wait for the specified interval
        }
    }

    void Start()
    {
        LoadCoins();
        float updateInterval = 1.0f; // Update interval in seconds (change this as needed)
        StartCoroutine(UpdateRealCoinCoroutine(updateInterval));
        
    }

    void LoadCoins()
    {
        if (PlayerPrefs.HasKey(PlayerSolarPoint))
        {
            currentPoint = PlayerPrefs.GetInt(PlayerSolarPoint);

        }
        else
        {
            currentPoint = startingCoins;
            SaveCoins();
        }
    }

    void SaveCoins()
    {
        PlayerPrefs.SetInt(PlayerSolarPoint, currentPoint);
        PlayerPrefs.Save();
    }

    public float GetRealCoin() // Changed return type to float
    {
        return RealCoin; // Returning RealCoin as a float
    }

    public int GetCurrentPoint()
    {
        return currentPoint;
    }

    public void SpendPoint(int amount)
    {
        if (currentPoint >= amount)
        {
            currentPoint -= amount;
            SaveCoins();
             StartCoroutine(PointsToCoins(currentPoint));

        }
        else
        {
            Debug.LogWarning("Not enough coins to spend!");
        }
    }

    public void AddPoint(int amount)
    {
        currentPoint += amount;
        SaveCoins();

        StartCoroutine(PointsToCoins(currentPoint));


    }

    // Method to convert collected points to real coins
    public float ConvertPointsToRealCoins(int points)
    {
        return (float)points / pointsToRealCoin * realCoinValue;
    }

    // Method to calculate the cost of a player's life in real coins
    public float CalculateLifeCost()
    {
        return lifeCost * pointValue;
    }

    // Example usage to update RealCoin based on current points
    void UpdateRealCoinValue()
    {
        RealCoin = ConvertPointsToRealCoins(currentPoint);


    }
}
