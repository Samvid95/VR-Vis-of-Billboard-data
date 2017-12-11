using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DataCenter;
using UnityEditor;

public class SongsSpawner : MonoBehaviour {

    int numObjects = 10;
    public GameObject prefab;
    public Vector3 centerPosition;
    public float radius;
    public float yearStart;
    void Start()
    {
        AnnualWordList annualWords = new AnnualWordList();
        annualWords.Load();
        Vector3 center = centerPosition;
        for (int i = 0; i < numObjects; i++,yearStart++)
        {
            int a = 360 / numObjects * i;
            Vector3 pos = RandomCircle(center, radius, a);
            GameObject yearName = Instantiate(prefab, pos, Quaternion.LookRotation(pos - center));
            yearName.transform.parent = this.transform;
            yearName.GetComponent<TextMeshPro>().text = yearStart.ToString();
			yearName.GetComponent<TextMeshPro>().faceColor = new Color32(255, 0, 0, 255);
            yearName.GetComponent<TextMeshPro>().fontMaterial.SetColor("_GlowColor", UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
            yearName.gameObject.name = yearStart.ToString();
			//yearName.GetComponent<LyricSpawner> ().GenerateWords (yearName.transform);

			List<TopWord> annualTopWords = annualWords.GetTopWordsByYear((uint)yearStart, LyricSpawner.NUM_WORDS); //Year, top#3
			UnityEngine.Debug.Log(yearStart.ToString() + " : " + annualTopWords[0].Word + "and" + annualTopWords[0].Polarity + " and " + annualTopWords[1].Word + " and " + annualTopWords[2].Word);
            yearName.GetComponent<LyricSpawner>().GenerateWords(yearName.transform, annualTopWords[0].Word, annualTopWords[1].Word, annualTopWords[2].Word);
        }

		// now it includes only top three - for debugging only
		uint[] years = annualWords.GetYearsByTopWord ("love");
		string result = string.Join(",", System.Array.ConvertAll<uint, string>(years, System.Convert.ToString));
		Debug.Log("Years for 'love' are " + result);
        //Debug.Log("Coming out here!!");
    }
    Vector3 RandomCircle(Vector3 center, float radius, int a)
    {

        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}


