using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DataCenter;
using UnityEditor;

public class SongsSpawner_Sphere : MonoBehaviour
{

	public static int numObjects = 10;
	public static float AngleOffset = -8f;

	float angleMargin = 35.0f;
	// 25 degrees open on the top and bottom
	int totalSpawners = 5;

	public GameObject prefab;
	public Vector3 centerPosition;
	public float radius;
	public float yearStart;
	public int latitudeIndex;

	void Start ()
	{
		AnnualWordList annualWords = new AnnualWordList ();
		annualWords.Load ();
		Vector3 center = centerPosition;

		float latitude = (180.0f - angleMargin * 2) / (totalSpawners - 1) * (latitudeIndex - 1) + angleMargin - 90.0f;

		WireframeNodes.RegisterCenterSphere (centerPosition, radius);
		//WireframeNodes.NorthPole = new Vector3 (center.x, center.y + 2 * radius, center.z);
		WireframeNodes.RegisterNorthPole(RandomCircleOnSphere(center, radius, 0f, 90f));

		for (int i = 0; i < numObjects; i++, yearStart++) {
			float longitude = 360.0f / numObjects * i;

			Vector3 pos = RandomCircleOnSphere (center, radius, longitude, latitude);

			// Debug.Log (string.Format ("Generating pos {0}, {1} for {2} at year {3}", longitude, latitude, latitudeIndex, yearStart));

			GameObject yearName = Instantiate (prefab, pos, Quaternion.LookRotation (pos - center));
			yearName.transform.parent = this.transform;

			// saving the position to Wireframe Nodes
			Vector3 offsetPos = RandomCircleOnSphere (center, radius, longitude + AngleOffset, latitude);
			WireframeNodes.RegisterNode ((uint)yearStart, offsetPos, longitude + AngleOffset, latitude);

			yearName.GetComponent<TextMeshPro> ().text = yearStart.ToString ();
			// yearName.GetComponent<TextMeshPro> ().faceColor = new Color32 (255, 0, 0, 255);
			yearName.GetComponent<TextMeshPro> ().fontMaterial.SetColor ("_GlowColor", UnityEngine.Random.ColorHSV (0f, 1f, 1f, 1f, 0.5f, 1f));
			yearName.gameObject.name = yearStart.ToString ();
			//yearName.GetComponent<LyricSpawner> ().GenerateWords (yearName.transform);

			List<TopWord> annualTopWords = annualWords.GetTopWordsByYear((uint)yearStart, LyricSpawner.NUM_WORDS); //Year, top#3
			// UnityEngine.Debug.Log(yearStart.ToString() + " : " + annualTopWords[0].Word + "and" + annualTopWords[0].Polarity + " and " + annualTopWords[1].Word + " and " + annualTopWords[2].Word);
			yearName.GetComponent<LyricSpawner>().GenerateWords(yearName.transform, annualTopWords.ToArray());
		}
	}

	Vector3 RandomCircle (Vector3 center, float radius, int a)
	{

		float ang = a;
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin (ang * Mathf.Deg2Rad);
		pos.y = center.y;
		pos.z = center.z + radius * Mathf.Cos (ang * Mathf.Deg2Rad);

		return pos;
	}

	Vector3 RandomCircleOnSphere (Vector3 center, float radius, float longitude, float latitude)
	{
		Vector3 pos;

		float radiusOnSphere = radius * Mathf.Cos (latitude * Mathf.Deg2Rad);
		pos.x = center.x + radiusOnSphere * Mathf.Sin (longitude * Mathf.Deg2Rad);
		pos.z = center.y + radiusOnSphere * Mathf.Cos (longitude * Mathf.Deg2Rad);
		pos.y = center.z + radius * Mathf.Sin (latitude * Mathf.Deg2Rad) + radius;

		return pos;
	}
}


