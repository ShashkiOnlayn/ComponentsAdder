using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Shashki.ComponentsAdder.Data
{
	[CreateAssetMenu(fileName = "ComponentsData", menuName = "ComponentsAdder/ComponentsData", order = 1)]
	public class ComponentsData : ScriptableObject
	{
		public List<MonoScript> scripts = new();
		public List<Component> components = new();
	} 
}