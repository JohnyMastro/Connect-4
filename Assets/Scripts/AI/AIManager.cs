using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This manages AI's behaviour
public class AIManager : MonoBehaviour {
    static Column[] mColumns;
    // Use this for initialization
    void Start() {
        mColumns = FindObjectsOfType<Column>();
        if (mColumns.Length != Board.mGridWidth)
        {
            Debug.LogError("Incorrect number of Columns");
        }
        SortColumns();
    }

    // Update is called once per frame
    void Update() {

    }

    public static void PlayMove(PieceType[,] board, PieceType type)
    {
        int choice = EvaluateBoard(board, type);
        if (choice >= 0)
        {
            mColumns[choice].SpawnToken();
        }
    }
    private static int EvaluateBoard(PieceType[,] board, PieceType type)
    {
        int choice = 0;
        int[] choicesValue = new int[mColumns.Length];
        for (int i = 0; i < mColumns.Length; i++)
        {
            //evaluates all possible choices for the AI
            choicesValue[i] = AIHelper.EvaluateDecision(board, type, i);
        }
        choice = AIHelper.FindValidHighestChoice(choicesValue);
        return choice;
    }

    private static void SortColumns()
    {
        Column[] tempColumns = new Column[mColumns.Length];
        for (int i = 0; i < mColumns.Length; i++)
        {
            tempColumns[i] = mColumns[i];
        }

        for (int i = 0; i < mColumns.Length; i++)
        {
            mColumns[tempColumns[i].mColumnNumber] = tempColumns[i];
        }
    }
}
