# Unity_3D_Arm_Tracking_To_Live2D

The easiest method to use this code is to add the script 'Arm_Control.cs' to any game object and within the Inspector selecting (drag and drop) the appropriate trackers or bones of the 3D Model to the corresponding Motion Tracker Values. The Live2D Parameters values in the Inspector should be set to the Minimum and Maximum Values set in the Live2D Cubism Editor.

Otherwise this code should be stand-alone and can hopefully be copied and pasted into your tracking code.

In the 'Arm_Control.cs' script the following outputs need to be passed onto the Live2D model or onto VTube Studio :-  
- Live2D_Left_Shoulder_Rotation_Parameter <br />
- Live2D_Left_Forearm_Rotation_Parameter <br />
- Live2D_Right_Shoulder_Rotation_Parameter <br />
- Live2D_Right_Forearm_Rotation_Parameter

Ver.2 (Current Version) - Added tracking for right arm and made the script a bit more user friendly 

Ver.1 - Only tracks and creates parameter values for the left arm shoulder and elbow.

Future versions plan on including:-
- Adding smoothing to the output parameter values <br />
- Adding wrist orientation tracking <br />
- Adding upper and forearm extension tracking <br />
- Possibly adding camera tracking using mediapipe for arm input?? <br />

Note: For best results outputting to VTube Studio, elbow smoothing should be set to Zero in VTube Studio.

This is necessary as VTube studio smoothes between a linear range e.g. 0 to 360 which result in the elbow looping back on itself 

E.g. for a total parameter range of 0 to 360 when the parameter moves from a value of 350 to 10 Vtube Studio will smooth a total range of 340 (going down from value of 350 to 10), where the method used in this code needs to smooth in a loop/circle to a total range of 20 (going up from a value of 350 to 10). Hopefully this is understandable, note this is my best understating of this problem. 

Would be nice in the future if VTube Studio could smooth in the values of a circle.

Please check the Live2d_Parameters_Reference image to see how the parameters are set for this code in the Live2D Cubism Editor.

Any problems or questions please let me know on Twitter @Blue_Pengcho (apologies might be slow to respond)
