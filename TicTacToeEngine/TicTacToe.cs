using System;

namespace TicTacToeEngine
{
    public class TicTacToe
    {

        //              COLUMN / ROW
        /*
         *             _____________________________
         *            | COLUMN1 | COLUMN1 | COLUMN1 |
         *        ROW |  
         *        ROW |
         *        ROW |
         *            
         */
        private static Players[][] GameGrid;


        public TicTacToe(int GridHeight)
        {
            if (GridHeight % 2 == 0)
                throw new Exception("Grid size can only be impair");
            InitialiseGrid(GridHeight);
        }

        private void InitialiseGrid(int GridHeight)
        {
            if (GridHeight <= 2)
                throw new Exception("Grid cannot be smaller then 3x3");

            GameGrid = new Players[GridHeight][];

            for (int column = 0 ; column < GameGrid.Length ; column++)
            {
                GameGrid[column] = new Players[GridHeight];
                for (var row = 0; row < GameGrid[column].Length; row++)
                {
                    GameGrid[column][row] = Players.NONE;
                }
            }
        }

        public void PlayMove(Players player, int position)
        {
            if (player == Players.NONE)
                throw new Exception("You cannot play as player NONE");
            if (position >= (GameGrid.Length * GameGrid.Length) || position < 0)
                throw new Exception("Out of bounds");

            int tempPos = -1;
            //Not optimised
            for (int column = 0; column < GameGrid.Length; column++)
            {
                for (var row = 0; row < GameGrid[column].Length; row++)
                {
                    tempPos++;
                    if(position == tempPos)
                    {
                        GameGrid[row][column] = player;
                    }                    
                }
            }
            

        }

        public Players VerifyWinner()
        {
            var winner = Players.NONE;

            winner = VerifyColumns();
            if (winner != Players.NONE)
                return winner;
            winner = VerifyRows();
            if (winner != Players.NONE)
                return winner;
            winner = VerifyDiagonals();
            if (winner != Players.NONE)
                return winner;

            return Players.NONE;
        }

        private static Players VerifyColumns()
        {
            foreach(var column in GameGrid)
            {
                var winner = VerifyColumn(column);
                if (winner != Players.NONE)
                    return winner;
            }

            return Players.NONE;
        }

        private static Players VerifyColumn(Players[] column)
        {
            var value = column[0];
            foreach(var row in column)
            {
                if (row != value)
                    return Players.NONE;
            }
            return value;
        }

        private static Players VerifyRows()
        {
            for(var rowIndex = 0; rowIndex< GameGrid.Length; rowIndex++)
            {
                var winner = VerifyRow(rowIndex);
                if (winner != Players.NONE)
                    return winner;
            }
            return Players.NONE;
        }

        private static Players VerifyRow(int row)
        {
            var winner = GameGrid[0][row];

            foreach(var column in GameGrid)
            {
                if (column[row] != winner)
                    return Players.NONE;
            }
            return winner;
        }

        private static Players VerifyDiagonals()
        {
            //diagonal from left to right 
            var winner = VerifyDiagonal(0);
            if (winner != Players.NONE)
                return winner;

            //diagonal from right to left 
            winner = VerifyDiagonal(1);
            if (winner != Players.NONE)
                return winner;

            return Players.NONE;
        }

        //If 0, verify diagonal from left to right,
        //If, 1, verify diagonal from right to left
        private static Players VerifyDiagonal(int diagonal)
        {
            if (diagonal != 0 && diagonal != 1)
                throw new Exception("Diagonal can only be 1 or 0");

            if(diagonal == 0)//from left to right
            {
                var winner = GameGrid[0][0];
                for (int i = 0; i < GameGrid.Length; i++)
                {
                    if (GameGrid[i][i] != winner)
                        return Players.NONE;
                }

                return winner;
            }else if(diagonal == 1)
            {
                var winner = GameGrid[GameGrid.Length - 1][0];
                for (int i = 0; i < GameGrid.Length; i++)
                {
                    if (GameGrid[GameGrid.Length-1-i][ i] != winner)
                        return Players.NONE;
                }

                return winner;
            }

            return Players.NONE;
        }


        public void PrintTable(int selectedColumn = -0, int selectedRow = -1)
        {
            if (selectedColumn >= GameGrid.Length || selectedRow >= GameGrid.Length)
                throw new Exception("Out of bounds");

            foreach(var temp in GameGrid)
                Console.Write(" " + new string('-', 7));
            Console.WriteLine();

            for (int row = 0; row < GameGrid.Length; row++)
            {
                for (var i = 0 ; i < GameGrid.Length; i++)
                    Console.Write((row == selectedRow && i == selectedColumn) ? "|\\     /": "|       ");
                Console.WriteLine("|");
                for (var column = 0; column < GameGrid[row].Length; column++)
                { 
                    Console.Write("|   " );
                    switch (GameGrid[column][row])
                    {
                        case Players.NONE:
                            Console.Write(" ");
                            break;
                        case Players.Player1:
                            Console.Write("X");
                            break;
                        case Players.Player2:
                            Console.Write("O");
                            break;
                    }
                    Console.Write("   ");
                    if(column == GameGrid[row].Length-1)
                        Console.WriteLine("|");
                    
                }
                for (var i = 0; i < GameGrid.Length; i++)
                    Console.Write((row == selectedRow && i == selectedColumn) ? "|/     \\" : "|       ");
                Console.WriteLine("|");

                foreach (var temp in GameGrid)
                    Console.Write(" " + new string('-', 7));
                Console.WriteLine();
            }
        }


        public static void TEST()
        {
            /*      -------------------------
            *       |       |       |       |
            *       |   X   |       |       |
            *       |       |       |       |
            *       -------------------------
            *       |       |       |       |
            *       |   X   |       |       |
            *       |       |       |       |
            *       -------------------------
            *       |       |       |       |
            *       |   X   |       |       |
            *       |       |       |       |
            *       -------------------------
            */

            Console.WriteLine("============================== TESTING NO WINNER =====================================");
            var engine = new TicTacToe(3);

            engine.PlayMove(Players.Player1, 8);

            engine.PrintTable();
            if (engine.VerifyWinner() != Players.NONE)
                throw new Exception("SHOULD BE PLAYERS.NONE");
            else
                Console.WriteLine("SUCCESS");



            engine = new TicTacToe(3);

            engine.PlayMove(Players.Player1, 0);
            engine.PlayMove(Players.Player1, 2);
            engine.PlayMove(Players.Player1, 6);
            engine.PlayMove(Players.Player1, 8);
            engine.PrintTable();
            if (engine.VerifyWinner() != Players.NONE)
                throw new Exception("SHOULD BE PLAYERS.PLAYER1");
            else
                Console.WriteLine("SUCCESS");


            Console.WriteLine("============================== TESTING COLUMNS =====================================");

            engine = new TicTacToe(3);

            engine.PlayMove(Players.Player1, 0);
            engine.PlayMove(Players.Player1, 3);
            engine.PlayMove(Players.Player1, 6);
            engine.PrintTable();
            if (engine.VerifyWinner() != Players.Player1)
                throw new Exception("SHOULD BE PLAYERS.PLAYER1");
            else
                Console.WriteLine("SUCCESS");


            Console.WriteLine("============================== TESTING ROWS =====================================");

            engine = new TicTacToe(3);

            engine.PlayMove(Players.Player1, 0);
            engine.PlayMove(Players.Player1, 1);
            engine.PlayMove(Players.Player1, 2);
            engine.PrintTable();
            if (engine.VerifyWinner() != Players.Player1)
                throw new Exception("SHOULD BE PLAYERS.PLAYER1");
            else
                Console.WriteLine("SUCCESS");

            engine = new TicTacToe(3);

            engine.PlayMove(Players.Player1, 3);
            engine.PlayMove(Players.Player1, 4);
            engine.PlayMove(Players.Player1, 5);
            engine.PrintTable();
            if (engine.VerifyWinner() != Players.Player1)
                throw new Exception("SHOULD BE PLAYERS.PLAYER1");
            else
                Console.WriteLine("SUCCESS");

            engine = new TicTacToe(3);

            engine.PlayMove(Players.Player1, 6);
            engine.PlayMove(Players.Player1, 7);
            engine.PlayMove(Players.Player1, 8);
            engine.PrintTable();
            if (engine.VerifyWinner() != Players.Player1)
                throw new Exception("SHOULD BE PLAYERS.PLAYER1");
            else
                Console.WriteLine("SUCCESS");



            Console.WriteLine("============================== TESTING DIAGONALS =====================================");

            engine = new TicTacToe(3);

            engine.PlayMove(Players.Player1, 0);
            engine.PlayMove(Players.Player1, 4);
            engine.PlayMove(Players.Player1, 8);
            engine.PrintTable();
            if (engine.VerifyWinner() != Players.Player1)
                throw new Exception("SHOULD BE PLAYERS.PLAYER1");
            else
                Console.WriteLine("SUCCESS");


            engine = new TicTacToe(3);

            engine.PlayMove(Players.Player1, 2);
            engine.PlayMove(Players.Player1, 4);
            engine.PlayMove(Players.Player1, 6);
            engine.PrintTable();
            if (engine.VerifyWinner() != Players.Player1)
                throw new Exception("SHOULD BE PLAYERS.PLAYER1");
            else
                Console.WriteLine("SUCCESS");



            Console.WriteLine("============================== DISPLAYING 9X9 GRID =====================================");

            engine = new TicTacToe(9);
            engine.PlayMove(Players.Player2, 80);
            engine.PrintTable();

        }
    }
}
