using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using TicTacToeEngine;

namespace TicTacToeAI
{
    class Program
    {

        private static readonly int GRID_SIZE = 3;

        private static readonly string OUTPUT_DIRECTORY = Directory.GetCurrentDirectory() + "\\output";

        static void Main(string[] args)
        {

            int Iteration = 0; // Current Training Iteration

            NeuralNetwork BestNetwork = new NeuralNetwork(new uint[] { 18, 9, 9 }); // The best network currently made
            var BestNetworkErrorCount = int.MaxValue;




            while (true) // Keep Training forever
            {
                /*
                 *      
                 *      1: Initialiser le joueur1 et le joueur 2 avec le best AI
                 *      2: mutate player 1 et player 2
                 *      3: Initialiser partie
                 *      
                 *      Tant que la partie n'est pas terminer 
                 *              Tant que le joueur 1 n'a pas jouer un moove légal
                 *                      Player1 jouer moove
                 *              Fin de Tant que
                 *              
                 *              Tant que le joueur 2 n'a pas jouer un moove légal
                 *                      Player2 jouer moove
                 *              Fin de Tant que
                 *      Fin tant que
                 *      
                 *      Si un joueur a gagner : mettre bestNetwork a ce joueur
                 *      
                 *      
                 * 
                 * 
                 * 
                 * 
                 */

                var AIplayer1 = new NeuralNetwork(BestNetwork);
                var AIplayer2 = new NeuralNetwork(BestNetwork);

                var game = new TicTacToe(GRID_SIZE);
                var errorCount = 0;
                var errorCountPlayer2 = 0;

                //AIplayer1.Mutate();
                AIplayer2.Mutate();

                while (!game.IsGameFinished())
                {

                    //For player 1
                    var IsLegalMoove = false;
                    var AiOutput = new double[GRID_SIZE* GRID_SIZE];

                    do
                    {
                        try
                        {
                            AiOutput = AIplayer1.FeedForward(game.GetGridAsSingleTable(Players.Player1));
                            game.PlayMove(Players.Player1, GetPositionFromAiOutput(AiOutput));
                            IsLegalMoove = true;
                        }
                        catch (Exception)
                        {
                            errorCount++;
                            AIplayer1.Mutate();
                        }
                    } while (!IsLegalMoove);

                    if (game.IsGameFinished())
                        break;

                    //Console.WriteLine("The player 1 play the move:");
                    //game.PrintTable();
                    //Console.ReadLine();

                    //For player 2
                    IsLegalMoove = false;
                    AiOutput = new double[GRID_SIZE * GRID_SIZE];

                    do
                    {
                        try
                        {
                            AiOutput = AIplayer2.FeedForward(game.GetGridAsSingleTable(Players.Player2));
                            game.PlayMove(Players.Player2, GetPositionFromAiOutput(AiOutput));
                            IsLegalMoove = true;
                        }
                        catch (Exception)
                        {
                            errorCountPlayer2++;
                            AIplayer2.Mutate();
                        }
                    } while (!IsLegalMoove);

                }

                var winner = game.VerifyWinner();
                    Console.Write(" ");
                    switch (winner) 
                    {
                        case Players.Player1:
                            Console.WriteLine("Iteration: " + Iteration + " - The player 1 wins the game");
                            //if (errorCount <= BestNetworkErrorCount)
                            //{
                                BestNetwork = AIplayer1;
                                BestNetworkErrorCount = errorCount;
                            //}
                            break;
                        case Players.Player2:
                            Console.WriteLine("Iteration: " + Iteration + " - The player 2 wins the game");
                            //if (errorCountPlayer2 <= BestNetworkErrorCount)
                            //{
                                BestNetwork = AIplayer2;
                                BestNetworkErrorCount = errorCount;
                            //}
                        break;
                        case Players.NONE:
                            Console.WriteLine("Iteration: " + Iteration + " - The game is NULL");
                        break;
                    }
                


                // An iteration is done
                Iteration++;

                //    TicTacToeEngine.TicTacToe tic = new TicTacToeEngine.TicTacToe(3);
                //tic.PlayMove(Players.Player1, 0);
                //tic.PlayMove(Players.Player2, 3);
                //var output = tic.GetGridAsSingleTable(Players.Player1);

                //Console.WriteLine(output.ToString());

                if (Iteration % 100 == 0)
                {
                    Directory.CreateDirectory(OUTPUT_DIRECTORY);
                    File.AppendAllText(OUTPUT_DIRECTORY + "\\" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + "_Iteration_" + Iteration + "_.json", JsonConvert.SerializeObject(BestNetwork));
                    Directory.CreateDirectory(OUTPUT_DIRECTORY + "\\games");
                    File.AppendAllText(OUTPUT_DIRECTORY + "\\games\\" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + "_Iteration_" + Iteration + "_.txt", "Error count: " + errorCount + "\n" + game.GetGameString());


                    Console.Clear();
                    Console.WriteLine("Iteration: " + Iteration);
                    Console.WriteLine("WINNER: " + winner.ToString());
                    Console.WriteLine(game.GetGameString());

                    
                }

                if (Iteration % 10000 == 0)
                {
                    Console.WriteLine("Pressa any key to continue!");
                    Console.ReadLine();
                }

            }
        }

        private static int GetPositionFromAiOutput(double[] output)
        {
            var biggerValueIndex = 0;
            for(int index = 0; index<output.Length; index++)
            {
                if (output[index] > output[biggerValueIndex])
                    biggerValueIndex = index;
            }
            return biggerValueIndex;
        }
    }
}

