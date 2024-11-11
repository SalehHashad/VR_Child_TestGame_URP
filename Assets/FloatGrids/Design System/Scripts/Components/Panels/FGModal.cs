using UnityEngine;

namespace floatgrids { 
    public class FGModal : FGPanelPosition
    {   
        [SerializeField] 
        private float _startMovingAngle = 30f;
        private float _updateMovingAngle = 0f;
        private float _stopMovingAngle = 10f;
        private float _breakAngle = 15f;
        private float _resistance = .02f;
        private float _maxSpeed = 10f;
        private float _speedDecrease = 0.6f;

        private Vector3 _speed;
        Vector3 _targetPosition;
        private bool _isMoving = false;

        public new void Awake()
        {
            base.Awake();
            
            _speed = Vector3.zero;
            _targetPosition = this.transform.forward;
        }

        protected override void Update()
        {
            base.Update();

            if (Camera.main != null)
            {
                Transform cameraTransform = Camera.main.transform;
                float angle = Vector3.Angle(cameraTransform.forward, this.transform.forward);

                if (!_isMoving && angle > _startMovingAngle || _isMoving && angle > _updateMovingAngle)
                {
                    _isMoving = true;
                    Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);
                    cameraForward.Normalize();
                    _targetPosition = cameraTransform.position + cameraForward * _distanceToChild;
                }

                Vector3 forward = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
                Vector3 targetForward = new Vector3(_targetPosition.x - cameraTransform.position.x, 0, _targetPosition.z - cameraTransform.position.z);
                float angleToTarget = Vector3.Angle(forward, targetForward);

                if (_isMoving)
                {
                    MoveTowardsUser(_targetPosition, angleToTarget);

                    if (angleToTarget < _stopMovingAngle)
                    {
                        _isMoving = false;
                    }
                }
            }
        }

        private void MoveTowardsUser(Vector3 targetPosition, float angleToTarget)
        {
            Vector3 directionMovement = (targetPosition - this.transform.position);
            directionMovement.Scale(new Vector3(1, 0, 1));
            directionMovement.Normalize();
            Vector3 acceleration = Vector3.zero;

            float distance = Vector3.Distance(this.transform.position, targetPosition); // used as a linear decreasing parameter for the acceleration
            distance = Mathf.Clamp(distance, 1f, 10f);

            if (angleToTarget < _breakAngle)
            {
                _speed *= _speedDecrease;
            }

            _speed *= _speedDecrease;
            acceleration = directionMovement * angleToTarget * distance * _resistance;
            _speed += acceleration;

            if (_speed.magnitude > _maxSpeed)
            {
                _speed = _speed.normalized * _maxSpeed;
            }

            this.transform.position += _speed * Time.deltaTime;
            
            SetRotation(Camera.main.transform.position.y - _distanceToChild * Mathf.Tan(_childHeight * Mathf.Deg2Rad));    
        }

        protected override void SetUITransform()
        {
            float uiHeight = Camera.main.transform.position.y - _distanceToChild * Mathf.Tan(_childHeight * Mathf.Deg2Rad);
            SetUITransform(uiHeight);
        }

        public void Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}