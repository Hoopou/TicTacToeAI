using System;
using System.Collections.Generic;
using System.Text;

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
        private Players[][] GameGrid;

        private List<Moove> mooves = new List<Moove>();

        public TicTacToe(int GridHeight)
        {
            if (GridHeight % 2 == 0)
                throw new Exception("Grid size can only be impair");
            InitialiseGrid(GridHeight);
        }

        public void PlayMove(Players player, int position)
        {
            if (player == Players.NONE)
                throw new Exception("You cannot play as player NONE");
            if (position >= (GameGrid.Length * GameGrid.Length) || position < 0)
                throw new System.Exception("Out of bounds");

            int tempPos = -1;
            //Not optimised
            for (int column = 0; column < GameGrid.Length; column++)
            {
                for (var row = 0; row < GameGrid[column].Length; row++)
                {
                    tempPos++;
                    if(position == tempPos)
                    {
                        if (GameGrid[row][column] != Players.NONE)
                            throw new Exception("Already occupied");
                        GameGrid[row][column] = player;
                        mooves.Add(new Moove() {Player=player, Position=position});
                    }                    
                }
            }
        }

        public bool IsGameFinished()
        {
            if (VerifyWinner() != Players.NONE)
                return true;

            for (int column = 0; column < GameGrid.Length; column++)
            {
                for (var row = 0; row < GameGrid[column].Length; row++)
                {
                    if (GameGrid[row][column] == Players.NONE)
                        return false;
                }
            }
            return true;
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

        public double[] GetGridAsSingleTable(Players currentPlayer)
        {
            if (currentPlayer == Players.NONE)
                throw new Exception("You cannot get the NONE PLAYER");

            double[] newTable = new double[GameGrid.Length * GameGrid.Length * 2];
            var index = -1;
            for (int column = 0; column < GameGrid.Length; column++)
            {
                for (var row = 0; row < GameGrid[column].Length; row++)
                {
                    index++;
                    if (GameGrid[row][column] != Players.NONE)
                    {
                        if (GameGrid[row][column] == currentPlayer)
                            newTable[index] = 1;
                        else
                            newTable[index + GameGrid.Length * GameGrid.Length] = 1;
                    }
                    //newTable[index] = currentPlayer == GameGrid[row][column]?1:0;
                }
            }

            return newTable;
        }

        public string GetTableUIString(int selectedColumn = -0, int selectedRow = -1)
        {
            if (selectedColumn >= GameGrid.Length || selectedRow >= GameGrid.Length)
                throw new Exception("Out of bounds");

            StringBuilder tableUI = new StringBuilder();

            foreach (var temp in GameGrid)
                tableUI.Append(" " + new string('-', 7));
            tableUI.AppendLine();

            for (int row = 0; row < GameGrid.Length; row++)
            {
                for (var i = 0; i < GameGrid.Length; i++)
                    tableUI.Append(((row == selectedRow && i == selectedColumn) ? "|\\     /" : "|       "));
                tableUI.AppendLine("|");
                for (var column = 0; column < GameGrid[row].Length; column++)
                {
                    tableUI.Append("|   ");
                    switch (GameGrid[column][row])
                    {
                        case Players.NONE:
                            tableUI.Append(" ");
                            break;
                        case Players.Player1:
                            tableUI.Append("X");
                            break;
                        case Players.Player2:
                            tableUI.Append("O");
                            break;
                    }
                    tableUI.Append("   ");
                    if (column == GameGrid[row].Length - 1)
                        tableUI.AppendLine("|");

                }
                for (var i = 0; i < GameGrid.Length; i++)
                    tableUI.Append((row == selectedRow && i == selectedColumn) ? "|/     \\" : "|       ");
                tableUI.AppendLine("|");

                foreach (var temp in GameGrid)
                    tableUI.Append(" " + new string('-', 7));
                tableUI.AppendLine();

            }
            return tableUI.ToString();
        }

        public void PrintTable()
        {
            Console.WriteLine(GetTableUIString());
        }

        public string GetGameString()
        {
            var tempTictactoe = new TicTacToe(GameGrid.Length);
            var gameStringBuilder = new StringBuilder();
            foreach (var moove in mooves)
            {
                tempTictactoe.PlayMove(moove.Player, moove.Position);
                gameStringBuilder.AppendLine(tempTictactoe.GetTableUIString());
            }
            return gameStringBuilder.ToString();
        }


        /////////////////////////////////////// PRIVATE


        private void InitialiseGrid(int GridHeight)
        {
            if (GridHeight <= 2)
                throw new Exception("Grid cannot be smaller then 3x3");

            GameGrid = new Players[GridHeight][];

            for (int column = 0; column < GameGrid.Length; column++)
            {
                GameGrid[column] = new Players[GridHeight];
                for (var row = 0; row < GameGrid[column].Length; row++)
                {
                    GameGrid[column][row] = Players.NONE;
                }
            }
        }

        private Players VerifyColumns()
        {
            foreach(var column in GameGrid)
            {
                var winner = VerifyColumn(column);
                if (winner != Players.NONE)
                    return winner;
            }

            return Players.NONE;
        }

        private Players VerifyColumn(Players[] column)
        {
            var value = column[0];
            foreach(var row in column)
            {
                if (row != value)
                    return Players.NONE;
            }
            return value;
        }

        private Players VerifyRows()
        {
            for(var rowIndex = 0; rowIndex < GameGrid.Length; rowIndex++)
            {
                var winner = VerifyRow(rowIndex);
                if (winner != Players.NONE)
                    return winner;
            }
            return Players.NONE;
        }

        private Players VerifyRow(int row)
        {
            var winner = GameGrid[0][row];

            foreach(var column in GameGrid)
            {
                if (column[row] != winner)
                    return Players.NONE;
            }
            return winner;
        }

        private Players VerifyDiagonals()
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
        private Players VerifyDiagonal(int diagonal)
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

            Console.WriteLine("============================== DISPLAYING GAME =====================================");

            engine = new TicTacToe(3);

            engine.PlayMove(Players.Player1, 0);
            engine.PlayMove(Players.Player2, 1);
            engine.PlayMove(Players.Player1, 2);
            engine.PlayMove(Players.Player2, 3);

            Console.WriteLine(engine.GetGameString());


        }

        
    }
}
