using System;

public class Compression
{
    public static string Compress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        StringBuilder compressed = new StringBuilder();
        char currentChar = input[0];
        int count = 1;

        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] == currentChar)
            {
                count++;
            }
            else
            {
                compressed.Append(currentChar);
                if (count > 1)
                {
                    compressed.Append(count);
                }
                currentChar = input[i];
                count = 1;
            }
        }

        compressed.Append(currentChar);
        if (count > 1)
        {
            compressed.Append(count);
        }

        return compressed.ToString();
    }

    static string Decompress(string input)
    {
        StringBuilder decompressed = new StringBuilder();
        char currentChar = '0';
        string countString = string.Empty;

        foreach (char c in input)
        {
            if (char.IsLetter(c))
            {
                if (currentChar != '0')
                {
                    int count = string.IsNullOrEmpty(countString) ? 1 : int.Parse(countString);
                    decompressed.Append(new string(currentChar, count));
                }
                currentChar = c;
                countString = string.Empty;
            }
            else if (char.IsDigit(c))
            {
                countString += c;
            }
        }

        if (currentChar != '0')
        {
            int count = string.IsNullOrEmpty(countString) ? 1 : int.Parse(countString);
            decompressed.Append(new string(currentChar, count));
        }

        return decompressed.ToString();
    }
}

