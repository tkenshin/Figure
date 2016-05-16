
#3点を含む平面の式

![3点を含む平面の式](http://i.imgur.com/nneJVqN.gif)

    Vector3 A = cutPointArray[0]; // 切断点(1) 
    Vector3 B = cutPointArray[1]; // 切断点(2) 
    Vector3 C = cutPointArray[2]; // 切断点(3) 

    Vector3 AB = new Vector3(B.x - A.x, B.y - A.y, B.z - A.z); // ABベクトル
    Vector3 AC = new Vector3(C.x - A.x, C.y - A.y, C.z - A.z); // ACベクトル

    float a = (B.y - A.y) * (C.z - A.z) - (C.y - A.y) * (B.z - A.z);
    float b = (B.z - A.z) * (C.x - A.x) - (C.z - A.z) * (B.x - A.x);
    float c = (B.x - A.x) * (C.y - A.y) - (C.x - A.x) * (B.y - A.y);
    
    float d = -(a * A.x + b * A.y + c * A.z);
    
    Vector3 ABxAC = new Vector3(a, b, c); // ABベクトル x ACベクトル
    
    Debug.Log("a = " + a);
    Debug.Log("b = " + b);
    Debug.Log("c = " + c);
    
    Debug.Log("d = " + d);
    
    Debug.Log("Q = " + (a + b + c + d));

![img](http://i.imgur.com/ZfU01xh.png) ![img](http://i.imgur.com/tOgi4li.png)
