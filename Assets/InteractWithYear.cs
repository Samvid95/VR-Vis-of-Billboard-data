using System.Collections;
using System.Collections.Generic;

namespace VRTK.Examples
{
    using UnityEngine;

    public class InteractWithYear : VRTK_InteractableObject
    {
        float spinSpeed = 0f;
        private List<GameObject> children;
        //Transform rotator;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            this.GetComponent<LyricSpawner>().stopRotation = true;
            spinSpeed = 0f;
        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            this.GetComponent<LyricSpawner>().stopRotation = false;
            spinSpeed = 0f;
        }

        protected void Start()
        {
           // rotator = transform.Find("Capsule");
        }

        protected override void Update()
        {
            base.Update();
           // transform.Rotate(new Vector3(spinSpeed * Time.deltaTime, 0f, 0f));
        }
    }
}
