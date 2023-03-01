using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireRing1 : MonoBehaviour
{

    public GameObject piecePrefab;
    public GameObject edgePiecePrefab;

    private float initialRadius = 30f;

    private float scaleFactor = 0.9537349f;

    private int numDivisions = 64;

    private int spiralLength = 64;

    // Start is called before the first frame update
    void Start()
    {
        // Offset step size in degrees to start the next spiral at
        float divRotation = 360f/numDivisions;

        // Value for adjusting the scale value to extend the cylinder to make it connect with the next piece
        // Divide by 2 here since cylinder length is double scale since it goes in both directions from starting position
        float a = Mathf.Sqrt(Mathf.Pow(scaleFactor,2f) + 1f - 2f*scaleFactor*Mathf.Cos(divRotation*Mathf.PI/180f));
        float conLengthScale = a/2f;

        // Rotation value for connecting the next piece to the old one
        //float connectionRotation = 25.5f; 
        float C = (Mathf.Asin(Mathf.Sin(divRotation*Mathf.PI/180f) * scaleFactor / a));
        float connectionRotation = 90f - C*180f/Mathf.PI;
        
        // Scale of values for the outer ring so they connect
        float outerRingScale = (Mathf.Sin(divRotation*Mathf.PI/180f) * initialRadius)/ Mathf.Sin((180f-divRotation)/2f*Mathf.PI/180f) /2f;

        // Build out the base ring
        for (int i = 0; i < numDivisions; i++) {
            GameObject go = Instantiate(edgePiecePrefab, new Vector3(initialRadius*Mathf.Cos((i+0.5f)*divRotation*Mathf.PI/180f),
                                                            0,
                                                            initialRadius*Mathf.Sin((i+0.5f)*divRotation*Mathf.PI/180f)), Quaternion.identity);
            go.transform.localEulerAngles = new Vector3(90,-(i+0.5f)*divRotation,0);
            Debug.Log(initialRadius*Mathf.Sin((i*(90f/64f))*Mathf.PI/180f));
            go.transform.localScale = new Vector3(  go.transform.localScale.x,
                                                    go.transform.localScale.y*outerRingScale,
                                                    go.transform.localScale.z); 
        }

        // BUILDING SPIRALS
        // Build counter clockwise spiral
        // For each starting point around the circle make a spiral
        for (int j = 0; j < numDivisions; j++) {
            // Place each cylinder on the spiral
            for (int i = 0; i < spiralLength; i++) {
                // float layerHeight = Mathf.Pow(scaleFactor, spiralLength - i)*initialRadius*Mathf.Sin(C);
                float layerHeight = 0; //initialRadius  - (initialRadius*Mathf.Exp(-(1/64f)*i));
                // Instantiate the object at the correct location
                GameObject go = Instantiate(piecePrefab, new Vector3(Mathf.Pow(scaleFactor, i)*initialRadius*Mathf.Cos((i+j)*divRotation*Mathf.PI/180f),
                                                                layerHeight,
                                                                Mathf.Pow(scaleFactor, i)*initialRadius*Mathf.Sin((i+j)*divRotation*Mathf.PI/180f)), Quaternion.identity);
               
                // Rotate the cylinder to be at the correct angle in spiral
                go.transform.localEulerAngles = new Vector3(0,-connectionRotation -((i+j)*divRotation),0);

                // Down scale to make the cylinders thinner as we go in
                go.transform.localScale = new Vector3(  Mathf.Pow(scaleFactor, i)*go.transform.localScale.x,
                                                        Mathf.Pow(scaleFactor, i)*go.transform.localScale.y,
                                                        Mathf.Pow(scaleFactor, i)*go.transform.localScale.z*(initialRadius*conLengthScale)); 
   
                // Instantiate the object at the correct location
                GameObject go2 = Instantiate(piecePrefab, new Vector3(-Mathf.Pow(scaleFactor, i)*initialRadius*Mathf.Cos((i+j)*divRotation*Mathf.PI/180f),
                                                                layerHeight,
                                                                -Mathf.Pow(scaleFactor, i)*initialRadius*Mathf.Sin((i+j)*divRotation*Mathf.PI/180f)), Quaternion.identity);
                // Rotate the cylinder to be at the correct angle in spiral
                go2.transform.localEulerAngles = new Vector3(0,connectionRotation-((i+j)*divRotation),0);

                // Down scale to make the cylinders thinner as we go in
                go2.transform.localScale = new Vector3(  Mathf.Pow(scaleFactor, i)*go2.transform.localScale.x,
                                                        Mathf.Pow(scaleFactor, i)*go2.transform.localScale.y,
                                                        Mathf.Pow(scaleFactor, i)*go2.transform.localScale.z*(initialRadius*conLengthScale));
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
