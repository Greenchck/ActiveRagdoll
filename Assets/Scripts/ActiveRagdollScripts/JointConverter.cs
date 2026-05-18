using UnityEngine;
using UnityEditor;

public class JointConverter : EditorWindow
{
    [MenuItem("Active-Ragdoll/Convert Character Joints to Configurable Joints")]
    public static void ConvertJoints()
    {
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogWarning("Please select the main character object containing the ragdoll!");
            return;
        }

        CharacterJoint[] characterJoints = selectedObject.GetComponentsInChildren<CharacterJoint>();

        if (characterJoints.Length == 0)
        {
            Debug.Log("No Character Joint found in the selected object.");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(selectedObject, "Convert Joints");

        int convertedCount = 0;

        foreach (CharacterJoint cj in characterJoints)
        {
            GameObject bone = cj.gameObject;

            Rigidbody connectedBody = cj.connectedBody;
            Vector3 anchor = cj.anchor;
            Vector3 connectedAnchor = cj.connectedAnchor;
            bool autoConfigureConnectedAnchor = cj.autoConfigureConnectedAnchor;
            Vector3 axis = cj.axis;
            Vector3 swingAxis = cj.swingAxis;
            
            SoftJointLimit lowTwistLimit = cj.lowTwistLimit;
            SoftJointLimit highTwistLimit = cj.highTwistLimit;
            SoftJointLimit swing1Limit = cj.swing1Limit;
            SoftJointLimit swing2Limit = cj.swing2Limit;

            float breakForce = cj.breakForce;
            float breakTorque = cj.breakTorque;
            bool enableCollision = cj.enableCollision;
            bool enablePreprocessing = cj.enablePreprocessing;
            float massScale = cj.massScale;
            float connectedMassScale = cj.connectedMassScale;

            DestroyImmediate(cj);

            ConfigurableJoint confJoint = bone.AddComponent<ConfigurableJoint>();

            confJoint.connectedBody = connectedBody;
            confJoint.anchor = anchor;
            confJoint.connectedAnchor = connectedAnchor;
            confJoint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
            confJoint.axis = axis;
            confJoint.secondaryAxis = swingAxis;
            
            confJoint.breakForce = breakForce;
            confJoint.breakTorque = breakTorque;
            confJoint.enableCollision = enableCollision;
            confJoint.enablePreprocessing = enablePreprocessing;
            confJoint.massScale = massScale;
            confJoint.connectedMassScale = connectedMassScale;

            confJoint.xMotion = ConfigurableJointMotion.Locked;
            confJoint.yMotion = ConfigurableJointMotion.Locked;
            confJoint.zMotion = ConfigurableJointMotion.Locked;

            confJoint.angularXMotion = ConfigurableJointMotion.Limited;
            confJoint.angularYMotion = ConfigurableJointMotion.Limited;
            confJoint.angularZMotion = ConfigurableJointMotion.Limited;

            confJoint.lowAngularXLimit = lowTwistLimit;
            confJoint.highAngularXLimit = highTwistLimit;
            confJoint.angularYLimit = swing1Limit;
            confJoint.angularZLimit = swing2Limit;

            confJoint.rotationDriveMode = RotationDriveMode.Slerp;

            JointDrive slerpDrive = new JointDrive();
            slerpDrive.positionSpring = 1000f;
            slerpDrive.positionDamper = 100f;
            slerpDrive.maximumForce = Mathf.Infinity;
            confJoint.slerpDrive = slerpDrive;

            convertedCount++;
        }

        Debug.Log($"Success! Converted {convertedCount} Character Joints to Configurable Joints.");
    }
}
