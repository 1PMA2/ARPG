#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitController))]
public class UnitControllerInspector : Editor
{
    //private SerializedProperty _isPlayer;
    //private SerializedProperty _cameraArm;
    //private SerializedProperty _unitCamera;
    //private SerializedProperty _kanata;
    //private SerializedProperty _smash;
    //private SerializedProperty _targetRotation;
    //private SerializedProperty _keyDelta;
    //private SerializedProperty _inputDir;
    //private SerializedProperty _rotationSpeed;
    //private SerializedProperty _targetMoveSpeed;
    //private SerializedProperty _turnAngle;
    //private SerializedProperty _actionZoom;
    //private SerializedProperty _distance;
    //private SerializedProperty _smashSpeed;
    //private SerializedProperty _weaponTrigger;
    //private SerializedProperty _maxcounter;


    private void OnEnable()
    {
        //_isPlayer = serializedObject.FindProperty("isPlayer");
        //_cameraArm = serializedObject.FindProperty("cameraArm");
        //_unitCamera = serializedObject.FindProperty("unitCamera");
        //_kanata = serializedObject.FindProperty("kanata");
        //_smash = serializedObject.FindProperty("smash");
        //_targetRotation = serializedObject.FindProperty("targetRotation");
        //_keyDelta = serializedObject.FindProperty("keyDelta");
        //_inputDir = serializedObject.FindProperty("inputDir");
        //_rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        //_targetMoveSpeed = serializedObject.FindProperty("targetMoveSpeed");
        //_turnAngle = serializedObject.FindProperty("turnAngle");
        //_actionZoom = serializedObject.FindProperty("actionZoom");
        //_distance = serializedObject.FindProperty("distance");
        //_smashSpeed = serializedObject.FindProperty("smashSpeed");
        //_weaponTrigger = serializedObject.FindProperty("weaponTrigger");
        //_maxcounter = serializedObject.FindProperty("maxCounter");
    }
    public override void OnInspectorGUI()
    {
        //serializedObject.Update();

        //EditorGUILayout.PropertyField(_isPlayer);

        //_isPlayer.boolValue = EditorGUILayout.Foldout(_isPlayer.boolValue, "Player");

        //if (_isPlayer.boolValue)
        //{
        //    EditorGUILayout.PropertyField(_cameraArm);
        //    EditorGUILayout.PropertyField(_unitCamera);
        //    EditorGUILayout.PropertyField(_kanata);
        //    EditorGUILayout.PropertyField(_smash);
        //    EditorGUILayout.PropertyField(_targetRotation);
        //    EditorGUILayout.PropertyField(_keyDelta);
        //    EditorGUILayout.PropertyField(_inputDir);
        //    EditorGUILayout.PropertyField(_rotationSpeed);
        //    EditorGUILayout.PropertyField(_targetMoveSpeed);
        //    EditorGUILayout.PropertyField(_turnAngle);
        //    EditorGUILayout.PropertyField(_actionZoom);
        //    EditorGUILayout.PropertyField(_distance);
        //    EditorGUILayout.PropertyField(_smashSpeed);
        //    //EditorGUILayout.PropertyField(_maxcounter);
        //}

        //EditorGUILayout.PropertyField(_weaponTrigger);
        //UnitController unitController = (UnitController)target;

        //unitController.nearUnitTransform = (Transform)EditorGUILayout.ObjectField("Near Transform", unitController.nearUnitTransform, typeof(Transform), true);

        //serializedObject.ApplyModifiedProperties();
    }
}
#endif