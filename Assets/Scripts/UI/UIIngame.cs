using UnityEngine;
using TMPro;

public class UIIngame : MonoBehaviour
{

    public TextMeshProUGUI totalCreatureText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.CreaturesValueUpdated += OnCreatureValueUpdated;
        SetCreatureValueText(0);
    }

    private void OnDestroy()
    {
        EventManager.CreaturesValueUpdated -= OnCreatureValueUpdated;
    }


    void OnCreatureValueUpdated(int value)
    {
        SetCreatureValueText(value);
    }


    void SetCreatureValueText(int value)
    {
        totalCreatureText.text = value.ToString();
    }

}
