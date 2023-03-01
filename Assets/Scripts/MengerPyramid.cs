using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MengerPyramid : MonoBehaviour
{

    public GameObject prefab;


    public float size = 300f;

    public int numRecursions;

    public List<List<GameObject>> cubeParents;

    private GameObject masterCube;

    private int clockCounter;

    private bool expanding;

    List<GameObject> cubes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        cubeParents = new List<List<GameObject>>();
        clockCounter = 0;
        expanding = true;
        GameObject go = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
        masterCube = go;
        //go.transform.localScale = new Vector3(size, size, size);
        go.GetComponent<MengerBox>().size = size;
        go.GetComponent<MengerBox>().recurrsionLayer = 0;

        cubes.Add(go);




        // Build the Menger Sponge to the desire # of recursions
        for (int i = 0; i < numRecursions; i++)
        {
            List<GameObject> newCubes = Split(cubes, i+1);
            cubeParents.Add(newCubes);
            cubes = newCubes;
        }


    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        int pauseTime = 30;
        int expandTime = 180;
        if (expanding)
        {
            clockCounter++;
        } else {
            clockCounter--;
        }

        if(clockCounter > (3*pauseTime) + (3*expandTime) )
        {
            expanding = !expanding;
        } else if(clockCounter < 1)
        {
            expanding = !expanding;
        }

        // TODO: make this clean

        // First layer
        if (clockCounter <= expandTime)
        {
            foreach (var c in cubeParents[0])
            {
                if (expanding) 
                { 
                    c.transform.position += (c.transform.position - c.transform.parent.position )/300f;
                } else {
                    c.transform.position -= (c.transform.position - c.transform.parent.position )/300f;
                }

            }
            return;
        }
        // First pause
        if (clockCounter > expandTime && clockCounter < (expandTime + pauseTime)){return;}



        // Second layer
        if (clockCounter > (expandTime + pauseTime) && clockCounter <= pauseTime + (2*expandTime))
        {
            foreach (var c in cubeParents[1])
            {
                if (expanding) 
                { 
                    c.transform.position += (c.transform.position - c.transform.parent.position )/300f;
                } else {
                    c.transform.position -= (c.transform.position - c.transform.parent.position )/300f;
                }
            }
            return;
        }
        // Second pause
        if (clockCounter > pauseTime + (2*expandTime) && clockCounter < (2*pauseTime) + (2*expandTime)){return;}


        // Third layer
        if (clockCounter > (2*pauseTime) + (2*expandTime) && clockCounter <= (2*pauseTime) + (3*expandTime))
        {
            foreach (var c in cubeParents[2])
            {
                if (expanding) 
                { 
                    c.transform.position += (c.transform.position - c.transform.parent.position )/300f;
                } else {
                    c.transform.position -= (c.transform.position - c.transform.parent.position )/300f;
                }
            }
            return;
        }
        // Third pause
        if (clockCounter > (2*pauseTime) + (3*expandTime) && clockCounter < (3*pauseTime) + (3*expandTime)){return;}
    } 

    List<GameObject> Split (List<GameObject> cubes, int recDepth)
    {
        List<GameObject> boxes = new List<GameObject>();

        foreach (var cube in cubes)
        {
            // Get size of current object
            float size = cube.GetComponent<MengerBox>().size;

             // Calculate the new size
            float newSize = size/2f;
            //Debug.Log(newSize)

            // Calculate slant height of pyramid
            // s = SQRT(h^2 + (1/12)a^2)
            // h = height of equilateral tri and a is side length
            float slantHeight = Mathf.Sqrt(Mathf.Pow((0.866f * newSize),2) + (0.0833f * Mathf.Pow((0.866f * newSize),2)));

            // Get magnitude of vector to move smaller triangles
            float dirMagnitude = 0.75f * slantHeight;
            
           

            Vector3 posUp = new Vector3(cube.transform.position.x, cube.transform.position.y+dirMagnitude, cube.transform.position.z);

            Vector3 lowerVector = new Vector3(dirMagnitude*0.875f, -slantHeight/4f,0f);

            
            
            // Instantiate the new cube object
            for (int i = 0; i < 4; i++)
            {
                Vector3 cubePos;
                if (i == 0)
                {
                    // Create position vector of the new cube
                    cubePos = posUp;
                } else {
                    cubePos = cube.transform.position + (Quaternion.AngleAxis((i-1)*120f, Vector3.up) * lowerVector);
                }
                GameObject copy;
                if (recDepth == numRecursions)
                {
                    
                    copy = Instantiate(prefab, cubePos, Quaternion.identity, cube.gameObject.transform);
                    // Set scale of cube to be correct
                    copy.transform.localScale = new Vector3(newSize, newSize, newSize);
                    
                } else {
                    //Debug.Log(recDepth);
                    copy = Instantiate(prefab, cubePos, Quaternion.identity, cube.gameObject.transform);
                }
                
                
                copy.GetComponent<MengerBox>().size = newSize;
                // Set the cube recurrsion id 
                copy.GetComponent<MengerBox>().recurrsionLayer = recDepth;
                
                // Add the cube to the return array
                boxes.Add(copy);
            }




            // Disable parent cube attributes so they dont appear on screen
            // Done this way so we can still move all subcubes by moving parent
            cube.GetComponent<MeshRenderer>().enabled = false;
            //cube.GetComponent<MeshCollider>().enabled = false;
    }

        
    return boxes;

    }
}


