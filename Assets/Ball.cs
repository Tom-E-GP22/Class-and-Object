using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Ball : ProcessingLite.GP21
{
  //Our class variables
  public Vector2 position; //Ball position
  public Vector2 velocity; //Ball direction

  public float rndSize;
  int r, g, b;

  //Ball Constructor, called when we type new Ball(x, y);
  public Ball(float posX, float posY, float size)
  {
    position = new Vector2(posX,posY);
    rndSize = size;
    //Create the velocity vector and give it a random direction.
    velocity = new Vector2();
    velocity.x = Random.Range(0, 11) - 2.5f;
    velocity.y = Random.Range(0, 11) - 2.5f;
    RandomColor();
  }

  //Draw our ball
  public void Draw()
  {
    Fill(0);
    Stroke(r,g,b,255);
    Circle(position.x, position.y, rndSize);
  }

  //Update our ball
  public void UpdatePos()
  {
    position += velocity * Time.deltaTime;
  }

  public void RandomColor()
  {
    r = Random.Range(0, 255);
    g = Random.Range(0, 255);
    b = Random.Range(0, 255);
  }
}
//}