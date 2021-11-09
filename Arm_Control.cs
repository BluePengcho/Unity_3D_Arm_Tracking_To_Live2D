using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Edit and use this code however you wish, no credit necessary

// Important! This code assumes the tracking 3D model is facing towards the +Z axis

public class Arm_Control : MonoBehaviour
{

//Set Motion Trackers and Parameters Values in Inspector

    [Header("Left Arm")]
    [Header("Motion Trackers (GameObjects or Bones)")]
    public GameObject LeftShoulder; 
    public GameObject LeftElbow;
    public GameObject LeftWrist;
    [Header("Right Arm")]
    public GameObject RightShoulder;
    public GameObject RightElbow;
    public GameObject RightWrist;
    [Header("Left Arm")]
    [Header("Live2D Parameters")]
    public float LeftUpperArmMin = -30; //Default Left Upper Minimum Parameters Value in Live2D
    public float LeftUpperArmMax = 30; //Default Left Upper Maximum Parameters Value in Live2D

    public float LeftForearmMin = -30; //Default Left Forearm Maximum Parameters Value in Live2D
    public float LeftForearmMax = 30; //Default Left Forearm Maximum Parameters Value in Live2D

    [Header("Right Arm")]
    public float RightUpperArmMin = -30; //Default Right Upper Minimum Parameters Value in Live2D
    public float RightUpperArmMax = 30; //Default Right Upper Maximum Parameters Value in Live2D

    public float RightForearmMin = -30; //Default Right Forearm Minimum Parameters Value in Live2D
    public float RightForearmMax = 30; //Default Right Forearm Maximum Parameters Value in Live2D



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

//Get Motion Trackers (GameObjects or Bones) 

        //Left Arm
        var Left_Shoulder = LeftShoulder; //Set Left Shoulder Tracker Object/Bone 
        var Left_Elbow = LeftElbow; //Set Left Elbow Tracker Object/Bone 
        var Left_Wrist = LeftWrist; //Set Left Wrist Tracker Object/Bone 

        //Right Arm
        var Right_Shoulder = RightShoulder; //Set Right Shoulder Tracker Object/Bone 
        var Right_Elbow = RightElbow; //Set Right Elbow Tracker Object/Bone 
        var Right_Wrist = RightWrist; //Set Right Wrist Tracker Object/Bone 

        //Optional Draw Line for Debug

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(6);
        lineRenderer.SetPosition(0, Left_Wrist.transform.position);
        lineRenderer.SetPosition(1, Left_Elbow.transform.position);
        lineRenderer.SetPosition(2, Left_Shoulder.transform.position);
        lineRenderer.SetPosition(3, Right_Shoulder.transform.position);
        lineRenderer.SetPosition(4, Right_Elbow.transform.position);
        lineRenderer.SetPosition(5, Right_Wrist.transform.position);

//Left Arm

    //Project Tracker References onto Plane

        Vector3 Left_wristProjection = Vector3.ProjectOnPlane(Left_Wrist.transform.position, Vector3.forward);
        Vector3 Left_elbowProjection = Vector3.ProjectOnPlane(Left_Elbow.transform.position, Vector3.forward);
        Vector3 Left_shoulderProjection = Vector3.ProjectOnPlane(Left_Shoulder.transform.position, Vector3.forward);

    //Get Shoulder Angle

        Vector2 Left_Shoulder_Projected_Vector = Left_shoulderProjection - Left_elbowProjection;

        float Left_Upper_Arm_Projected_Angle = Mathf.Atan2(Left_Shoulder_Projected_Vector.y, Left_Shoulder_Projected_Vector.x) * Mathf.Rad2Deg;  // radians to degrees

    //Get Elbow Angle

        Vector2 Left_Elbow_Projected_Vector = Left_elbowProjection - Left_wristProjection;
        float Left_Forearm_Projected_Angle = Mathf.Atan2(Left_Elbow_Projected_Vector.y, Left_Elbow_Projected_Vector.x) * Mathf.Rad2Deg;  // radians to degrees

    //Upper Arm Calculation

        var Left_Model3D_Upper_Arm_Min = -90; // Minimum Y Axis 3D Rotation Value in Unity
        var Left_Model3D_Upper_Arm_Max = 90;  // Maximum Y Axis 3D Rotation Value in Unity

        var Left_Live2d_Upper_Arm_Min = LeftUpperArmMin; // Minimum Parameter Value in Live 2D
        var Left_Live2d_Upper_Arm_Max = LeftUpperArmMax;  // Maximum Parameter Value in Live 2D

    // Calculate Live2d Upper Arm Range 

        var Left_Live2d_Upper_Arm_Range = Mathf.Max(Left_Live2d_Upper_Arm_Min, Left_Live2d_Upper_Arm_Max) - Mathf.Min(Left_Live2d_Upper_Arm_Max, Left_Live2d_Upper_Arm_Min);

        //Important to invert this angle ??
        var Left_Inverted_Upper_Arm_Projected_Angle = Left_Upper_Arm_Projected_Angle - Left_Upper_Arm_Projected_Angle - Left_Upper_Arm_Projected_Angle;
        //

        var Left_Upper_Arm_Rotation_Percentage = (((Left_Inverted_Upper_Arm_Projected_Angle - Left_Model3D_Upper_Arm_Min) * 100f) / (Left_Model3D_Upper_Arm_Max - Left_Model3D_Upper_Arm_Min));

        var Left_Calculated_UpperarmRotation = ((Left_Upper_Arm_Rotation_Percentage * (Left_Live2d_Upper_Arm_Range / 100f)) - (Left_Live2d_Upper_Arm_Range / 2));

    //End Upper Arm Calculation

    //Forearm Calculation

        var Left_Model3D_Forearm_Min = -180; // Minimum Y Axis 3D Rotation Value in Unity
        var Left_Model3D_Forearm_Max = 180; // Maximum Y Axis 3D Rotation Value in Unity

        var Left_Live2d_Forearm_Min = LeftForearmMin; // Minimum Parameter Value in Live 2D
        var Left_Live2d_Forearm_Max = LeftForearmMax; // Maximum Parameter Value in Live 2D

    //Calculate Live2d Forearm Range 

        var Left_Live2d_Forearm_Range = Mathf.Max(Left_Live2d_Forearm_Min, Left_Live2d_Forearm_Max) - Mathf.Min(Left_Live2d_Forearm_Max, Left_Live2d_Forearm_Min);

    //Calculate Differential Between Angles 

        var Left_Calculate_Upperarm_Forearm_Differential_Angle = Left_Upper_Arm_Projected_Angle - Left_Forearm_Projected_Angle;

        var Left_Forearm_Rotation_Percentage = (((Left_Calculate_Upperarm_Forearm_Differential_Angle - Left_Model3D_Forearm_Min) * 100f) / (Left_Model3D_Forearm_Max - Left_Model3D_Forearm_Min));

        //Important for if more than 100 and less than 0 ??? This Stops arms Locking / Skipping / Jumping  
                if (Left_Forearm_Rotation_Percentage <= 0)
                {
                    Left_Forearm_Rotation_Percentage = 100 + Left_Forearm_Rotation_Percentage;
                }

                if (Left_Forearm_Rotation_Percentage >= 100)
                {
                    Left_Forearm_Rotation_Percentage = Left_Forearm_Rotation_Percentage - 100;
                }
        //

        var Left_Calculated_ForearmRotation = ((Left_Forearm_Rotation_Percentage * (Left_Live2d_Forearm_Range / 100f)) - (Left_Live2d_Forearm_Range / 2));

    //End Forearm Calculation

//End of Left Arm

//Right Arm

    //Project Tracker References onto Plane

        Vector3 Right_wristProjection = Vector3.ProjectOnPlane(Right_Wrist.transform.position, Vector3.forward);
        Vector3 Right_elbowProjection = Vector3.ProjectOnPlane(Right_Elbow.transform.position, Vector3.forward);
        Vector3 Right_shoulderProjection = Vector3.ProjectOnPlane(Right_Shoulder.transform.position, Vector3.forward);

    //Get Shoulder Angle

        Vector2 Right_Shoulder_Projected_Vector = Right_elbowProjection - Right_shoulderProjection;

        float Right_Upper_Arm_Projected_Angle = Mathf.Atan2(Right_Shoulder_Projected_Vector.y, Right_Shoulder_Projected_Vector.x) * Mathf.Rad2Deg;  // radians to degrees

    //Get Elbow Angle

        Vector2 Right_Elbow_Projected_Vector = Right_wristProjection - Right_elbowProjection;

        float Right_Forearm_Projected_Angle = Mathf.Atan2(Right_Elbow_Projected_Vector.y, Right_Elbow_Projected_Vector.x) * Mathf.Rad2Deg;  // radians to degrees

    //Upper Arm Calculation

        var Right_Model3D_Upper_Arm_Min = -90; // Minimum Y Axis 3D Rotation Value in Unity
        var Right_Model3D_Upper_Arm_Max = 90;  // Maximum Y Axis 3D Rotation Value in Unity

        var Right_Live2d_Upper_Arm_Min = RightUpperArmMin; // Minimum Parameter Value in Live 2D
        var Right_Live2d_Upper_Arm_Max = RightUpperArmMax;  // Maximum Parameter Value in Live 2D

    // Calculate Live2d Upper Arm Range 

        var Right_Live2d_Upper_Arm_Range = Mathf.Max(Right_Live2d_Upper_Arm_Min, Right_Live2d_Upper_Arm_Max) - Mathf.Min(Right_Live2d_Upper_Arm_Max, Right_Live2d_Upper_Arm_Min);

        //Important there is no need to invert this angle for the right arm ??
        //var Right_Inverted_Upper_Arm_Projected_Angle = Right_Upper_Arm_Projected_Angle;
        //

        var Right_Upper_Arm_Rotation_Percentage = (((Right_Upper_Arm_Projected_Angle - Right_Model3D_Upper_Arm_Min) * 100f) / (Right_Model3D_Upper_Arm_Max - Right_Model3D_Upper_Arm_Min));

        var Right_Calculated_UpperarmRotation = ((Right_Upper_Arm_Rotation_Percentage * (Right_Live2d_Upper_Arm_Range / 100f)) - (Right_Live2d_Upper_Arm_Range / 2));

    //End Upper Arm Calculation

    //Forearm Calculation

        var Right_Model3D_Forearm_Min = -180; // Minimum Y Axis 3D Rotation Value in Unity
        var Right_Model3D_Forearm_Max = 180; // Maximum Y Axis 3D Rotation Value in Unity

        var Right_Live2d_Forearm_Min = RightForearmMin; // Minimum Parameter Value in Live 2D
        var Right_Live2d_Forearm_Max = RightForearmMax; // Maximum Parameter Value in Live 2D

    //Calculate Live2d Forearm Range 

        var Right_Live2d_Forearm_Range = Mathf.Max(Right_Live2d_Forearm_Min, Right_Live2d_Forearm_Max) - Mathf.Min(Right_Live2d_Forearm_Max, Right_Live2d_Forearm_Min);
        
        //Important there is no need to invert this angle for the right arm ??
        //var Right_Inverted_Upper_Arm_Projected_Angle = Right_Upper_Arm_Projected_Angle;
        //

    //Calculate Differential Between Angles 

        var Right_Calculate_Upperarm_Forearm_Differential_Angle = Right_Forearm_Projected_Angle - Right_Upper_Arm_Projected_Angle;

        var Right_Forearm_Rotation_Percentage = (((Right_Calculate_Upperarm_Forearm_Differential_Angle - Right_Model3D_Forearm_Min) * 100f) / (Right_Model3D_Forearm_Max - Right_Model3D_Forearm_Min));

        //Important for if more than 100 and less than 0 ??? This Stops arms Locking / Skipping / Jumping  
            if (Right_Forearm_Rotation_Percentage <= 0)
            {
                Right_Forearm_Rotation_Percentage = 100 + Right_Forearm_Rotation_Percentage;
            }

            if (Right_Forearm_Rotation_Percentage >= 100)
            {
                Right_Forearm_Rotation_Percentage = Right_Forearm_Rotation_Percentage - 100;
            }
        //

        var Right_Calculated_ForearmRotation = ((Right_Forearm_Rotation_Percentage * (Right_Live2d_Forearm_Range / 100f)) - (Right_Live2d_Forearm_Range / 2));

    //End Forearm Calculation

//End of Right Arm

//Output to Live2D/VTube Studio

    //Left Arm

        //Upper Arm
        var Live2D_Left_Shoulder_Rotation_Parameter = Left_Calculated_UpperarmRotation;
        //Forearm
        var Live2D_Left_Forearm_Rotation_Parameter = Left_Calculated_ForearmRotation;

    //Right Arm

        //Upper Arm
        var Live2D_Right_Shoulder_Rotation_Parameter = Right_Calculated_UpperarmRotation;
        //Forearm
        var Live2D_Right_Forearm_Rotation_Parameter = Right_Calculated_ForearmRotation;

    }


}
