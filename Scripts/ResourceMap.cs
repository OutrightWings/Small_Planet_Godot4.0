using System.Collections.Generic;

public class ResourceMap
{
    private Dictionary<string,float> resourceMap = new();
    public void AddToResource(string name, float amount){
        if(resourceMap.ContainsKey(name)){
            resourceMap[name] += amount;
        } else {
            AddResource(name, amount);
        }
    }
    public void SetResource(string name, float amount){
        if(resourceMap.ContainsKey(name)){
            resourceMap[name] = amount;
        } else {
            AddResource(name, amount);
        }
    }
    public float GetResourceAmount(string name){
        return resourceMap[name];
    }

    private void AddResource(string name, float amount)
    {
        resourceMap.Add(name,amount);
    }
}