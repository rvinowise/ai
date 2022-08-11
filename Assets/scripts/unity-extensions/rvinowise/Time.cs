namespace rvinowise {

using unity = UnityEngine;

public class Time: unity::Time
{
    public static new float deltaTime  { 
        get {
            //return unity::Time.deltaTime / unity.Time.fixedDeltaTime;
            return unity::Time.deltaTime;
        }
    }
}

}