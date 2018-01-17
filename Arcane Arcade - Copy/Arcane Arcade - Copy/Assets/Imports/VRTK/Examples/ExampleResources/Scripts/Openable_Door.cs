﻿namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    public class Openable_Door : VRTK_InteractableObject
    {
        public bool flipped = false;
        public bool rotated = false;
        private string toScene="Duel";

        private float sideFlip = -1;
        private float side = -1;
        private float smooth = 270.0f;
        private float doorOpenAngle = -90f;
        private bool open = false;

        private Vector3 defaultRotation;
        private Vector3 openRotation;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            SetDoorRotation(usingObject.transform.position);
            SetRotation();
            open = !open;
            if(open){
              Invoke("LoadLevel", 1.0f);
            }
        }
        private void LoadLevel()
        {
            SceneManager.LoadScene(toScene);
        }

        protected void Start()
        {
            defaultRotation = transform.eulerAngles;
            SetRotation();
            sideFlip = (flipped ? 1 : -1);
        }

        protected override void Update()
        {
            base.Update();
            if (open)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(openRotation), Time.deltaTime * smooth);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(defaultRotation), Time.deltaTime * smooth);
            }
        }

        private void SetRotation()
        {
            openRotation = new Vector3(defaultRotation.x, defaultRotation.y + (doorOpenAngle * (sideFlip * side)), defaultRotation.z);
        }

        private void SetDoorRotation(Vector3 interacterPosition)
        {
            side = ((rotated == false && interacterPosition.z > transform.position.z) || (rotated == true && interacterPosition.x > transform.position.x) ? -1 : 1);
        }
    }
}