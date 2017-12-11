using System;
using System.Collections.Generic;
using UnityEngine;

public class WireframeNodes
{
	private static Dictionary<uint, Vector3> _yearNodes = new Dictionary<uint, Vector3>();

	// saving x as longituge, y as latitude
	private static Dictionary<uint, Vector2> _yearAngles = new Dictionary<uint, Vector2>();

	private static Vector3 _center;
	private static float _radius;

	public static void Clear()
	{
		_yearNodes.Clear ();
	}

	public static void RegisterNode(uint year, Vector3 pos, float longtitude = 0f, float latitude = 0f)
	{
		_yearNodes.Add(year, pos);
		_yearAngles.Add(year, new Vector2(longtitude, latitude)); 	// saving x as longituge, y as latitude
		//Debug.Log(string.Format("Year {0} with pos {1} are added to dictionary, length is {2}", year, pos, _yearNodes.Count));
	}

	public static void RegisterNorthPole(Vector3 pos, float longtitude = 0f, float latitude = 90f)
	{
		if (!_yearNodes.ContainsKey (0)) {
			_center = pos;
			_yearNodes.Add (0, pos);
			_yearAngles.Add (0, new Vector2 (longtitude, latitude));
		}
	}

	public static void RegisterCenterSphere(Vector3 center, float radius)
	{
		_center = center;
		_radius = radius;
	}

	public static Vector3[] GetLinesByYears(uint[] years)
	{
		List<Vector3> ret = new List<Vector3> ();

		foreach (uint year in years) {
			if (_yearNodes.ContainsKey (year)) {
				ret.Add (_yearNodes [year]);
			} else {
				Debug.Log (string.Format ("Given year {0} is not in the dictionary.", year));
			}
		}

		if (ret.Count == 0) {
			Debug.Log (string.Format ("Given years {0} - {1} doesn't have position info.", years[0], years[years.Length - 1]));
		}

		return ret.ToArray();
	}

	public static Vector3[] GetInterpolatedLinesByYears(uint[] years, bool backToOrigin)
	{
		List<Vector3> ret = new List<Vector3> ();

		for (int i = 0; i < years.Length; i++) {
			uint year = years [i];
			if (_yearNodes.ContainsKey (year)) {

				// interpolating
				if (ret.Count > 0) {
					Vector3[] addedPoints = GetInterpolatedPoints(year, years [i - 1]);
					ret.AddRange (addedPoints);
				} 

				ret.Add (_yearNodes [year]);
			} else {
				Debug.LogError (string.Format ("Given year {0} is not in the dictionary.", year));	// not supposed to happen
			}
		}

		if (backToOrigin) {
			Vector3[] backToOriginPoints = GetInterpolatedPoints (years [0], years [years.Length - 1]);
			ret.AddRange (backToOriginPoints);
		}

		if (ret.Count == 0) {
			Debug.LogError (string.Format ("Given years {0} - {1} doesn't have position info.", years[0], years[years.Length - 1]));
		}

		return ret.ToArray();
	}

	public static Vector3[] GetInterpolatedPoints(uint right, uint left, uint interpolatedPoints = 5)
	{
		// Debug.Log (string.Format ("Getting interpolated points between {0} and {1}", right, left));

		if (!_yearAngles.ContainsKey (left) || !_yearAngles.ContainsKey (right)) {
			// Debug.Log (string.Format("_yearAngles doesn't have the required year {0}, {1}", left, right));
			return new Vector3[] {};
		}

		List<Vector3> retList = new List<Vector3> ();
		float leftLongitude = _yearAngles [left].x;	// saving x as longituge, y as latitude
		float rightLongitude = _yearAngles [right].x;
		float leftLatitude = _yearAngles [left].y;	
		float rightLatitude = _yearAngles [right].y;
		float longitudeStep = 0.0f;
		float latitudeStep = 0.0f;

		// for years on the same latitude
		if (leftLatitude == rightLatitude) {
			if (leftLongitude > rightLongitude) {
				longitudeStep = (360f - rightLongitude - leftLongitude) / (interpolatedPoints + 1);
			} else {
				longitudeStep = (rightLongitude - leftLongitude) / (interpolatedPoints + 1);
			}
		} else {
			// for years on longitude lines
			if (leftLatitude > rightLatitude) {
				latitudeStep = (360f - rightLatitude - leftLatitude) / (interpolatedPoints + 1);
			} else {
				latitudeStep = (rightLatitude - leftLatitude) / (interpolatedPoints + 1);
			}
		}

		for (int i = 0; i < interpolatedPoints; i++) {
			Vector3 pos;

			float longitude = leftLongitude + i * longitudeStep;
			float latitude = leftLatitude + i * latitudeStep;
			float radiusOnSphere = _radius * Mathf.Cos (latitude * Mathf.Deg2Rad);
			pos.x = _center.x + radiusOnSphere * Mathf.Sin (longitude * Mathf.Deg2Rad);
			pos.z = _center.y + radiusOnSphere * Mathf.Cos (longitude * Mathf.Deg2Rad);
			pos.y = _center.z + _radius * Mathf.Sin (latitude * Mathf.Deg2Rad) + _radius;

			// Debug.Log (string.Format("point added between year {0}@{1} and {2}@{3} on angle {4}", left, leftLongitude, right, rightLongitude, longitude));
			retList.Add (pos);
		}

		return retList.ToArray();
	}
}

