using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerBall : ProcessingLite.GP21
{
    public List<Ball> myBalls = new List<Ball>();
    bool wait = false;
    bool gameOver = false;
    bool invincible = false;
    float x1,y1,size;
    float spawnDelay = 5;
    float ballLimit = 100;
    int frameCounter=0;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    frameCounter++;
    StartCoroutine(AddBalls());

    if(myBalls.Count < 1000)
    {
        foreach(Ball m_ball in myBalls)
        {
            m_ball.UpdatePos();
            m_ball.Draw();
        if(m_ball.position.x+(m_ball.rndSize/2) > Width || m_ball.position.x-(m_ball.rndSize/2) < 0)
            m_ball.velocity = new Vector3(-m_ball.velocity.x, m_ball.velocity.y);

        if(m_ball.position.y+(m_ball.rndSize/2) > Height || m_ball.position.y-(m_ball.rndSize/2) < 0)
            m_ball.velocity = new Vector3(m_ball.velocity.x, -m_ball.velocity.y);
        }
        if(frameCounter%5 == 0)
        {
            frameCounter=0;
            StartCoroutine(collidePlayerCheck());
        }
    }
    else
        Background(0);

  }
  
  IEnumerator AddBalls()
  {
    Debug.Log(myBalls.Count);
    if(myBalls.Count < ballLimit && !wait)
    {
        wait = true;
        retry:
        if(Instansiate())
            myBalls.Add(new Ball(x1,y1,size));
        else if(gameOver)
            myBalls.Add(new Ball(x1,y1,3));
        else
            goto retry;
        yield return new WaitForSeconds(spawnDelay);
        wait = false;
    }
  }

  bool Instansiate()
  {
    x1 = Random.Range(0,Width);
    y1 = Random.Range(0,Height);
    size = Random.Range(0.5f, 2f);
    foreach(Ball m_ball in myBalls)
    {
        float x2 = m_ball.position.x;
        float y2 = m_ball.position.y;
        float maxDist = size/2 + m_ball.rndSize/2;

        //first a quick check to see if we are too far away in x or y direction
        //if we are far away we don't collide so just return false and be done.
        if (Mathf.Abs(x1 - x2) > maxDist || Mathf.Abs(y1 - y2) > maxDist)
        {
            return true;
        }
        //we then run the slower distance calculation
        //Distance uses Pythagoras to get exact distance, if we still are to far away we are not colliding.
        else if (Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2)) > maxDist)
        {
            return true;
        }
        //We now know the points are closer then the distance so we are colliding!
        else
        {
            return false;
        }
    }
    return true;
  }

  IEnumerator collidePlayerCheck()
  {
    foreach(Ball m_ball in myBalls)
    {
        var player = GameObject.Find("Main Camera").GetComponent<AccelerationMovement>();

        float x1 = m_ball.position.x;
        float y1 = m_ball.position.y;
        float maxDist = size/2 + m_ball.rndSize/2;
        //first a quick check to see if we are too far away in x or y direction
        //if we are far away we don't collide so just return false and be done.
        if (Mathf.Abs(x1 - player.posX) > maxDist || Mathf.Abs(y1 - player.posY) > maxDist){}
        //we then run the slower distance calculation
        //Distance uses Pythagoras to get exact distance, if we still are to far away we are not colliding.
        else if (Vector2.Distance(new Vector2(x1, y1), new Vector2(player.posX, player.posY)) > maxDist){}
        //We now know the points are closer then the distance so we are colliding!
        else if(!invincible)
        {
            GameObject.Find("heart"+player.hp).SetActive(false);
            player.hp--;
            if(player.hp <= 0)
            {
                spawnDelay=0;
                ballLimit=1000;
                gameOver=true;
            }
            invincible=true;
            yield return new WaitForSeconds(2.5f);
            invincible=false;
        }
    }
  }
}
