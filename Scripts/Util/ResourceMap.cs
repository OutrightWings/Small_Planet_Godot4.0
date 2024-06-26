using System;
using System.Collections.Generic;
using Godot;

public partial class ResourceMap : Node
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

    public override string ToString()
    {
        string str = "";
        foreach(var pair in resourceMap){
            str += $"{pair.Key}: {pair.Value}\n";
        }
        return str;
    }
}