using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 10f;
    public float gravity = -9.81f;

    public GameObject[] animals;
    public int animalsCaught = 0;

    public Vector3 velocity;
    public GameObject[] foodChoices;
    public GameObject selectedFood;
    Vector3 itemSpawnPoint;

    public TextMeshProUGUI foodUI;
    public TextMeshProUGUI caughtAnimalsDisp;
    public Image instructionsPanel;

    // vector motion from module 1
    void Start()
    {
        selectedFood = foodChoices[0];
        animals = GameObject.FindGameObjectsWithTag("Animal");
        foodUI.text = "Selected Food: " + selectedFood.name;
        UpdateAnimalCountDisp();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        GetItemSpawnPoint();
        SelectingFood();

        if (Input.GetKeyDown(KeyCode.Q))
            Instantiate(selectedFood, itemSpawnPoint, selectedFood.transform.rotation);
        if (Input.anyKeyDown)
            instructionsPanel.gameObject.SetActive(false);

        if (animalsCaught == animals.Length - 1)
            caughtAnimalsDisp.text = "Caught all animals!";
    }

    void SelectingFood()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedFood = foodChoices[0];    
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedFood = foodChoices[1];
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedFood = foodChoices[2];

        foodUI.text = "Selected Food: " + selectedFood.name;
    }

    void GetItemSpawnPoint()
    {
        itemSpawnPoint = gameObject.transform.position;
        itemSpawnPoint.y -= 1.05f;
    }

    public void UpdateAnimalCountDisp()
    {
        caughtAnimalsDisp.text = "Animals Caught: " + animalsCaught + " / " + (animals.Length - 1);
    }
}
