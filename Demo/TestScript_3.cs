using UnityEngine;

namespace Shashki.ComponentsAdder.Demo
{
    public class TestScript_3 : MonoBehaviour
    {
        [SerializeField] private GameObject _field3;

        private void Start() =>
                    print(this + " added. Field value: " + _field3.name);
    }

}