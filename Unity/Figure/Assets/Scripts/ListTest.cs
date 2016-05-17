
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListTest : MonoBehaviour
{
	List<string> someStringList = new List<string>();

	void Start()
	{
		// Calling method with variable number of parameters
		AddToList("One", "Two", "Three");

		for (int i = 0; i < someStringList.Count; i++)
			Debug.Log(someStringList[i]);

		// Two step AddRange()
		string[] input = { "four", "five", "six" };
		someStringList.AddRange(input);

		Debug.Log("-----");
		for (int i = 0; i < someStringList.Count; i++)
			Debug.Log(someStringList[i]);

		// One step AddRange()
		someStringList.AddRange(new string[] { "seven", "eight", "nine" });

		Debug.Log("-----");
		for (int i = 0; i < someStringList.Count; i++)
			Debug.Log(someStringList[i]);
	}
	// Function with a variable number of parameters
	void AddToList(params string[] list)
	{
		for (int i = 0; i < list.Length; i++)
		{
			someStringList.Add(list[i]);
		}
	}
}