

using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.InteropServices;

namespace AdventOfCode.Solutions.Year2024.Day07;

class Solution : SolutionBase
{
    public Solution() : base(07, 2024, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = Input.SplitByNewline().ToList();

        Dictionary<long, List<long>> calibrationData = ParseCalibrationData(lines);
        Dictionary<long, bool> validCalibrationData = DetermineCalDataValid(calibrationData, 2);
        long totalSum = GetFinalCalData(validCalibrationData);

        //Attempt 1: 424 - Too low - didn't sum the valid calibration data
        //Attempt 2: 1582598718861 - CORRECT
        return totalSum.ToString();
    }

    protected override string SolvePartTwo()
    {
        List<string> lines = Input.SplitByNewline().ToList();

        Dictionary<long, List<long>> calibrationData = ParseCalibrationData(lines);
        Dictionary<long, bool> validCalibrationData = DetermineCalDataValid(calibrationData, 2);
        long totalSum1 = GetFinalCalData(validCalibrationData);

        //Get only the invalid calibration data
        Dictionary<long, bool> invalidCalibrationData = validCalibrationData.Where(x => x.Value == false).Select(t => new { t.Key, t.Value }).ToDictionary(t => t.Key, t => t.Value);
        Dictionary<long, List<long>> calibrationData2 = new();

        foreach (var item in invalidCalibrationData.Keys)
        {
            calibrationData2.Add(item, calibrationData[item]);
        }

        //Now go try concatentating on this data
        Dictionary<long, bool> validCalibrationData2 = DetermineCalDataValid(calibrationData2, 3);
        long totalSum2 = GetFinalCalData(validCalibrationData2);

        Dictionary<long, bool> valid3 = new();
        long sum = 0;
        foreach (long item in calibrationData.Keys)
        {
            long[] numbers = calibrationData[item].ToArray();
            long theValue = solve(item, numbers.First(), numbers, 1, 3);
            valid3.Add(item, theValue.Equals(0) == false);
            sum += theValue;
        }

        //Only the true items
        List<long> valid4 = valid3.Where(x => x.Value).Select(x => x.Key).ToList();
        List<long> valid5 = validCalibrationData.Where(x => x.Value).Select(x => x.Key).ToList();
        List<long> valid6 = validCalibrationData2.Where(x => x.Value).Select(x => x.Key).ToList();
        valid5.AddRange(valid6);


        //Valid answers not caught:
        // Line 335: 10728897
        // Line 372: 1691650192
        // Line 458: 213283675
        List<long> exceptions = valid4.Except(valid5).ToList();

        //Attempt 1 - 163693637141019 - too low - didn't add the two total sum values together
        //Attempt 2 - 165276235859880 - too low 
        //Attempt 3 - 165278151522644 - CORRECT using solve()
        return (sum).ToString();
    }

    private long solve(long lhs, long total, long[] rhs, int ri, int ops)
    {
        if (lhs == total && rhs.Length == ri)
        {
            return total;
        }
        else if (rhs.Length == ri)
        {
            return 0;
        }
        else if (total > lhs)
        {
            return 0;
        }
        else
        {
            long result = solve(lhs, total + rhs[ri], rhs, ri + 1, ops);
            if (result == lhs)
            {
                return lhs;
            }
            result = solve(lhs, total * rhs[ri], rhs, ri + 1, ops);
            if (result == lhs)
            {
                return lhs;
            }
            if (ops == 3)
            {
                return solve(lhs, long.Parse(total + "" + rhs[ri]), rhs, ri + 1, ops);
            }
            else
            {
                return 0;
            }
        }
    }

    private Dictionary<long, List<long>> ParseCalibrationData(List<string> lines)
    {
        Dictionary<long, List<long>> calibrationData = new();
        foreach (string line in lines)
        {
            string[] theSplit = line.Trim().Split(":");
            long answer = long.Parse(theSplit[0]);
            string[] theNums = theSplit.Last().Trim().Split(" ");
            List<long> numLst = new List<long>();
            foreach (string item in theNums)
            {
                numLst.Add(long.Parse(item));
            }

            calibrationData.Add(answer, numLst);
        }
        return calibrationData;
    }

    private Dictionary<long, bool> DetermineCalDataValid(Dictionary<long, List<long>> calibrationData, int baseNumber)
    {
        Dictionary<long, bool> valid = new Dictionary<long, bool>();

        foreach (long item in calibrationData.Keys)
        {
            List<long> numbers = calibrationData[item];
            int spaces = numbers.Count - 1;

            //Make a bit array with the length equal to the number of (operator) spaces between numbers
            // FALSE (0) will represent an addition operation
            // TRUE (1) will represent a multiplication operation
            ushort tracker = 0;
            char[] theChars;
            double max = Math.Pow(baseNumber, spaces);
            valid.Add(item, false);
            for (int i = 0; i < max; i++)
            {
                theChars = DetermineCharArray(tracker, baseNumber, spaces);

                //Perform the operation with the current set of operators
                long operationAnswer = 0;
                GetAnswer(theChars, numbers, out operationAnswer);

                //If the answer from the operation matches the expected answer (the key)
                //set the boolean flag to true and break out of the inner for loop
                if (item.Equals(operationAnswer))
                {
                    valid[item] = true;
                    break;
                }

                //Otherwise, increment the tracking number by 1 and 
                //determine the char array for the next operation at the top of the loop
                tracker++;
            }
        }

        return valid;
    }

    private void GetAnswer(char[] theChars, List<long> numbers, out long operationAnswer)
    {
        //Operators are always evaluated left-to-right, not according to precedence rules
        //because of this rule - set the initial value of operationAnswer to the first number of the list        
        operationAnswer = (long)numbers.FirstOrDefault();
        for (int i = 0; i < theChars.Length; i++)
        {
            switch (theChars[i])
            {
                case '+':
                    operationAnswer += numbers[i + 1];
                    break;
                case '*':
                    operationAnswer *= numbers[i + 1];
                    break;
                case '|':
                    string newConcat = string.Concat(operationAnswer.ToString(), numbers[i + 1]);
                    operationAnswer = long.Parse(newConcat);
                    break;
            }
        }
    }

    private long GetFinalCalData(Dictionary<long, bool> validCalibrationData)
    {
        long totalSum = 0;
        foreach (long item in validCalibrationData.Keys)
        {
            if (validCalibrationData[item] == true)
                totalSum += item;
        }
        return totalSum;
    }

    private char[] DetermineCharArray(ushort trackingNumber, int baseValue, int maxSpaces)
    {
        int[] newInt = new int[1] { trackingNumber };
        List<int> positions = Convert(trackingNumber, baseValue);

        char[] c = new char[maxSpaces];

        //Only go up to the number of spaces
        for (int i = 0; i < maxSpaces; i++)
        {
            if (i >= positions.Count())
                c[i] = '+';
            else
                c[i] = positions[i] == 0 ? '+' : positions[i] == 1 ? '*' : '|';
        }

        return c;
    }

    private List<int> Convert(int numberToConvert, int baseValue)
    {
        List<int> numericPositions = new List<int>();

        int remainder, quotient = numberToConvert;
        while (quotient != 0)
        {
            convertToTernary(numberToConvert, baseValue, out quotient, out remainder);
            numberToConvert = quotient;
            numericPositions.Add(remainder);
        }
        return numericPositions;
    }

    private void convertToTernary(int number, int baseValue, out int quotient, out int remainder)
    {
        quotient = 0;
        remainder = 0;
        //Stop case - done when number is zero
        if (number == 0)
            return;

        //Get remainder 
        remainder = number % baseValue;

        //Divide by the base 
        quotient = number / baseValue;
    }

}
