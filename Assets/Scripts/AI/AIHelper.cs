using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Helper functions for the AI
public class AIHelper {

    public static int FindValidHighestChoice(int[] choicesValue)
    {
        int choice = 0;
        int invalidCount=0;
        int initializedCount=0;
        while (true)
        {
            invalidCount = 0;
            initializedCount = 0;
            for (int i = 0; i < choicesValue.Length; i++)
            {
                if(choicesValue[i] == 0)
                {
                    initializedCount++;
                    if(initializedCount>= Board.mGridWidth)
                    {
                        //init choice if the board is empty, randomize
                        choice = Random.Range(0, Board.mGridWidth);
                        break;
                    }
                }
                if (choicesValue[i] > choicesValue[choice])
                {
                    choice = i;
                }
            }
            if (Board.ColumnHasRoom(choice))
            {
                break;
            }
            else
            {
                choicesValue[choice] = -1;
                invalidCount++;
                if (invalidCount >= Board.mGridWidth)
                {
                    //no options
                    choice = -1;
                    break;
                }

            }

        }
        return choice;
    }


    public static int EvaluateDecision(PieceType[,] board, PieceType type, int columnNum)
    {
        PieceType enemy;
        if (type == PieceType.RED)
        {
            enemy = PieceType.YELLOW;
        }
        else
        {
            enemy = PieceType.RED;
        }

        int value = 0;

        //Evaluate decisions to maximize value for AI's move
        PieceType[,] boardCopy = Board.BoardCopy(board);
        int rowNum = CreateNextState(ref boardCopy, type, columnNum);
        Vector2Int startingPoint = new Vector2Int(columnNum, rowNum);
        value = ClusterCount(startingPoint, type, boardCopy);

        //Minimizes the enemy's value if they made that move
        PieceType[,] enemyBoardCopy = Board.BoardCopy(board);
        int enemyRowNum = CreateNextState(ref enemyBoardCopy, enemy, columnNum);
        Vector2Int enemyStartingPoint = new Vector2Int(columnNum, enemyRowNum);
        value += ClusterCount(enemyStartingPoint, enemy, enemyBoardCopy);

        return value;
    }
    //used to create a hypothetical state to evaluate the AI's choice
    private static int CreateNextState(ref PieceType[,] boardCopy, PieceType type, int columnNum)
    {
        int row = 0;
        for (int j = 0; j <= Board.mGridHeight; j++)
        {
            if (j == Board.mGridHeight)
            {
                boardCopy[columnNum, j - 1] = type;
                row = j - 1;
                break;
            }
            else if ((boardCopy[columnNum, j] != PieceType.EMPTY) && (j - 1) >= 0)
            {
                boardCopy[columnNum, j - 1] = type;
                row = j - 1;
                break;
            }
        }
        return row;
    }
    //This is used to evaluate the AI's Hypothetical choice
    private static int ClusterCount(Vector2Int startingPoint, PieceType type, PieceType[,] grid)
    {
        int count = 0;
        count += CountHorizontal(startingPoint, type, grid);
        count += CountDiagTLDR(startingPoint, type, grid);
        count += CountDiagDLTR(startingPoint, type, grid);
        count += CountDown(startingPoint, type, grid);
        return count;
    }

    private static int CountHorizontal(Vector2Int startingPoint, PieceType type, PieceType[,] grid)
    {
        int value = Board.CountRight(startingPoint, type, grid) + Board.CountLeft(startingPoint, type, grid);
        if (value >= 3)
        {
            value += 100;
        }
        return value;

    }
    private static int CountDiagTLDR(Vector2Int startingPoint, PieceType type, PieceType[,] grid)
    {
        int value = Board.CountDownRight(startingPoint, type, grid) + Board.CountUpLeft(startingPoint, type, grid);
        if (value >= 3)
        {
            value += 100;
        }
        return value;

    }
    private static int CountDiagDLTR(Vector2Int startingPoint, PieceType type, PieceType[,] grid)
    {
        int value = Board.CountUpRight(startingPoint, type, grid) + Board.CountDownLeft(startingPoint, type, grid);
        if (value >= 3)
        {
            value += 100;
        }
        return value;
    }
    private static int CountDown(Vector2Int startingPoint, PieceType type, PieceType[,] grid)
    {
        int value = Board.CountDown(startingPoint, type, grid);
        if (value >= 3)
        {
            value += 100;
        }
        return value;
    }

    
}
