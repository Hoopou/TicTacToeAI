using System;

namespace NeuralXOR
{
    class Program
    {
        static void Main(string[] args)
        {
            int Iteration = 0; // Current Training Iteration

            NeuralNetwork BestNetwork = new NeuralNetwork(new uint[] { 2, 2, 1 }); // The best network currently made
            double BestCost = double.MaxValue; // The cost that the best network achieved
            double[] BestNetworkResults = new double[4]; // The results that the best network calculated

            double[][] Inputs = new double[][] // This represents the possible inputs or the training dataset
            {
                    new double[] { 0, 0 },
                    new double[] { 0, 1 },
                    new double[] { 1, 0 },
                    new double[] { 1, 1 }
            };
            double[] ExpectedOutputs = new double[] { 0, 1, 1, 0 }; // This represents the expected outputs from the optimum NeuralNetwork

            while (true) // Keep Training forever
            {
                NeuralNetwork MutatedNetwork = new NeuralNetwork(BestNetwork); // Clone the current best network
                MutatedNetwork.Mutate(); // Mutate the clone
                double MutatedNetworkCost = 0;
                double[] CurrentNetworkResults = new double[4]; // The results that the mutated network calculated

                // Calculate the cost of the mutated network
                for (int i = 0; i < Inputs.Length; i++)
                {
                    double[] Result = MutatedNetwork.FeedForward(Inputs[i]);
                    MutatedNetworkCost += Math.Abs(Result[0] - ExpectedOutputs[i]);

                    CurrentNetworkResults[i] = Result[0];
                }

                // Does the mutated network perform better than the last one
                if (MutatedNetworkCost < BestCost)
                {
                    BestNetwork = MutatedNetwork;
                    BestCost = MutatedNetworkCost;
                    BestNetworkResults = CurrentNetworkResults;
                }

                // Print only each 20000 iteration in order to speed up the training process
                if (Iteration % 20000 == 0)
                {
                    Console.Clear(); // Clear the current console text

                    for (int i = 0; i < BestNetworkResults.Length; i++) // Print the best truth table
                    {
                        Console.WriteLine(Inputs[i][0] + "," + Inputs[i][1] + " | " + BestNetworkResults[i].ToString("N17"));
                    }
                    Console.WriteLine("Cost: " + BestCost); // Print the best cost
                    Console.WriteLine("Iteration: " + Iteration); // Print the current Iteration
                }

                // An iteration is done
                Iteration++;
            }
        }
    }
}