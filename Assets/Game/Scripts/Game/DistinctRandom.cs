using System;
using System.Collections.Generic;
using System.Linq;

public class DistinctRandom
{
    private static readonly Random _random = new Random();
    private readonly int[] _numbers;
    private List<int> _notGeneratedNumbers;

    public DistinctRandom(int[] generateFrom)
    {
        _numbers = generateFrom;
        _notGeneratedNumbers = new List<int>(generateFrom);
    }

    public int Next()
    {
        if (!_notGeneratedNumbers.Any())
        {
            _notGeneratedNumbers = new List<int>(_numbers);
        }

        var pickIndex = _random.Next(_notGeneratedNumbers.Count);
        var randNumber = _notGeneratedNumbers[pickIndex];
        _notGeneratedNumbers.RemoveAt(pickIndex);
        return randNumber;
    }
}