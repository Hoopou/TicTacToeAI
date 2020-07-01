using System;
using System.Linq;
using TicTacToeEngine;

namespace TicTacToeAI
{
    class Program
    {
        static void Main(string[] args)
        {

            int Iteration = 0; // Current Training Iteration

            NeuralNetwork BestNetwork = new NeuralNetwork(new uint[] { 18, 18, 9, 9 }); // The best network currently made
            double BestCost = double.MaxValue; // The cost that the best network achieved
            double[] BestNetworkResults = new double[4]; // The results that the best network calculated


            while (true) // Keep Training forever
            {
                NeuralNetwork Player1 = new NeuralNetwork(BestNetwork); // Clone the current best network
                Player1.Mutate(); // Mutate the clone
                double MutatedNetworkCost = 0;
                double[] CurrentNetworkResults = new double[4]; // The results that the mutated network calculated

                TicTacToeEngine.TicTacToe game = new TicTacToeEngine.TicTacToe(3);

                // Calculate the cost of the mutated network

                double[] Result = Player1.FeedForward(game.GetGridAsSingleTable(Players.Player1));

                // Does the mutated network perform better than the last one
                if (MutatedNetworkCost < BestCost)
                {
                    BestNetwork = Player1;
                    BestCost = MutatedNetworkCost;
                    BestNetworkResults = CurrentNetworkResults;
                }

                // Print only each 20000 iteration in order to speed up the training process
                if (Iteration % 20000 == 0)
                {
                    Console.Clear(); // Clear the current console text

                    for (int i = 0; i < BestNetworkResults.Length; i++) // Print the best truth table
                    {
                        //Console.WriteLine(Inputs[i][0] + "," + Inputs[i][1] + " | " + BestNetworkResults[i].ToString("N17"));
                    }
                    Console.WriteLine("Cost: " + BestCost); // Print the best cost
                    Console.WriteLine("Iteration: " + Iteration); // Print the current Iteration
                }

                // An iteration is done
                Iteration++;

                //    TicTacToeEngine.TicTacToe tic = new TicTacToeEngine.TicTacToe(3);
                //tic.PlayMove(Players.Player1, 0);
                //tic.PlayMove(Players.Player2, 3);
                //var output = tic.GetGridAsSingleTable(Players.Player1);

                //Console.WriteLine(output.ToString());

                Console.WriteLine("Pressa any key to continue!");
                Console.ReadLine();
            }
        }
    }
}

