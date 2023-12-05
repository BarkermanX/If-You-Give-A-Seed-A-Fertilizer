// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

try
{ 
    // Specify the path to the text file
    string filePath = "TestData.txt";

    // Check if the file exists
    if (File.Exists(filePath))
    {
        // Read all lines from the file
        string[] lines = File.ReadAllLines(filePath);

        // Display the content of the file
        Console.WriteLine("Content of the file:");

        string strStage = string.Empty;

        foreach (string line in lines)
        {

            if (line.Contains("seeds"))
            {
                strStage = "Seeds";
            }

            if (line.Contains("seed-to-soil map"))
            {
                strStage = "Soil";
                continue;
            }

            if (line.Contains("soil-to-fertilizer map"))
            {
                strStage = "Fertlizer";
                continue;
            }

            if (line.Contains("fertilizer-to-water map"))
            {
                strStage = "Water";
                continue;
            }

            if (line.Contains("water-to-light map"))
            {
                strStage = "Light";
                continue;
            }

            if (line.Contains("light-to-temperature map"))
            {
                strStage = "Temperature";
                continue;
            }

            if (line.Contains("temperature-to-humidity map"))
            {
                strStage = "Humidity";
                continue;
            }

            if (line.Contains("humidity-to-location map"))
            {
                strStage = "Location";
                continue;
            }

            switch (strStage)
            {
                case "Seeds":
                    Console.Write("Processing Seeds");
                    Helper.addSeeds(line);
                    break;

                case "Soil":
                    Console.WriteLine("Processing Soil");
                    //Helper.addDictionaryItem(Helper.dctSeedToSoil, line, Helper.dctSeeds);
                    Helper.lstSoil.Add(line);
                    break;

                case "Fertlizer":
                    Console.WriteLine("Processing fertilizer");
                    //Helper.addDictionaryItem(Helper.dctSoilToFertilizer, line, Helper.dctSeedToSoil);
                    Helper.lstFert.Add(line);
                    break;

                case "Water":
                    Console.WriteLine("Processing Water");
                    //Helper.addDictionaryItem(Helper.dctFertlizerToWater, line, Helper.dctSoilToFertilizer);
                    Helper.lstWater.Add(line);
                    break;

                case "Light":
                    Console.WriteLine("Processing Light");
                    //Helper.addDictionaryItem(Helper.dctWaterToLight, line, Helper.dctFertlizerToWater);
                    Helper.lstLight.Add(line);
                    break;

                case "Temperature":
                    Console.WriteLine("Processing Temperature");
                    //Helper.addDictionaryItem(Helper.dctLightToTemp, line, Helper.dctWaterToLight);
                    Helper.lstTemp.Add(line);
                    break;

                case "Humidity":
                    Console.WriteLine("Processing Humidity");
                    //Helper.addDictionaryItem(Helper.dctTempToHumidity, line, Helper.dctLightToTemp);
                    Helper.lstHumidity.Add(line);
                    break;

                case "Location":
                    Console.WriteLine("Processing Location");
                    //Helper.addDictionaryItem(Helper.dctHumidityToLocation, line, Helper.dctTempToHumidity);
                    Helper.lstLocation.Add(line);
                    break;

                default:
                    break;
            }
        }

        // Find lowest location id Part 1

        decimal iLocation = 0;
        bool bLocationSet = false;

        foreach (decimal dSeed in Helper.lstSeeds)
        {
            Decimal dSoil = Helper.checkDictionary(dSeed, Helper.lstSoil);
            Decimal dFert = Helper.checkDictionary(dSoil, Helper.lstFert);
            Decimal dWater = Helper.checkDictionary(dFert, Helper.lstWater);
            Decimal dLight = Helper.checkDictionary(dWater, Helper.lstLight);
            Decimal dTemp = Helper.checkDictionary(dLight, Helper.lstTemp);
            Decimal dHum = Helper.checkDictionary(dTemp, Helper.lstHumidity);
            Decimal dlocation = Helper.checkDictionary(dHum, Helper.lstLocation);

            if (!bLocationSet)
            {
                iLocation = dlocation;
                bLocationSet = true;
            }
            else if (dlocation < iLocation)
            {
                iLocation = dlocation;
            }

            //Seed 79, soil 81, fertilizer 81, water 81, light 74, temperature 78, humidity 78, location 82.
            //Seed 14, soil 14, fertilizer 53, water 49, light 42, temperature 42, humidity 43, location 43.
            //Seed 55, soil 57, fertilizer 57, water 53, light 46, temperature 82, humidity 82, location 86.
            //Seed 13, soil 13, fertilizer 52, water 41, light 34, temperature 34, humidity 35, location 35.

        }

        if(iLocation == 462648396)
        {
            Console.WriteLine("All Good");
        }

        Console.WriteLine("Minimum location is: " + iLocation);



        // Part 2

        iLocation = 0;
        bLocationSet = false;
        List<decimal> lstAttempts = new List<decimal>();

        lstAttempts.Add(10000000); // 10 million
        lstAttempts.Add(9000000); // 9 million
        lstAttempts.Add(8000000); // 8 million
        lstAttempts.Add(7000000); // 7 million
        lstAttempts.Add(6000000); // 6 million
        lstAttempts.Add(5000000); // 5 million
        lstAttempts.Add(4000000); // 4 million
        lstAttempts.Add(3000000); // 3 million
        lstAttempts.Add(2000000); // 2 million
        lstAttempts.Add(1000000); // 1 million

        lstAttempts.Add(500000); // 500k million
        lstAttempts.Add(250000); // 250k million
        lstAttempts.Add(100000); // 100k million

        lstAttempts.Add(10000); // 10k
        lstAttempts.Add(5000); // 5k 
        lstAttempts.Add(1000); // 1k 
        lstAttempts.Add(500); // 500
        lstAttempts.Add(250); // 250
        lstAttempts.Add(100); // 100
        lstAttempts.Add(10); // 50


        foreach (Seed objSeed in Helper.lstSeedObjects)
        {
            decimal dRange = objSeed.Range;
            decimal dSeed = objSeed.FirstSeed;

            while (dRange > 0)
            {
                decimal dLocation = Helper.getLocation(dSeed);
                bool bSameBucket = false;

                foreach(decimal dAttempt in lstAttempts)
                {
                    if(Helper.trySeedRange(dSeed, dLocation, dAttempt))
                    {
                        dSeed  += dAttempt;
                        dRange -= dAttempt;
                        bSameBucket = true;
                        break;
                    }
                }

                if (!bLocationSet)
                {
                    iLocation = dLocation;
                    bLocationSet = true;
                }
                else if (dLocation < iLocation)
                {
                    iLocation = dLocation;
                }

                if (!bSameBucket)
                {
                    dSeed++;
                    dRange--;
                }

                //Seed 79, soil 81, fertilizer 81, water 81, light 74, temperature 78, humidity 78, location 82.
                //Seed 14, soil 14, fertilizer 53, water 49, light 42, temperature 42, humidity 43, location 43.
                //Seed 55, soil 57, fertilizer 57, water 53, light 46, temperature 82, humidity 82, location 86.
                //Seed 13, soil 13, fertilizer 52, water 41, light 34, temperature 34, humidity 35, location 35.
            }

        }

        if (iLocation == 462648396)
        {
            Console.WriteLine("All Good");
        }

        Console.WriteLine("Minimum location is: " + iLocation);

    }
    else
    {
        Console.WriteLine("File not found.");
    }

}
catch(Exception ex)
{
    Console.Write(ex.Message);
}

public class Seed
{
    public decimal FirstSeed;
    public decimal Range;

    public Seed(decimal dFirst, decimal dRange)
    {
        FirstSeed = dFirst;
        Range = dRange;
    }
}

public static class Helper
{
    public static List<decimal> lstSeeds = new List<decimal>();
    public static List<Seed> lstSeedObjects = new List<Seed>();

    public static List<string> lstSoil = new List<string>();
    public static List<string> lstFert = new List<string>();
    public static List<string> lstWater = new List<string>();
    public static List<string> lstLight = new List<string>();
    public static List<string> lstTemp = new List<string>();
    public static List<string> lstHumidity = new List<string>();
    public static List<string> lstLocation = new List<string>();

    public static bool trySeedRange(decimal dSeed, decimal dLocation, decimal dAttempt)
    {
        decimal dForwardSeed = dSeed + dAttempt;

        decimal dForwardLocation = getLocation(dForwardSeed);

        decimal dDifference = dForwardLocation - dLocation;

        if (dDifference == dAttempt)
        {
            return true;
        }
        return false;
    }

    public static decimal getLocation(decimal dSeed)
    {
        Decimal dSoil = Helper.checkDictionary(dSeed, lstSoil);
        Decimal dFert = Helper.checkDictionary(dSoil, lstFert);
        Decimal dWater = Helper.checkDictionary(dFert, lstWater);
        Decimal dLight = Helper.checkDictionary(dWater, lstLight);
        Decimal dTemp = Helper.checkDictionary(dLight, lstTemp);
        Decimal dHum = Helper.checkDictionary(dTemp, lstHumidity);
        Decimal dlocation = Helper.checkDictionary(dHum, lstLocation);

        return dlocation;
    }

    public static decimal checkDictionary(decimal dKey, List<string> lstItems)
    {
        foreach(string strLine in lstItems)
        {
            if (string.IsNullOrEmpty(strLine))
            {
                continue;
            }

            string[] astrData = strLine.Split(' ');

            // if SeedToSoil
            // 1 is seed
            // 2 is Soil

            decimal iItem1 = decimal.Parse(astrData[1]);
            decimal iItem2 = decimal.Parse(astrData[0]);
            decimal iRange = decimal.Parse(astrData[2]);

            // The seed only get incremented so if smaller than range continue
            if (dKey < iItem1)
            {
                continue;
            }

            decimal iMaxKey = iItem1 + iRange - 1;

            // Not in this range
            if(dKey > iMaxKey)
            {
                continue;
            }

            return iItem2 + (dKey - iItem1);
        }

        // If not in range then return seed number
        return dKey;
    }

    public static void addSeeds(string strLine)
    {
        if (string.IsNullOrEmpty(strLine))
        {
            return;
        }

        string[] astrData = strLine.Split(' ');

        foreach(string strSeed in astrData)
        {
            decimal dResult;
            if (decimal.TryParse(strSeed, out dResult))
            {
                // Conversion successful
                lstSeeds.Add(dResult);
            }
        }

        for(int iLoop = 0; iLoop < lstSeeds.Count; iLoop+=2)
        {
            Seed objSeed = new Seed(lstSeeds[iLoop], lstSeeds[iLoop + 1]);
            lstSeedObjects.Add(objSeed);
        }
    }
}