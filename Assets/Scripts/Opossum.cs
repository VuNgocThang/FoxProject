using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : Enemy
{

    public List<Transform> points;
    public int nextID = 0;
    public int idChangeValue = 1;
    public float speed = 2;

    protected override void Start()
    {
        base.Start();
    }


    private void Update()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        // nhận điểm đích
        Transform goalPoint = points[nextID];
        //flip 
        if (goalPoint.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);    
        // move to the goalpoint
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);
        // check khoảng cách để trigger kích hoạt điểm tiếp
        if (Vector2.Distance(transform.position, goalPoint.position) < 0.2f)
        {
            if (nextID == points.Count - 1)
            {
                idChangeValue = -1;
            }
            if (nextID == 0)
            {
                idChangeValue = 1;
            }
          
            nextID += idChangeValue;
        }
    }
}
