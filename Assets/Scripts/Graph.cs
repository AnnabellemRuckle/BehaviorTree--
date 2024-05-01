using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<Connection> mConnections;

    // an array of connections outgoing from the given node
    public List<Connection> getConnections(Node fromNode)
    {
        List<Connection> connections = new List<Connection>();
        foreach (Connection c in mConnections)
        {
            if (c.getFromNode() == fromNode)
            {
                connections.Add(c);
            }
        }
        return connections;
    }

    public void Build()
    {
        // find all nodes in scene
        // iterate over the nodes
        //   create connection objects,
        //   stuff them in mConnections
        mConnections = new List<Connection>();

        Node[] nodes = GameObject.FindObjectsOfType<Node>();
        foreach (Node fromNode in nodes)
        {
            foreach (Node toNode in fromNode.ConnectsTo)
            {
                float cost = (toNode.transform.position - fromNode.transform.position).sqrMagnitude;

                // Get the material of the "toNode" object
                Material material = toNode.GetComponent<Renderer>().material;

                Connection c = new Connection(cost, fromNode, toNode, material);
                mConnections.Add(c);
            }
        }
    }
}

public class Connection
{
    float cost;
    Node fromNode;
    Node toNode;
    Material material;

    public Connection(float cost, Node fromNode, Node toNode, Material material)
    {
        this.cost = cost;
        this.fromNode = fromNode;
        this.toNode = toNode;
        this.material = material;
    }

    public float getCost()
    {
        return cost;
    }

    public Node getFromNode()
    {
        return fromNode;
    }

    public Node getToNode()
    {
        return toNode;
    }

    public Material getMaterial()
    {
        return material;
    }
}
