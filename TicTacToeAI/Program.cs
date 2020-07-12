using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using TicTacToeEngine;
using TwitchAPI.Utils.Menu;

namespace TicTacToeAI
{
    class Program
    {

        private static readonly int GRID_SIZE = 3;

        private static readonly string OUTPUT_DIRECTORY = Directory.GetCurrentDirectory() + "\\output";

        private static NeuralNetwork _BestNetwork;

        static void Main(string[] args)
        {
            _BestNetwork = new NeuralNetwork(new uint[] { 18, 13, 9 }); // The best network currently made

            CustomConsoleMenu menu = new CustomConsoleMenu();
            menu.AddChoice(0, "Train Network 10 times");
            menu.AddChoice(2, "Train Network X times");
            menu.AddChoice(1, "Play against current network");
            menu.AddChoice(3, "Save network");

            menu.ChoiceSelection += Menu_ChoiceSelection;

            while (true)
            {
                Console.Clear();

                menu.DisplayMenu();
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
            }
        }

        private static void Menu_ChoiceSelection(object sender, ChoiceSelectionArgs e)
        {
            switch (e.SelectedID)
            {
                case 0:
                    _BestNetwork = TrainNeutworkXTimes(_BestNetwork, 10);
                    break;
                case 1:
                    UserVsAi(_BestNetwork);
                    break;
                case 2:
                    Console.WriteLine("Enter the amount of times you want to train the network: ");
                    bool isValidNumber = int.TryParse(Console.ReadLine(), out var result);

                    if(!isValidNumber)
                    {
                        Console.WriteLine("INVALID NUMBER");
                        return;
                    }

                    _BestNetwork = TrainNeutworkXTimes(_BestNetwork, result);
                    break;
                case 3:
                    Console.WriteLine("Enter file name:");
                    var fileName = Console.ReadLine();
                    if(string.IsNullOrEmpty(fileName))
                    {
                        Console.WriteLine("INVALID File name");
                        return;
                    }

                    saveNetwork(fileName, _BestNetwork);
                    break;
            }
        }

        private static void UserVsAi(NeuralNetwork network, bool isAiStarting = true)
        {
            var game = new TicTacToe(GRID_SIZE);
            var AIplayer1 = new NeuralNetwork(network);

            while (!game.IsGameFinished())
            {
                if (isAiStarting)
                {
                    AIPlayMoove(AIplayer1, game, Players.Player1);
                    game.PrintTable();
                    if (game.IsGameFinished())
                        break;
                    WaitForUserInput(game, Players.Player2);
                }
                else
                {
                    WaitForUserInput(game, Players.Player1);
                    if (game.IsGameFinished())
                        break;
                    AIPlayMoove(AIplayer1, game, Players.Player2);
                }
            }

            if(game.VerifyWinner() != Players.NONE)
            {                
                Console.WriteLine(game.VerifyWinner() + " wins!");
            }

            //Console.WriteLine("Press any key to continue");
            //Console.ReadLine();
        }

        private static NeuralNetwork TrainNeutworkXTimes(NeuralNetwork BestNetwork, int numberOfTimes)
        {
            int Iteration = 0; // Current Training Iteration


            while (Iteration <= numberOfTimes) // Keep Training untill the number of times is reached
            {               

                var game = new TicTacToe(GRID_SIZE);

                var AIplayer1 = new NeuralNetwork(BestNetwork);
                var AIplayer2 = new NeuralNetwork(BestNetwork);

                var certaintyPlayer1 = 0.0;
                var certaintyPlayer2 = 0.0;

                //AIplayer1.Mutate();
                AIplayer2.Mutate();

                while (!game.IsGameFinished())
                {

                    //For player 1
                    certaintyPlayer1 = AIPlayMoove(AIplayer1, game, Players.Player1);

                    if (game.IsGameFinished())
                        break;

                    //Player 2
                    certaintyPlayer2 = AIPlayMoove(AIplayer2, game, Players.Player2);

                }

                var winner = game.VerifyWinner();
                //Console.Write(" ");
                switch (winner)
                {
                    case Players.Player1:
                            Console.WriteLine("Iteration: " + Iteration + "/"+ numberOfTimes+" (" + ((double)Iteration/numberOfTimes*100).ToString("0.00") + "%) - The player 1 wins the game ");
                            BestNetwork = AIplayer1;
                        break;
                    case Players.Player2:
                            Console.WriteLine("Iteration: " + Iteration + "/" + numberOfTimes + " (" + ((double)Iteration / numberOfTimes * 100).ToString("0.00") + "%) - The player 2 wins the game ");
                            BestNetwork = AIplayer2;
                        break;
                    case Players.NONE:
                        Console.WriteLine("Iteration: " + Iteration + "/" + numberOfTimes + " (" + ((double)Iteration / numberOfTimes * 100).ToString("0.00") + "%) - The game is NULL");
                        break;
                }
                Iteration++;

                //    TicTacToeEngine.TicTacToe tic = new TicTacToeEngine.TicTacToe(3);
                //tic.PlayMove(Players.Player1, 0);
                //tic.PlayMove(Players.Player2, 3);
                //var output = tic.GetGridAsSingleTable(Players.Player1);

                //Console.WriteLine(output.ToString());

                //if (Iteration % 100 == 0)
                //{
                //    Directory.CreateDirectory(OUTPUT_DIRECTORY);
                //    File.AppendAllText(OUTPUT_DIRECTORY + "\\" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + "_Iteration_" + Iteration + "_.json", JsonConvert.SerializeObject(BestNetwork));
                //    Directory.CreateDirectory(OUTPUT_DIRECTORY + "\\games");
                //    File.AppendAllText(OUTPUT_DIRECTORY + "\\games\\" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + "_Iteration_" + Iteration + "_.txt",game.GetGameString());


                //    Console.Clear();
                //    Console.WriteLine("Iteration: " + Iteration);
                //    Console.WriteLine("WINNER: " + winner.ToString());
                //    Console.WriteLine(game.GetGameString());


                //}

                //if (Iteration % 10000 == 0)
                //{
                //    Console.WriteLine("Pressa any key to continue!");
                //    Console.ReadLine();
                //}

            }
            return BestNetwork;
        }

        private static double AIPlayMoove(NeuralNetwork AIplayer, TicTacToe game, Players playerNumber)
        {
            var IsLegalMoove = false;
            var AiOutput = new double[GRID_SIZE * GRID_SIZE];

            var certainty = 0.0;

            for(int i = 0; i < AiOutput.Length; i++)
            {
                try
                {
                    AiOutput = AIplayer.FeedForward(game.GetGridAsSingleTable(playerNumber));
                    game.PlayMove(playerNumber, GetPositionFromAiOutput(AiOutput, i, out certainty));
                    IsLegalMoove = true;
                }
                catch (Exception)
                {
                }
                if (IsLegalMoove)
                    return certainty;
            }
            return certainty;
        }
        private static int GetPositionFromAiOutput(double[] output, int positionOfBiggerIndex, out double certainty)
        {
            certainty = output.OrderByDescending(c => c).ToList()[positionOfBiggerIndex];
            return Array.IndexOf(output, certainty);
        }

        private static void WaitForUserInput(TicTacToe game , Players playerNumber)
        {
            var selectedRow = 0;
            var selectedColumn = 0;
            var isValidMoove = false;

            Console.WriteLine(game.GetTableUIString(selectedColumn, selectedRow));

            while (!isValidMoove) { 
                var keyInput = Console.ReadKey().Key;
                while (keyInput != ConsoleKey.Enter)
                {
                    Console.Clear();

                    switch (keyInput)
                    {
                        case ConsoleKey.UpArrow:
                            if (selectedRow > 0)
                                selectedRow--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (selectedRow < GRID_SIZE-1)
                                selectedRow++;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (selectedColumn > 0)
                                selectedColumn--;
                            break;
                        case ConsoleKey.RightArrow:
                            if (selectedColumn < GRID_SIZE - 1)
                                selectedColumn++;
                            break;
                    }

                    Console.WriteLine(game.GetTableUIString(selectedColumn,selectedRow));

                    keyInput = Console.ReadKey().Key;
                }
                try
                {
                    game.PlayMove(playerNumber, (selectedRow*GRID_SIZE+selectedColumn));
                    isValidMoove = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("INVALID MOOVE, pick again");
                }
            }

            game.PrintTable();

            Console.WriteLine("Valid moove");
        }

        private static void saveNetwork(string fileName, NeuralNetwork network)
        {
            Directory.CreateDirectory(OUTPUT_DIRECTORY);
            File.AppendAllText(OUTPUT_DIRECTORY + "\\" + fileName + ".json", JsonConvert.SerializeObject(network));
        }
    }
}

