using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectClass {
	public static Dictionary<string,GameObject> gameObjectDictionary = new Dictionary<string, GameObject> ();

	public static Dictionary<string, Color32> colorDictionary = new Dictionary<string, Color32> ();
	public static string NEGATIVE = "negative";
	public static string POSITIVE = "positive";
	public static string NEUTRAL = "neutral";
	public static string INITIAL = "initial";

	static GameObjectClass()
	{
		colorDictionary.Add(NEGATIVE, new Color32(0, 255, 234, 255));
		colorDictionary.Add(POSITIVE, new Color32(255, 114, 118, 255));
		colorDictionary.Add(NEUTRAL, new Color32(139, 147, 231, 255));
		colorDictionary.Add(INITIAL, new Color32(255, 255, 255, 255));
	}

}