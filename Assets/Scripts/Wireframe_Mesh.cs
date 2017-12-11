using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wireframe_Mesh : MonoBehaviour {
	public float lineWidth = 3;
	public bool backToOriginHorizontal = true;
	public bool backToOriginVertical = true;

	private List<uint[]> horizontalWires = new List<uint[]> ();
	private List<uint[]> verticalWires = new List<uint[]> ();

	private MeshFilter meshFilter = null;
	private bool meshGenerated = false;
	private Mesh meshGrid = null;

	private MeshBuilder meshBuilder;

	private Vector3 to_world(Vector3 vec)
	{
		return gameObject.transform.TransformPoint(vec);
	}

	public static IEnumerable<uint> RangePython(uint start, uint stop, uint step = 1)
	{
		if (step == 0)
			throw new ArgumentException("Parameter step cannot equal zero.");

		if (start < stop && step > 0)
		{
			for (var i = start; i < stop; i += step)
			{
				yield return i;
			}
		}
		else if (start > stop && step < 0)
		{
			for (var i = start; i > stop; i += step)
			{
				yield return i;
			}
		}
	}

	public static IEnumerable<uint> RangePython(uint stop)
	{
		return RangePython(0, stop);
	}

	void PopulateYears()
	{
		uint startYear = 1965;
		uint stopYear = 2015;
		uint steps = 10;

		for (uint year = startYear; year < stopYear; year += steps) {
			horizontalWires.Add(RangePython(year, year + steps).ToArray());
		}

		for (uint year = startYear; year < startYear + steps; year++) {
			verticalWires.Add(RangePython(year, stopYear, steps).ToArray());
		}
	}

	void Start() {
		this.meshGenerated = false;
		this.meshFilter = this.GetComponent<MeshFilter> ();
		this.meshBuilder = new MeshBuilder ();

		PopulateYears ();
	}
		
	void Update() {
		if (!meshGenerated) {
			foreach (uint[] years in horizontalWires) {
				Vector3[] lines = WireframeNodes.GetInterpolatedLinesByYears (years, backToOriginHorizontal);
				GenerateMesh (lines, backToOriginHorizontal);
			}

			foreach (uint[] years in verticalWires) {
				Vector3[] lines = WireframeNodes.GetInterpolatedLinesByYears (years, backToOriginHorizontal);
				GenerateMesh (lines, backToOriginVertical);
			}

			meshGenerated = true;
			meshGrid = meshBuilder.CreateMesh ();
			meshFilter.mesh = meshGrid;
		}
	}

	private void GenerateMesh(Vector3[] lines, bool backToOrigin) {
		if (lines == null) {
			Debug.Log("No lines");
		} 
		else
		{
			for(int i = 0; i + 1 < lines.Length; i += 1) {
				Vector3 vec1 = to_world(lines[i]);
				Vector3 vec2 = to_world(lines[i + 1]);
				AddMesh(vec1, vec2);
			}
			if (backToOrigin) {
				// connect back to origin
				AddMesh (to_world (lines [lines.Length - 1]), to_world (lines [0]));
			}
		}
	}
	// to simulate thickness, draw line as a quad scaled along the camera's vertical axis.
	private void AddMesh(Vector3 p1, Vector3 p2 ){
		Vector3 edge1 = Camera.main.transform.position - (p2 + p1) / 2.0f;    //vector from line center to camera
		Vector3 edge2 = p2 - p1;    //vector from point to point
		Vector3 perpendicular = Vector3.Cross (edge1, edge2).normalized * lineWidth / 100f;

		Vector3 pos1 = p1 + perpendicular;
		Vector3 pos2 = p1 - perpendicular;
		Vector3 pos3 = p2 - perpendicular;
		Vector3 pos4 = p1 + perpendicular;

		meshBuilder.Vertices.Add(pos1);
		meshBuilder.Vertices.Add(pos2);
		meshBuilder.Vertices.Add(pos3);
		meshBuilder.Vertices.Add(pos4);
//		meshBuilder.UVs.Add (new Vector2 (pos1.x, pos1.z));
//		meshBuilder.UVs.Add (new Vector2 (pos2.x, pos2.z));
//		meshBuilder.UVs.Add (new Vector2 (pos3.x, pos3.z));
//		meshBuilder.UVs.Add (new Vector2 (pos4.x, pos4.z));
//
		int vertsPerRow = 2;
		int baseIndex = meshBuilder.Vertices.Count - 1;

		int index0 = baseIndex;
		int index1 = baseIndex - 1;
		int index2 = baseIndex - vertsPerRow;
		int index3 = baseIndex - vertsPerRow - 1;

		meshBuilder.AddTriangle(index0, index2, index1);
		meshBuilder.AddTriangle(index2, index3, index1);
	}
}
