using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAlphaSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPointT;
    [SerializeField] private GameObject textPrefab;
    
    [Space]
    
    [SerializeField] private float speed;
    [SerializeField] private float alphaSpeed;

    private List<Transform> spawnedTextsT = new List<Transform>();
    private List<TMP_Text> spawnedTexts = new List<TMP_Text>();
    
    private void Update()
    {
        for (int i = 0; i < spawnedTextsT.Count; i++)
        {
            var itemT = spawnedTextsT[i];
            var item = spawnedTexts[i];

            var timeStepT = speed * Time.deltaTime;
            var timeStep = alphaSpeed * Time.deltaTime;

            itemT.localPosition += spawnPointT.up * timeStepT;

            var textColor = item.color;
            textColor.a = Mathf.MoveTowards(textColor.a,0,timeStep);
            item.color = textColor;

            if (item.color.a <= 0.01f)
            {
                spawnedTextsT.Remove(itemT);
                spawnedTexts.Remove(item);
                
                Destroy(itemT.gameObject);
            }
        }
    }

    public void SpawnText(string text)
    {
        var textTMP = 
            Instantiate(textPrefab,spawnPointT).GetComponent<TMP_Text>();

        textTMP.text = text;
        
        spawnedTextsT.Add(textTMP.transform);
        spawnedTexts.Add(textTMP);
    }
}
