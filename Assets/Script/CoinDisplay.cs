using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CoinDisplay : MonoBehaviour
{
    private CoinManager coinManager;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI PointText;

    void Start()
    {
        coinManager=FindObjectOfType<CoinManager>();
        UpdateCoinDisplay();
    }

   void Update(){
    UpdateCoinDisplay();

    }

    void UpdateCoinDisplay()
    {
        if (coinManager != null && PointText != null)
        {
            PointText.text =  coinManager.GetCurrentPoint().ToString();
        }
         if (coinManager != null && coinText != null)
        {
            coinText.text =  coinManager.GetRealCoin().ToString();
        }
    }
}
