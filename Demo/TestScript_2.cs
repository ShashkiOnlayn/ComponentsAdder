using UnityEngine;

namespace Shashki.ComponentsAdder.Demo
{
    public class TestScript_2 : MonoBehaviour
    {
        [SerializeField] private GameObject _field2;

        private void Start() =>
                    print(this + " added. Field value: " + _field2.name);
    } 
}