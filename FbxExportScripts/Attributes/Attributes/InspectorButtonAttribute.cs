using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

[System.AttributeUsage(System.AttributeTargets.All)]
public class InspectorButtonAttribute : PropertyAttribute
{
    public readonly string MethodName;
    public InspectorButtonAttribute(string methodName)
    {
        MethodName = methodName;
    }

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
public class InspectorButtonAttributeDrawer : PropertyDrawer
{
    private MethodInfo _eventMethodInfo = null;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        InspectorButtonAttribute inspectorButtonAttribute = (InspectorButtonAttribute)attribute;
        float buttonLength = position.width;
        Rect buttonRect = new Rect(position.x, position.y, buttonLength, position.height);
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        if (GUI.Button(buttonRect, inspectorButtonAttribute.MethodName))
        {
            System.Type eventOwnerType = property.serializedObject.targetObject.GetType();
            string eventName = inspectorButtonAttribute.MethodName;
            
            //通过反射获取方法
            if (_eventMethodInfo == null)
            {
                _eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }

            //判断是否获取到方法
            if (_eventMethodInfo != null)
            {
                _eventMethodInfo.Invoke(property.serializedObject.targetObject, null);
            }
            else
            {
                Debug.LogWarningFormat("InspectorButton:Unable to find method {0} in {1}",eventName,eventOwnerType);
            }
        }
    }
}
#endif







