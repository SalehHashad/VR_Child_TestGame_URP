using UnityEngine;

namespace floatgrids
{
    public abstract class FGTokenizable : MonoBehaviour
    {
        public abstract void Setup();
        public void OnValidate()
        {
            Setup();
        }
    }
}
