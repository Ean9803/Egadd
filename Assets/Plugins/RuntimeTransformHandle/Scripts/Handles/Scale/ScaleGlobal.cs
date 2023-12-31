using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RuntimeHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class ScaleGlobal : HandleBase
    {
        protected Vector3 _axis;
        protected Vector3 _startScale;
        
        public ScaleGlobal Initialize(RuntimeTransformHandle p_parentTransformHandle, Vector3 p_axis, Color p_color)
        {
            _parentTransformHandle = p_parentTransformHandle;
            _axis = p_axis;
            _defaultColor = p_color;
            
            InitializeMaterial();

            transform.SetParent(p_parentTransformHandle.transform, false);

            GameObject o = new GameObject();
            o.transform.SetParent(transform, false);
            MeshRenderer mr = o.AddComponent<MeshRenderer>();
            mr.material = _material;
            MeshFilter mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateBox(.35f, .35f, .35f);
            MeshCollider mc = o.AddComponent<MeshCollider>();

            return this;
        }

        public override void Interact(Vector3 p_previousPosition, Camera Cam)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Cam.nearClipPlane;
            Vector3 mouseVector = (mousePos - p_previousPosition);
            float d = (mouseVector.x + mouseVector.y) * Time.deltaTime * 2;
            delta += d;
            _parentTransformHandle.target.localScale = _startScale + Vector3.Scale(_startScale,_axis) * delta;
            
            base.Interact(p_previousPosition, Cam);
        }

        public override void StartInteraction(Vector3 p_hitPoint, Camera Cam)
        {
            base.StartInteraction(p_hitPoint, Cam);
            _startScale = _parentTransformHandle.target.localScale;
        }
    }
}