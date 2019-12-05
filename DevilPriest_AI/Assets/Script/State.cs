using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class State{
    public int priest;
    public int devil;
    public bool boat;
    public State parent;
    public State best_way;
    
    public State() {}
    public State(int p, int d, bool b) {
        this.priest = p;
        this.devil = d;
        this.boat = b;
    }

    public State(int p, int d, bool b, State par) {
        this.priest = p;
        this.devil = d;
        this.boat = b;
        this.parent = par;
    }
    public State(State copy) {
        this.priest = copy.priest;
        this.devil = copy.devil;
        this.boat = copy.boat;
        this.parent = copy.parent;
        this.best_way = copy.best_way;
    }

    public bool isEqual(State compare) {
        return this.priest==compare.priest && this.devil==compare.devil && this.boat == compare.boat;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        State tmp = (State)obj;
        return this.priest==tmp.priest && this.devil==tmp.devil && this.boat == tmp.boat;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
    }

    public override String ToString() {
        if (best_way == null) {
            return "priest: " + priest.ToString() + " devil: " + devil.ToString() + " boat: " + boat.ToString() + 
        "\nNext: " + "NULL";
        }
        return "priest: " + priest.ToString() + " devil: " + devil.ToString() + " boat: " + boat.ToString() + 
        "\nNext: " + best_way.priest.ToString() + " " + best_way.devil.ToString() + " " + best_way.boat.ToString();
    }
}