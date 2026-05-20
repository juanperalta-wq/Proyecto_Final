using UnityEngine;
using DulceSueño.Collections.Graphs;
public class GameManagerGraphs : MonoBehaviour
{
    public NonOrientedGraph<string> graph = new();
    void Start()
    {
        Node<string> A = graph.AddNode("A");
        Node<string> B = graph.AddNode("B");
        Node<string> C = graph.AddNode("C");
        Node<string> D = graph.AddNode("D");

        graph.AddEdges(A, B);
        graph.AddEdges(B, C);
        graph.AddEdges(C, D);

        graph.PrintAdjancencyList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
