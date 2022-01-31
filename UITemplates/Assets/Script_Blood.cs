using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Blood : MonoBehaviour
{
    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;
    Texture2D texture;
    Renderer renderer;
    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        texture = new Texture2D(128, 128);
        
    }
    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag != "Blood")
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

            int i = 0;
            renderer = other.GetComponent<Renderer>();
            renderer.material.mainTexture = texture;
            while (i < numCollisionEvents)
            {
                Color color = (((int)collisionEvents[i].intersection.y & (int)collisionEvents[i].intersection.x) != 0 ? Color.red : Color.red);
                texture.SetPixel((int)collisionEvents[i].intersection.x, (int)collisionEvents[i].intersection.y, color);
                texture.Apply();
                i++;
            }



            
        }
        
    }
}
