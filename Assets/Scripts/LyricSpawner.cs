using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DataCenter;


public class LyricSpawner : MonoBehaviour {
	public static int NUM_WORDS = 5;
    public GameObject lyricObject;
    public List<GameObject> lyrics;
    Vector3 pointOfRotation;

    Quaternion oldRotation1;
    Quaternion oldRotation2;
    Quaternion oldRotation3;

	Vector3[] rotateDirections = new Vector3[]{Vector3.up, Vector3.left, Vector3.forward, new Vector3(1, -1, 0), new Vector3(0, -1, 1)};
	int[] rotateSpeeds = new int[] {100, 80, 60, 40, 20};
    public bool stopRotation = false;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (!stopRotation)
        {
            for (int i = 0; i < lyrics.Count; i++)
            {
                //oldRotation1 = lyrics[0].GetComponent<RectTransform>().localRotation;
                lyrics[i].transform.RotateAround(pointOfRotation, rotateDirections[i], Time.deltaTime * rotateSpeeds[i]);
                //lyrics[0].GetComponent<RectTransform>().localRotation = oldRotation1;

                //	        //oldRotation2 = lyrics[1].GetComponent<RectTransform>().localRotation;
                //	        lyrics[1].transform.RotateAround(pointOfRotation, Vector3.left, Time.deltaTime * 50);
                //	       // lyrics[1].GetComponent<RectTransform>().localRotation = oldRotation2;
                //
                //	        //oldRotation3 = lyrics[2].GetComponent<RectTransform>().localRotation;
                //	        lyrics[2].transform.RotateAround(pointOfRotation, Vector3.forward, Time.deltaTime * 25);
                //	       // lyrics[2].GetComponent<RectTransform>().localRotation = oldRotation3;
            }
        }
		
    }

   public void GenerateWords(Transform pos)
    {
        //Debug.Log("I will spawn object at: " + pos.position);
        GameObject word1 = Instantiate(lyricObject);
        word1.transform.position = pos.position;
        word1.transform.parent = pos;
        word1.transform.localPosition += new Vector3(-7.5f, 2.92f, 0.28f);
        pointOfRotation = word1.transform.position;
        word1.gameObject.SetActive(false);

        //word1.transform.position += new Vector3(-7.5f, 2.92f, 0.28f);
        word1.gameObject.name = "Main";
        for (int i = 0; i < NUM_WORDS; i++)
        {
            GameObject word = Instantiate(lyricObject);
            word.transform.position = pos.position + Random.insideUnitSphere * 0.05f;           
            word.transform.parent = pos;

            lyrics.Add(word);
        }

    }

    public void GenerateWords(Transform pos, string word1, string word2, string word3)
	{
		//Debug.Log("I will spawn object at: " + pos.position);
		GameObject word1Dummy = Instantiate (lyricObject);
		word1Dummy.transform.position = pos.position;
		word1Dummy.transform.parent = pos;
		word1Dummy.transform.localPosition += new Vector3 (-7.5f, 2.92f, 0.28f);
		pointOfRotation = word1Dummy.transform.position;
		word1Dummy.gameObject.SetActive (false);

		//word1.transform.position += new Vector3(-7.5f, 2.92f, 0.28f);
		word1Dummy.gameObject.name = "Main";
		for (int i = 0; i < NUM_WORDS; i++) {
			GameObject word = Instantiate (lyricObject);
			word.transform.position = pos.position + Random.insideUnitSphere * 0.05f;
			word.transform.parent = pos;
			if (i == 0) {
				word.gameObject.name = word1;
				word.GetComponent<TextMeshPro> ().text = word1;
			} else if (i == 1) {
				word.gameObject.name = word2;
				word.GetComponent<TextMeshPro> ().text = word2;
			} else {
				word.gameObject.name = word3;
				word.GetComponent<TextMeshPro> ().text = word3;
			}

			// change scale
			float scaleFactor = 0.15f * i;
			//Debug.Log ("Last scale " + word.transform.localScale);
			word.transform.localScale = word.transform.localScale - new Vector3 (scaleFactor, scaleFactor, scaleFactor);
			//Debug.Log ("New scale " + word.transform.localScale);

			lyrics.Add (word);
		}
	}

	public void GenerateWords(Transform pos, TopWord[] words)
	{
		//Debug.Log("I will spawn object at: " + pos.position);
		GameObject word1Dummy = Instantiate(lyricObject);
		word1Dummy.transform.position = pos.position;
		word1Dummy.transform.parent = pos;
		word1Dummy.transform.localPosition += new Vector3(-7.5f, 2.92f, 0.28f);
		pointOfRotation = word1Dummy.transform.position;
		word1Dummy.gameObject.SetActive(false);

		//word1.transform.position += new Vector3(-7.5f, 2.92f, 0.28f);
		word1Dummy.gameObject.name = "Main";
		for (int i = 0; i < NUM_WORDS; i++)
		{
			GameObject word = Instantiate(lyricObject);
			word.transform.position = pos.position + Random.insideUnitSphere * 0.05f;
			word.transform.parent = pos;
		
			word.gameObject.name = words[i].Word;
			word.GetComponent<TextMeshPro>().text = words[i].Word;

			// change scale
			float scaleFactor = 0.2f * Mathf.Pow(0.7f, (float)i);
			//Debug.Log ("Last scale " + word.transform.localScale);
			word.transform.localScale = word.transform.localScale - new Vector3(scaleFactor, scaleFactor, scaleFactor);
			//Debug.Log ("New scale " + word.transform.localScale);

			//change color
			word.GetComponent<TextMeshPro>().faceColor = GameObjectClass.colorDictionary[GameObjectClass.INITIAL]; //new Color32(139, 147, 231, 255);

			lyrics.Add(word);

			//Generate gameObject dictionary
			string key = pos.gameObject.name + "_" + words[i].Word;
			GameObjectClass.gameObjectDictionary.Add (key, word);
		}
    }
}
