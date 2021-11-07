# Unity_3D_Arm_Tracking_To_Live2D

This code should be stand-alone and can hopefully be copied and pasted into your tracking code.

Otherwise Arm_Control.cs script must be added to the desired 3D tracking model/game object in Unity.

The output Live2D_Shoulder_Rotation_Parameters & Live2D_Forearm_Rotation_Parameters need to be passed on / sent to the Live2d model or onto Vtube Studio.

This versions is currently only tracking and creating parameter values for the left arm shoulder and elbow.

Future versions plan on including adding tracking to right arm shoulder and elbow, adding smoothing to the output parameter values, possibly adding camera tracking for arm input??

Note: For best results outputting to VTube Studio, elbow smoothing should be set to Zero in VTube Studio.

This is necessary as unfortunately Vtube studio smoothes between a linear range e.g. 0 to 360 which result in the elbow looping back on itself 

E.g. for a total parameter range of 0 to 360 when the parameter moves from a value of 350 to 10 Vtube Studio will smooth a total range of 340 (going down from value of 350 to 10), where the method used in this code needs to smooth in a loop/circle to a total range of 20 (going up from a value of 350 to 10). Hopefully this is understandable.

Note this is my best understating of this problem. 

Would be nice in the future if VTube Studio could smooth in the values of a circle.

Please check the Live2d_Parameters_Reference image to see how the parameters are set for this code

Any problems or question please let me know on Twitter @Blue_Pengcho (apologies might be slow to response) 
