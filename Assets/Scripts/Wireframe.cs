using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wireframe : MonoBehaviour
{
	public Color lineColor = new Color (0.0f, 1.0f, 1.0f, 255);
	public float lineWidth = 3;
	public Material lineMaterial;
	public bool backToOriginHorizontal = true;
	public bool backToOriginVertical = true;

	private List<uint[]> horizontalWires = new List<uint[]> ();
	private List<uint[]> verticalWires = new List<uint[]> ();

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
		if (!lineMaterial)
		{
			// Unity has a built-in shader that is useful for drawing
			// simple colored things.
			Shader shader = Shader.Find("Hidden/Internal-Colored");
			lineMaterial = new Material(shader);
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			// Turn on alpha blending
			lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			// Turn backface culling off
			lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			// Turn off depth writes
			lineMaterial.SetInt("_ZWrite", 0);
		}
	}

	void OnRenderObject () {
		if (horizontalWires.Count == 0 || verticalWires.Count == 0) {
			PopulateYears ();	
		}

		foreach (uint[] years in horizontalWires) {
			Vector3[] lines = WireframeNodes.GetInterpolatedLinesByYears (years, backToOriginHorizontal);
			GLPlotLines (lines, backToOriginHorizontal);
		}

		foreach (uint[] years in verticalWires) {
			Vector3[] lines = WireframeNodes.GetInterpolatedLinesByYears (years, backToOriginVertical);
			GLPlotLines (lines, backToOriginVertical);
		}
	} 

	protected void GLPlotLines(Vector3[] lines, bool backToOrigin, float widthOverride = 0f) {

		float actualWidth = lineWidth;
		if (widthOverride != 0f) {
			actualWidth = widthOverride;
		}

		if (lines == null) {
			Debug.Log("No lines");
		} 
		else
		{
			lineMaterial.SetPass(0);
			GL.Color(lineColor);

			if (actualWidth == 1) {
				GL.Begin(GL.LINES);
				GL.Color(lineColor);
				for(int i = 0; i + 1 < lines.Length; i++)
				{
					Vector3 vec1 = to_world(lines[i]);
					Vector3 vec2 = to_world(lines[i + 1]);
					GL.Vertex (vec1);
					GL.Vertex (vec2);
				}

				if (backToOrigin) {
					// connect back to origin
					GL.Vertex (to_world (lines [lines.Length - 1]));
					GL.Vertex (to_world (lines [0]));
				}
			} else {
				GL.Begin(GL.QUADS);
				GL.Color(lineColor);
				for(int i = 0; i + 1 < lines.Length; i += 1) {
					Vector3 vec1 = to_world(lines[i]);
					Vector3 vec2 = to_world(lines[i + 1]);
					DrawQuad(vec1, vec2, actualWidth);
				}
				if (backToOrigin) {
					// connect back to origin
					DrawQuad (to_world (lines [lines.Length - 1]), to_world (lines [0]), actualWidth);
				}
			}
			GL.End();
		}
	}
	// to simulate thickness, draw line as a quad scaled along the camera's vertical axis.
	private void DrawQuad(Vector3 p1, Vector3 p2, float actualWidth){
		Vector3 edge1 = Camera.main.transform.position - (p2 + p1)/2.0f;    //vector from line center to camera
		Vector3 edge2 = p2 - p1;    //vector from point to point
		Vector3 perpendicular = Vector3.Cross(edge1,edge2).normalized * actualWidth / 100f;

		GL.Vertex(p1 - perpendicular);
		GL.Vertex(p1 + perpendicular);
		GL.Vertex(p2 + perpendicular);
		GL.Vertex(p2 - perpendicular);
	}

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
}


