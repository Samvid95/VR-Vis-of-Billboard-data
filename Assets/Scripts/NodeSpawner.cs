using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCenter;
using System;
/*
 * This class will randomly create nodes in the virtual world to interact with!! 
 * With given parameter range it will put 100 nodes inside the environment.
 */
public class NodeSpawner : MonoBehaviour {
    public GameObject node;
    [SerializeField]
    private List<GameObject> Nodes;
    private LineRenderer link;
    private List<int> nodeLinkList;

	// Use this for initialization
	void Start () {
        nodeLinkList = new List<int>();
        int i = 0;
        while(i < 100)
        {
			float x = UnityEngine.Random.Range(-35, 35);
			float z = UnityEngine.Random.Range(-35, 35);
			float y = UnityEngine.Random.Range(5,15);

            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, y, z), 5);
            if (hitColliders.Length == 0)
            {
                GameObject networkNode = Instantiate(node, new Vector3(x, y, z), Quaternion.identity);
				networkNode.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                networkNode.GetComponent<PlanetSpawner>().SpawnPlanets(networkNode.transform);
                Nodes.Add(networkNode);
                i++;
            }
        }

        link = Nodes[20].GetComponent<LineRenderer>();
        link.SetPosition(0, Nodes[20].transform.position);
        link.SetPosition(1, Nodes[30].transform.position);
        for (int j =0; j < 100; j++)
        {
			int rand = UnityEngine.Random.Range(0, 99);
            nodeLinkList.Add(rand);
        }
       for(int j=0; j < 100; j+=2)
        {
            link = Nodes[nodeLinkList[j]].GetComponent<LineRenderer>();
            link.SetPosition(0, Nodes[nodeLinkList[j]].transform.position);
            link.SetPosition(1, Nodes[nodeLinkList[j+1]].transform.position);
        }

		//import billboard data
		//Get all top 3 words in all songs
		DataSource songList = new DataSource();
		songList.Load();
		List<SongInfo> allSongs = songList.GetAllSongs();
		// example reading data
		UnityEngine.Debug.Log(allSongs[1].ToString());

		//Get annual top 3 words 
		AnnualWordList annualWords = new AnnualWordList();
		annualWords.Load();
		List<TopWord> annualTopWords = annualWords.GetTopWordsByYear(1983, 3); //Year, top#3
		UnityEngine.Debug.Log(annualTopWords[0].Word);

		//Get top words over the years
		List<TopWord> topWords = annualWords.GetTopWords(3);
		String toPrint = String.Format ("Word:{0}, Polarity:{1}", topWords [0].Word, topWords [0].Polarity);
		UnityEngine.Debug.Log(toPrint);

		// Get an array of years by word
		uint[] years = annualWords.GetYearsByTopWord("right");
		string result = string.Join(",", System.Array.ConvertAll<uint, string>(years, System.Convert.ToString));
		Debug.Log("Years for 'right' are " + result);

		years = annualWords.GetYearsByTopWord("see");
		result = string.Join(",", System.Array.ConvertAll<uint, string>(years, System.Convert.ToString));
		Debug.Log("Years for 'see' are " + result);
	}
}
