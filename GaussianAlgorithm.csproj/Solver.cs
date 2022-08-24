using System;
using System.Collections.Generic;
using System.Linq;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[][] ConcatenateMatrixes(double[][] matrix, double[] freeMembers)
        {
            var rows = matrix.Length;
            var columns = matrix[0].Length;
            var result = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                result[i] = new double[columns + 1];
                for (int j = 0; j < columns; j++)
                    result[i][j] = matrix[i][j];
                result[i][columns] = freeMembers[i];
            }
            return result;
        }

        public void SubstractVectorFromOthers
            (double[][] matrix, double[] vector, int column)
        {
            int rows = matrix.Length;
            int columns = matrix[0].Length;
            for (int i = 0; i < rows; i++)
            {
                if (matrix[i].Equals(vector))
                    continue;
                double coefficient = matrix[i][column] / vector[column];
                for (int j = column; j < columns; j++)
                    matrix[i][j] -= vector[j] * coefficient; 
            }
        }

        public bool IsTrivial(double[] vector)
        {
            int length = vector.Length;
            for (int i = 0; i < length; i++)
                if (Math.Abs(vector[i]) > 1e-5 && i != length - 1)
                    return false;
            return true;
        }

        public bool IsNoSolution(double[] vector)
        {
            int length = vector.Length;
            for (int i = 0; i < length - 1; i++)
                if (Math.Abs(vector[i]) > 1e-5)
                    return false;
            return vector[length - 1] != 0;
        }

        public double VectorMultiplyExceptOne(double[] vector, double[] solution, int j)
        {
            double result = 0;
            int length = solution.Length;
            for (int i = 0; i < length; i++)
                if (i != j && solution[i] != double.MinValue)
                    result += vector[i] * solution[i];
            return Math.Abs(result) < 1e-9? 0 : result;
        }

        public void FindVariables(double[] vector, double[] solution)
        {
            int length = solution.Length;
            int variablesToFind = 0;
            for (int j = 0; j < length; j++)
                if (Math.Abs(vector[j]) > 1e-5 && solution[j] == double.MinValue)
                    variablesToFind++;
            for (int j = 0; j < length; j++)
            {
                if (variablesToFind == 0) return;
                if (Math.Abs(vector[j]) > 1e-5 && solution[j] == double.MinValue && variablesToFind == 1)
                {
                    solution[j] = (vector[length] - VectorMultiplyExceptOne(vector, solution, j)) / vector[j];
                    variablesToFind--;
                }
                else if (Math.Abs(vector[j]) > 1e-5 && solution[j] == double.MinValue)
                {
                    solution[j] = 1;
                    variablesToFind--;
                }
            }
        }

        public double[] GetSolution(double[][] matrix)
        {
            int columns = matrix[0].Length;
            int rows = matrix.Length;
            var solution = new double[columns - 1].Select(x => double.MinValue).ToArray();
            if (matrix.Any(x => IsNoSolution(x))) throw new NoSolutionException("No solution!");
            var resultEquations = matrix.Where(x => !IsTrivial(x));
            for (int i = 0; i < rows; i++)
            {
                //if (IsSimpleEquation(matrix[i]))
                    //FindVarInSimpleEquation(matrix[i], solution);
                FindVariables(matrix[i], solution);
            }
            return solution;
        }

        public double[] Solve(double[][] matrix, double[] freeMembers)
        {
            var concMatrix = ConcatenateMatrixes(matrix, freeMembers);
            var flagsOfUsing = concMatrix.ToDictionary(x => x, x => 0);
            int rows = matrix.Length;
            int columns = matrix[0].Length;
            for (int i = 0; i < columns; i++)
            {
                double[] xRow = concMatrix.FirstOrDefault(x => Math.Abs(x[i]) > 1e-5 && flagsOfUsing[x] == 0);
                if (xRow is null) continue;
                SubstractVectorFromOthers(concMatrix, xRow, i);
                flagsOfUsing[xRow] = 1;
            }
            return GetSolution(concMatrix);
        }
    }
}
