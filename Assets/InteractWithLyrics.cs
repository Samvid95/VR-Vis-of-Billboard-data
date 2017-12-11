using System.Collections;
using System.Collections.Generic;

namespace VRTK.Examples
{
    using UnityEngine;

    public class InteractWithLyrics : VRTK_InteractableObject
    {
        float spinSpeed = 0f;
        private GameObject WordLink;
        //Transform rotator;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            Debug.Log("I have pointed to the word: " + gameObject.name);
            WordLink.GetComponent<WordLink>().selectedWord = gameObject.name;
            spinSpeed = 0f;
        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            spinSpeed = 0f;
        }

        protected void Start()
        {
            WordLink = GameObject.Find("WordLink");
        }

        protected override void Update()
        {
            base.Update();
            // transform.Rotate(new Vector3(spinSpeed * Time.deltaTime, 0f, 0f));
        }
    }
}
