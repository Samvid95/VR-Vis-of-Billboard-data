using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour {
    public GameObject planet;
    public List<GameObject> planetObjects;
    private Transform coreNode;

	public void SpawnPlanets(Transform mainObject)
    {
        coreNode = mainObject;
        for(int i =0; i < 3; i++)
        {
            if(i == 0)
            {
                Vector3 positionAroundNode = new Vector3(0.5f, 0, 0) + transform.position;
                GameObject smallNode = Instantiate(planet, positionAroundNode, Quaternion.identity);
                smallNode.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 0f, 0f, 0f, 0f, 1f);
                planetObjects.Add(smallNode);
            }
            else if (i == 1)
            {
                Vector3 positionAroundNode = new Vector3(0.5f, 0, 0) + transform.position;
                GameObject smallNode = Instantiate(planet, positionAroundNode, Quaternion.identity);
                smallNode.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 0f, 0f, 0f, 0f, 1f);
                planetObjects.Add(smallNode);
            }
            else
            {
                Vector3 positionAroundNode = new Vector3(0.5f, 0, 0) + transform.position;
                GameObject smallNode = Instantiate(planet, positionAroundNode, Quaternion.identity);
                smallNode.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 0f, 0f, 0f, 0f, 1f);
                planetObjects.Add(smallNode);
            }
        }
    }
    private void Update()
    {

        planetObjects[0].transform.RotateAround(coreNode.position, new Vector3(90, 0, 0), 50 * Time.deltaTime);
        planetObjects[1].transform.RotateAround(coreNode.position, new Vector3(0, 90, 0), 25 * Time.deltaTime);
        planetObjects[2].transform.RotateAround(coreNode.position, new Vector3(0, 0, 90), 10 * Time.deltaTime);
    }
}
