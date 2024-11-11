using System.Collections.Generic;
using UnityEngine;

namespace floatgrids
{
    public class FGOnboarding : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _steps;

        [SerializeField]
        private TMPro.TextMeshProUGUI _stepText;

        [SerializeField]
        private string _stepTextPrefix = "Step";

        [SerializeField]
        private string _stepTextConjuction = "of";

        private int _currentStep = 0;

        private void OnEnable()
        {
            if (_steps.Count > 0)
            {
                _steps[_currentStep].SetActive(true);
            }
        }

        public void NextStep()
        {
            if (_currentStep < _steps.Count - 1)
            {
                _steps[_currentStep].SetActive(false);
                _currentStep++;
                _steps[_currentStep].SetActive(true);
            }

            UpdateStepText();
        }

        public void PreviousStep()
        {
            if (_currentStep > 0)
            {
                _steps[_currentStep].SetActive(false);
                _currentStep--;
                _steps[_currentStep].SetActive(true);
            }

            UpdateStepText();
        }

        public void UpdateStepText()
        {
            _stepText.text = _stepTextPrefix + " " + (_currentStep + 1) + " " + _stepTextConjuction + " " + _steps.Count;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        void OnValidate()
        {
            UpdateStepText();
        }
    }
}
