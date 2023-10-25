using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public GameObject timeGO;

    public void Setup (float time)
    {
        gameObject.SetActive(true);
        timeGO.GetComponent<TMPro.TextMeshProUGUI>().text = "Zeit: " + time.ToString() + " Sekunden";
        //timeText.text = time.ToString() + " Sekunden";
    }
}
