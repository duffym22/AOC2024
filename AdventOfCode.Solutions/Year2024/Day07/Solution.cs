

using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Net;

namespace AdventOfCode.Solutions.Year2024.Day07;

class Solution : SolutionBase
{
    public Solution() : base(07, 2024, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = Input.SplitByNewline().ToList();

        Dictionary<ulong, List<ulong>> calibrationData = ParseCalibrationData(lines);
        Dictionary<ulong, bool> validCalibrationData = DetermineCalDataValid(calibrationData);
        ulong totalSum = GetFinalCalData(validCalibrationData);

        //Attempt 1: 424 - Too low - didn't sum the valid calibration data
        //Attempt 2: 1582598718861 - CORRECT
        return totalSum.ToString();
    }

    protected override string SolvePartTwo()
    {
        List<string> lines = Input.SplitByNewline().ToList();

        Dictionary<ulong, List<ulong>> calibrationData = ParseCalibrationData(lines);
        Dictionary<ulong, bool> validCalibrationData = DetermineCalDataValid(calibrationData);

        //Get only the invalid calibration data
        Dictionary<ulong, bool> invalidCalibrationData = validCalibrationData.Where(x => x.Value == false).Select(t => new { t.Key, t.Value }).ToDictionary(t => t.Key, t=> t.Value);
        Dictionary<ulong, List<ulong>> calibrationData2 = new();

        foreach (var item in invalidCalibrationData.Keys)
        {
            calibrationData2.Add(item, calibrationData[item]);
        }

        //Now go try concatentating on this data
        Dictionary<ulong, bool> validCalibrationData2 = DetermineCalDataValid(calibrationData2);

        return "";
    }

    private Dictionary<ulong, List<ulong>> ParseCalibrationData(List<string> lines)
    {
        Dictionary<ulong, List<ulong>> calibrationData = new();
        foreach (string line in lines)
        {
            string[] theSplit = line.Trim().Split(":");
            ulong answer = ulong.Parse(theSplit[0]);
            string[] theNums = theSplit.Last().Trim().Split(" ");
            List<ulong> numLst = new List<ulong>();
            foreach (string item in theNums)
            {
                numLst.Add(ulong.Parse(item));
            }

            calibrationData.Add(answer, numLst);
        }
        return calibrationData;
    }

    private Dictionary<ulong, bool> DetermineCalDataValid(Dictionary<ulong, List<ulong>> calibrationData)
    {
        Dictionary<ulong, bool> valid = new Dictionary<ulong, bool>();

        //Default all lines to false
        foreach (var item in calibrationData)
        {
            valid.Add(item.Key, false);
        }

        foreach (ulong item in calibrationData.Keys)
        {
            List<ulong> numbers = calibrationData[item];
            int spaces = numbers.Count - 1;

            //Make a bit array with the length equal to the number of (operator) spaces between numbers
            // FALSE (0) will represent an addition operation
            // TRUE (1) will represent a multiplication operation
            ushort tracker = 0;
            char[] theChars;
            double max = Math.Pow(2, spaces);
            for (int i = 0; i < max; i++)
            {
                theChars = DetermineCharArrayFromUShort(tracker, spaces);

                //Perform the operation with the current set of operators
                ulong operationAnswer = 0;
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

    private void GetAnswer(char[] theChars, List<ulong> numbers, out ulong operationAnswer)
    {
        //Operators are always evaluated left-to-right, not according to precedence rules
        //because of this rule - set the initial value of operationAnswer to the first number of the list        
        operationAnswer = (ulong)numbers.FirstOrDefault();
        for (int i = 0; i < theChars.Length; i++)
        {
            if (theChars[i].Equals('+'))
                operationAnswer += numbers[i + 1];
            else
                operationAnswer *= numbers[i + 1];
        }
    }    

    private ulong GetFinalCalData(Dictionary<ulong, bool> validCalibrationData)
    {
        ulong totalSum = 0;
        foreach (ulong item in validCalibrationData.Keys)
        {
            if (validCalibrationData[item] == true)
                totalSum += item;
        }
        return totalSum;
    }

    private char[] DetermineCharArrayFromUShort(ushort trackingNumber, int maxSpaces)
    {
        int[] newInt = new int[1] { trackingNumber };
        BitArray bitArray = new BitArray(newInt);
        char[] c = new char[maxSpaces];

        //Only go up to the number of spaces
        for (int i = 0; i < maxSpaces; i++)
        {
            //When the number is converted to a BitArray
            //leading bits are dropped, so allow them to be added back 
            if (i > bitArray.Length)
            {
                c[i] = '+';
            }
            else
                c[i] = bitArray[i] ? '*' : '+';
        }

        return c;
    }

}
