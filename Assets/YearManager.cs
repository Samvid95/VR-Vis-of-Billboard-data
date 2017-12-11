using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCenter;

public class YearManager : MonoBehaviour {
    public List<GameObject> allYears;
	// Use this for initialization
	void Start () {
		for(int i = 1965; i < 2015; i++)
        {
            GameObject year = GameObject.Find(i.ToString());
            allYears.Add(year);
        }
        AnnualWordList annualWords = new AnnualWordList();
        annualWords.Load();

        uint[] commanYears;
        commanYears= annualWords.GetYearsByTopWord("girl");
        for(int i=0; i < commanYears.Length; i++)
        {
            Debug.Log(commanYears[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
