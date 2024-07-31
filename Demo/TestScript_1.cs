using UnityEngine;

namespace Shashki.ComponentsAdder.Demo
{
    public class TestScript_1 : MonoBehaviour
    {
        [SerializeField] private GameObject _field1;

        private void Start() =>
                    print(this + " added. Field value: " + _field1.name);
    } 
}