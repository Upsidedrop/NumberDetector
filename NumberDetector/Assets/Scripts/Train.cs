using System.IO;
using UnityEngine;

public class Train : MonoBehaviour
{
    Node[][] nodes;
    float[] inputs = new float[784];
    readonly int nodesPerLayer = 10;
    int dataIndex = 0;
    Image image;
    float[] changes;
    float[] nextChanges;
    readonly float changeSensitivity = 0.1f;
    private void Awake()
    {
        CreateNodes();
    }
    class Node
    {
        public float bias;
        public float[] inputWeights;
        public float value;
    }
    void CreateNodes()
    {
        nodes = new Node[6][];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Node[nodesPerLayer];
            for (int i1 = 0; i1 < nodesPerLayer; i1++)
            {
                nodes[i][i1] = new Node();

                nodes[i][i1].bias = 0;
                nodes[i][i1].value = 0;

                if (i == 0)
                {
                    nodes[i][i1].inputWeights = new float[inputs.Length];
                }
                else
                {
                    nodes[i][i1].inputWeights = new float[nodesPerLayer];
                }
            }
        }
    }
    float Sum(float[] arguments)
    {
        if (arguments.Length == 0)
        {
            return 0;
        }

        // Initialize a variable to hold the sum
        float sum = 0;

        // Loop through each element in the array
        foreach (float argument in arguments)
        {
            // Add the current element to the sum
            sum += argument;
        }

        // Return the sum of the elements in the array
        return sum;
    }
    float CalculateNodes(float[] values, float[] weights, float bias)
    {
        float[] alteredValues = new float[values.Length];
        float result;
        for (int i = 0; i < values.Length; i++)
        {
            alteredValues[i] = values[i] * weights[i];
        }
        result = Sum(alteredValues) + bias;
        return LogSigmoid(result);
    }
    void AssignNodes()
    {
        // Initialize two arrays of floating point numbers to hold the values of nodes and the values of nodes from the previous iteration
        float[] values = new float[nodesPerLayer];
        float[] lastValues = new float[nodesPerLayer];

        // Loop through each layer of nodes in the network
        for (int i = 0; i < nodes.Length; i++)
        {
            // Loop through each individual node in the current layer
            for (int r = 0; r < nodesPerLayer; r++)
            {
                // Store the current value of the node in an array
                values[r] = nodes[i][r].value;

                // If the current layer is not the input layer, calculate the value of the current node based on the previous layer's values
                if (i != 0)
                {
                    nodes[i][r].value = CalculateNodes(
                        lastValues,
                        nodes[i][r].inputWeights,
                        nodes[i][r].bias);
                }
                // If the current layer is the input layer, calculate the value of the current node based on the input values
                else
                {
                    nodes[i][r].value = CalculateNodes(
                        inputs,
                        nodes[i][r].inputWeights,
                        nodes[i][r].bias);
                }
            }

            // Copy the values of the current layer's nodes to the lastValues array to be used in the next iteration
            for (int i1 = 0; i1 < values.Length; i1++)
            {
                float item = values[i1];
                lastValues[i1] = item;
            }

        }
    }
    void AssignInputs()
    {

        image = JsonUtility.FromJson<JsonableListWrapper<Image>>(File.ReadAllText(Application.dataPath + "/saveFile.json"))
                           .list[dataIndex];
        for (int i = 0; i < inputs.Length; i++)
        {
            if (image.imgBool[i])
            {
                inputs[i] = 1;
            }
            else
            {
                inputs[i] = 0;
            }
        }
    }
    void BackPropogation()
    {
        changes = new float[nodesPerLayer];
        nextChanges = new float[nodesPerLayer];
        for (int i = 0; i < nodesPerLayer; i++)
        {
            if (image.correctOutput == i)
            {
                changes[i] = 1 - nodes[5][i].value;
            }
            else
            {
                changes[i] = 0 - nodes[5][i].value;
            }
        }
        print(CalculateCost());
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int i1 = 0; i1 < nodesPerLayer; i1++)
            {
                ChangeActivations(changes[i1], 5 - i, i1);
            }
            for (int i2 = 0; i2 < nodesPerLayer; i2++)
            {
                changes[i2] = nextChanges[i2];
                nextChanges[i2] = 0;
            }
        }

    }
    void ChangeActivations(float change, int nodeLayer, int nodeIndex)
    {
        float averageChange = change * changeSensitivity;
        nodes[nodeLayer][nodeIndex].bias += averageChange;
        if (nodeLayer == 0)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                nodes[nodeLayer][nodeIndex].inputWeights[i] = inputs[i] * averageChange;
            }

        }
        else
        {
            for (int i = 0; i < nodesPerLayer; i++)
            {
                nodes[nodeLayer][nodeIndex].inputWeights[i] = nodes[nodeLayer - 1][i].value * averageChange;
                nextChanges[i] += nodes[nodeLayer][nodeIndex].inputWeights[i] * averageChange;
            }

        }

    }
    float LogSigmoid(float x)
    {
        if (x < -45.0) return 0.0f;
        else if (x > 45.0) return 1.0f;
        else return 1.0f / (1.0f + Mathf.Exp(-x));
    }
    public void CallMethods()
    {

        AssignInputs();
        AssignNodes();
        BackPropogation();
    }
    float CalculateCost()
    {
        float cost = 0;
        for (int i = 0; i < nodesPerLayer; i++)
        {
            cost += Mathf.Pow(nodes[5][i].value - changes[i], 2);
        }
        return cost;
    }

}

