using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Inventory))]
public class InventoryEditor : Editor {

    private SerializedProperty itemImagesProporty;
    private SerializedProperty itemsProporty;
    private bool[] showItemSlot = new bool[Inventory.numItemSlot];

    private const string inventoryPropItemImagesName = "itemImages";
    private const string inventoryPropItemsName = "items";

    private void OnEnable()
    {
        itemImagesProporty = serializedObject.FindProperty(inventoryPropItemImagesName);
        itemsProporty = serializedObject.FindProperty(inventoryPropItemsName);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < Inventory.numItemSlot; i++)
        {
            ItemSlotGUI(i);
        }


        serializedObject.ApplyModifiedProperties();
    }


    private void ItemSlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        showItemSlot[index] = EditorGUILayout.Foldout(showItemSlot[index], "Item Slot"+ index);

        if (showItemSlot[index])
        {
            EditorGUILayout.PropertyField(itemImagesProporty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(itemsProporty.GetArrayElementAtIndex(index));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

}
