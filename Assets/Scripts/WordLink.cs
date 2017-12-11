using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCenter;
using TMPro;

public class WordLink : Wireframe
{

	private string _lastSelectedWord = string.Empty;
	private AnnualWordList _annualWords = null;
	private uint[] _linkWiresNodes = null;
	public string selectedWord = "baby";

	public GameObject clickedWord;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		//string selectedWord = "baby";
		SelectWord (selectedWord); 

	}

	public void SelectWord (string word)
	{
		if (_lastSelectedWord != word) {
			if (_annualWords == null) {
				_annualWords = new AnnualWordList ();
				_annualWords.Load ();
			}
			resetColor ();
			showConnections(word);

			// Get an array of years by word
			uint[] years = _annualWords.GetYearsByTopWord (word);
			string result = string.Join (",", System.Array.ConvertAll<uint, string> (years, System.Convert.ToString));
			Debug.Log ("Years for '" + word + "' are " + result);

			_linkWiresNodes = years;
			_lastSelectedWord = word;

			if (clickedWord != null) {
				// Check attribute of the word
				clickedWord.GetComponent<TextMeshPro> ().text = word; 
				clickedWord.GetComponent<TextMeshPro> ().faceColor = getColor (word);//new Color32 (255, 0, 0, 255);
				clickedWord.GetComponent<TextMeshPro> ().fontMaterial.SetColor ("_GlowColor", UnityEngine.Random.ColorHSV (0f, 1f, 1f, 1f, 0.5f, 1f));
				//set lineColor
				lineColor = getColor(word);
			}
		}
	}

	void OnRenderObject ()
	{
		if (_lastSelectedWord.Length == 0 || _linkWiresNodes.Length == 0) {
			return;
		} else {
			foreach (uint year in _linkWiresNodes) {
				List<Vector3> wordLink = new List<Vector3> ();
				float factor = 2f;
				uint yearDiff = (year - 1965) / 10;
				float widthOverride = lineWidth + Mathf.Pow(factor, yearDiff) * 1f;
				// Debug.Log(string.Format("{0} year with width of {1}", year, widthOverride));

				int totalInterpolated = 10;
				Vector3[] arr = WireframeNodes.GetInterpolatedPoints (0, year, (uint)totalInterpolated * (6 - yearDiff));
				for (int i = 0; i < arr.Length - yearDiff * 2; i++) {
					wordLink.Add (arr [i]);
				}
				// wordLink.AddRange(WireframeNodes.GetLinesByYears (new uint[]{ 0 }));

				GLPlotLines (wordLink.ToArray (), false, widthOverride);
			}
		}
	}

	void showConnections(string word)
	{
		uint[] years = _annualWords.GetYearsByTopWord (word); 
		string key = "";
		Color wordColor = getColor (word);

		for (int i = 0; i < years.Length; i++) {
			key = years[i].ToString () + "_" + word;

			if (GameObjectClass.gameObjectDictionary.ContainsKey (key)) {
				GameObject wordObj = GameObjectClass.gameObjectDictionary [key]; 
				wordObj.GetComponent<TextMeshPro> ().faceColor = wordColor;

				//wordObj.GetComponent<TextMeshPro> ().faceColor = new Color32 (0, 255, 234, 255);
			}
		}
	}

	void resetColor()
	{
		if (_lastSelectedWord.Length != 0) {
			uint[] years = _annualWords.GetYearsByTopWord (_lastSelectedWord); 
			string key = "";
			for (int i = 0; i < years.Length; i++) {
				key = years [i].ToString () + "_" + _lastSelectedWord;

				if (GameObjectClass.gameObjectDictionary.ContainsKey (key)) {
					GameObject wordObj = GameObjectClass.gameObjectDictionary [key]; 
					wordObj.GetComponent<TextMeshPro> ().faceColor = GameObjectClass.colorDictionary[GameObjectClass.INITIAL];

				}
			}
		}
	}

	public Color32 getColor(string word)
	{
		float polarity = _annualWords.sentimentTable [word] ;
		Debug.Log ("polarity" + polarity);
		Color32 color = new Color32 ();
		if (polarity < 0.0f) {
			Debug.Log ("Negative color");
			color = GameObjectClass.colorDictionary [GameObjectClass.NEGATIVE]; //new Color32 (0, 255, 234, 255);
		} else if (polarity > 0.0f) {
			color = GameObjectClass.colorDictionary [GameObjectClass.POSITIVE]; //new Color32 (0, 255, 234, 255);
		} else {
			Debug.Log ("Nnnnnnneutral color");
			color = GameObjectClass.colorDictionary [GameObjectClass.NEUTRAL]; //new Color32 (0, 255, 234, 255);
		}
		return color;
	}


}


