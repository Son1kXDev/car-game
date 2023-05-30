using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class BridgeContainer : MonoBehaviour, ICantExistInPlayMode
    {
        public bool IsBreakable
        {
            get { return _isBreakable; }
            set { _isBreakable = value; UpdateBrakeVariables(); }
        }

        public float BrakeForce
        {
            get { return _brakeForce; }
            set { _brakeForce = value; UpdateBrakeVariables(); }
        }

        public float BrakeTorque
        {
            get { return _brakeTorque; }
            set { _brakeTorque = value; UpdateBrakeVariables(); }
        }

        public float Count
        { get { return _bridge.Count; } }

        private GameObject _stake;
        private GameObject _plank;
        private GameObject _endStake;

        private float _brakeForce = Mathf.Infinity;
        private float _brakeTorque = Mathf.Infinity;
        private bool _isBreakable = false;

        private List<GameObject> _bridge;

        private void UpdateBrakeVariables()
        {
            if (IsBreakable)
            {
                _bridge.ForEach(plank => { if (plank.name == "Plank") plank.GetComponent<HingeJoint2D>().breakForce = BrakeForce; });
                _bridge.ForEach(plank => { if (plank.name == "Plank") plank.GetComponent<HingeJoint2D>().breakTorque = BrakeTorque; });
            }
            else
            {
                _bridge.ForEach(plank => { if (plank.name == "Plank") plank.GetComponent<HingeJoint2D>().breakForce = Mathf.Infinity; });
                _bridge.ForEach(plank => { if (plank.name == "Plank") plank.GetComponent<HingeJoint2D>().breakTorque = Mathf.Infinity; });
            }
        }

        public void SetData(GameObject stake, GameObject plank, GameObject endStake)
        {
            _stake = stake;
            _plank = plank;
            _endStake = endStake;
        }

        public void Create()
        {
            if (_bridge != null) _bridge.Clear();
            else _bridge = new List<GameObject>();

            transform.gameObject.name = "Bridge (In Process)";
            _bridge.Add(Instantiate(_stake, transform));
            _bridge[0].name = "Stake";
            _bridge.Add(Instantiate(_plank, new Vector2(0.325f, -0.3f), Quaternion.identity, transform));
            _bridge[1].name = "Plank";
            _bridge[1].transform.Find("HorizontalLine").GetComponent<LineRenderer>().SetPosition(0, new(-0.375f, 0, 0));
            SetConnectedRigidBody();
        }

        public void AddPlank()
        {
            GameObject plank = Instantiate(_plank, Vector2.zero, Quaternion.identity, transform);
            _bridge.Add(plank);
            _bridge[^1].transform.localPosition = NewPosition();
            _bridge[^1].transform.rotation = ObjectQuaternion();
            SetConnectedRigidBody();
            _bridge[^1].name = "Plank";
            if (IsBreakable)
            {
                _bridge[^1].GetComponent<HingeJoint2D>().breakForce = BrakeForce;
                _bridge[^1].GetComponent<HingeJoint2D>().breakTorque = BrakeTorque;
            }
        }

        public void RemovePlank()
        {
            DestroyImmediate(_bridge[^1]);
            _bridge.Remove(_bridge[^1]);
        }

        public void Finish()
        {
            DestroyImmediate(_bridge[^1].transform.Find("VerticalLine").gameObject);
            _bridge[^1].transform.Find("HorizontalLine").GetComponent<LineRenderer>().SetPosition(1, new(0.375f, 0, 0));
            GameObject stake = Instantiate(_endStake, Vector2.zero, Quaternion.identity, transform);
            _bridge.Add(stake);
            _bridge[^1].transform.localPosition = NewPosition() + new Vector2(-0.25f, 0.33f);
            _bridge[^1].transform.rotation = ObjectQuaternion();
            SetConnectedRigidBody();
            _bridge[^1].name = "Stake";
            transform.gameObject.name = "Bridge";
            DestroyImmediate(this);
        }

        private Quaternion ObjectQuaternion()
        {
            return _bridge[0].transform.parent.rotation;
        }

        private Vector2 NewPosition()
        {
            SpriteRenderer spriteRenderer = _bridge[^2].GetComponent<SpriteRenderer>();
            float width = spriteRenderer.sprite.bounds.size.x;
            return (Vector2)_bridge[^2].transform.localPosition + new Vector2(width, 0);
        }

        private void SetConnectedRigidBody()
        {
            Rigidbody2D connectedRidigbody = _bridge[^2].GetComponent<Rigidbody2D>();
            HingeJoint2D hingeJoint = _bridge[^1].GetComponent<HingeJoint2D>();
            hingeJoint.connectedBody = connectedRidigbody;
        }
    }
}