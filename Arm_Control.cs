using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Edit and use this code however you wish, no credit necessary

// Important! This code assumes the tracking 3D model is facing towards the +Z axis

public class Arm_Control : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        //Optional Create Line For Debug
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.SetWidth(0.08f, 0.08f);

    }

    // Update is called once per frame
    void Update()
    {

//Get Reference GameObjects can also find/use Bones by name 

        var Shoulder = GameObject.Find("L_Shoulder_Sphere"); //Set Shoulder GameObject Tracker Object or Bone 
        var Elbow = GameObject.Find("L_Elbow_Sphere"); //Set Elbow GameObject Tracker Object or Bone 
        var Wrist = GameObject.Find("L_Wrist_Sphere"); //Set Wrist GameObject Tracker Object or Bone 

//Optional Draw Line for Debug

        LineRenderer LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.SetVertexCount(3);
        LineRenderer.SetPosition(0, Shoulder.transform.position);
        LineRenderer.SetPosition(1, Elbow.transform.position);
        LineRenderer.SetPosition(2, Wrist.transform.position);

//Project References onto Plane

        Vector3 wristProjection = Vector3.ProjectOnPlane(Wrist.transform.position, Vector3.forward);
        Vector3 elbowProjection = Vector3.ProjectOnPlane(Elbow.transform.position, Vector3.forward);
        Vector3 shoulderProjection = Vector3.ProjectOnPlane(Shoulder.transform.position, Vector3.forward);

//Get Shoulder Angle

        Vector2 Shoulder_Projected_Vector = shoulderProjection - elbowProjection;

        float Upper_Arm_Projected_Angle = Mathf.Atan2(Shoulder_Projected_Vector.y, Shoulder_Projected_Vector.x) * Mathf.Rad2Deg;  // radians to degrees

//Get Elbow Angle

        Vector2 Elbow_Projected_Vector = elbowProjection - wristProjection;
        float Forearm_Projected_Angle = Mathf.Atan2(Elbow_Projected_Vector.y, Elbow_Projected_Vector.x) * Mathf.Rad2Deg;  // radians to degrees

//Upper Arm Calculation

        var Model3D_Upper_Arm_Min = -90; // Minimum Y Axis 3D Rotation Value in Unity
        var Model3D_Upper_Arm_Max = 90;  // Maximum Y Axis 3D Rotation Value in Unity

        var Live2d_Upper_Arm_Min = -30; // Minimum Parameter Value in Live 2D
        var Live2d_Upper_Arm_Max = 30;  // Maximum Parameter Value in Live 2D

// Calculate Live2d Upper Arm Range 

        var Live2d_Upper_Arm_Range = Mathf.Max(Live2d_Upper_Arm_Min, Live2d_Upper_Arm_Max) - Mathf.Min(Live2d_Upper_Arm_Max, Live2d_Upper_Arm_Min);

        //Important to invert this angle ??
        var Inverted_Upper_Arm_Projected_Angle = Upper_Arm_Projected_Angle - Upper_Arm_Projected_Angle - Upper_Arm_Projected_Angle;
        //

        var Upper_Arm_Rotation_Percentage = (((Inverted_Upper_Arm_Projected_Angle - Model3D_Upper_Arm_Min) * 100f) / (Model3D_Upper_Arm_Max - Model3D_Upper_Arm_Min));

        var Calculated_UpperarmRotation = ((Upper_Arm_Rotation_Percentage * (Live2d_Upper_Arm_Range / 100f)) - (Live2d_Upper_Arm_Range / 2));

//End Upper Arm Calculation

//Forearm Calculation

        var Model3D_Forearm_Min = -180;    // Minimum Y Axis 3D Rotation Value in Unity
        var Model3D_Forearm_Max = 180; // Maximum Y Axis 3D Rotation Value in Unity

        var Live2d_Forearm_Min = -30; // Minimum Parameter Value in Live 2D
        var Live2d_Forearm_Max = 30;  // Maximum Parameter Value in Live 2D

//Calculate Live2d Forearm Range 

        var Live2d_Forearm_Range = Mathf.Max(Live2d_Forearm_Min, Live2d_Forearm_Max) - Mathf.Min(Live2d_Forearm_Max, Live2d_Forearm_Min);

//Calculate Differential Between Angles 

        var Calculate_Upperarm_Forearm_Differential_Angle = Upper_Arm_Projected_Angle - Forearm_Projected_Angle;

        var Forearm_Rotation_Percentage = (((Calculate_Upperarm_Forearm_Differential_Angle - Model3D_Forearm_Min) * 100f) / (Model3D_Forearm_Max - Model3D_Forearm_Min));

        //Important for if more than 100 and less than 0 ??? This Stops arms Locking / Skipping / Jumping  
                if (Forearm_Rotation_Percentage <= 0)
                {
                    Forearm_Rotation_Percentage = 100 + Forearm_Rotation_Percentage;
                }

                if (Forearm_Rotation_Percentage >= 100)
                {
                    Forearm_Rotation_Percentage = Forearm_Rotation_Percentage - 100;
                }
        //

        var Calculated_ForearmRotation = ((Forearm_Rotation_Percentage * (Live2d_Forearm_Range / 100f)) - (Live2d_Forearm_Range / 2));

//End Forearm Calculation


//Output Parameters for Live2d/VTube Studio

        //Upper Arm
        var Live2D_Shoulder_Rotation_Parameters = Calculated_UpperarmRotation;
        //Forearm
        var Live2D_Forearm_Rotation_Parameters = Calculated_ForearmRotation;

    }


}
