using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WasaaMP
{
    public class Collision : MonoBehaviour
    {

        bool above = false;
        bool below = false;
        int id;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider hit)
        {
            checkPosition(hit);
        }

        void OnTriggerExit(Collider hit)
        {

            //checkPosition(hit);
            var collided = hit.GetComponent<Collision>();
            if (collided != null)
            {
                collided.setAbove(false);
            }

        }


        private void checkPosition(Collider hit)
        {
            int this_id;
            int hit_id;

            char char1 = gameObject.name[gameObject.name.Length - 1];
            char char2 = hit.name[hit.name.Length - 1];

            string str1 = new string(char1, 1);
            string str2 = new string(char2, 1);

            //GameObject go = gameObject.transform.parent.gameObject;
            Renderer renderer = gameObject.GetComponent<Renderer>();

            if (Int32.TryParse(str1, out this_id) && Int32.TryParse(str2, out hit_id))
            {

                if (this_id > hit_id)
                {
                    if (transform.position.y < hit.transform.position.y)
                    {
                        above = true;
                        print("theres an object above " + gameObject.name);
                        if (renderer != null)
                        {
                            renderer.material.color = Color.blue;
                        }
                    }
                    else
                    {
                        above = false;
                        print("theres not an object above " + gameObject.name);
                    }
  
                }
                else if (this_id < hit_id)
                {
                    if (transform.position.y > hit.transform.position.y)
                    {
                        below = true;
                        print("theres an object below " + gameObject.name);
                        if (renderer != null)
                        {
                            renderer.material.color = Color.blue;
                        }
                    }
                    else
                    {
                        below = false;
                        print("theres not an object above " + gameObject.name);
                    }

                }
                else
                {
                    print("false");
                    if (renderer != null)
                    {
                        renderer.material.color = Color.red;

                    }
                }
            }
        }

        /*private bool validate(Collider hit)
        {
            if (Int32.TryParse(gameObject.name[gameObject.name.Length - 1], out this_id) && Int32.TryParse(hit.name[hit.name.Length - 1], out hit_id))
            {
                // continue
                if (this_id > hit_id && transform.position.y > hit.position.y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }*/

        public bool getAbove()
        {
            return above;
        }

        public void setAbove(bool value)
        {
            above = value;

        }

        public bool getBelow()
        {
            return below;
        }

        public void setBelow(bool value)
        {
            below = value;

        }
    }

   
}
