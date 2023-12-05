// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;

List<string> lstSeeds = new List<string>();
List<string> lstSoil = new List<string>();
List<string> lstFert = new List<string>();
List<string> lstWater = new List<string>();
List<string> lstLight = new List<string>();
List<string> lstTemp = new List<string>();
List<string> lstHumidity = new List<string>();
List<string> lstLocation = new List<string>();

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
                    lstSeeds.Add(line);
                    break;

                case "Soil":
                    Console.WriteLine("Processing Soil");
                    //Helper.addDictionaryItem(Helper.dctSeedToSoil, line, Helper.dctSeeds);
                    lstSoil.Add(line);
                    break;

                case "Fertlizer":
                    Console.WriteLine("Processing fertilizer");
                    //Helper.addDictionaryItem(Helper.dctSoilToFertilizer, line, Helper.dctSeedToSoil);
                    lstFert.Add(line);
                    break;

                case "Water":
                    Console.WriteLine("Processing Water");
                    //Helper.addDictionaryItem(Helper.dctFertlizerToWater, line, Helper.dctSoilToFertilizer);
                    lstWater.Add(line);
                    break;

                case "Light":
                    Console.WriteLine("Processing Light");
                    //Helper.addDictionaryItem(Helper.dctWaterToLight, line, Helper.dctFertlizerToWater);
                    lstLight.Add(line);
                    break;

                case "Temperature":
                    Console.WriteLine("Processing Temperature");
                    //Helper.addDictionaryItem(Helper.dctLightToTemp, line, Helper.dctWaterToLight);
                    lstTemp.Add(line);
                    break;

                case "Humidity":
                    Console.WriteLine("Processing Humidity");
                    //Helper.addDictionaryItem(Helper.dctTempToHumidity, line, Helper.dctLightToTemp);
                    lstHumidity.Add(line);
                    break;

                case "Location":
                    Console.WriteLine("Processing Location");
                    //Helper.addDictionaryItem(Helper.dctHumidityToLocation, line, Helper.dctTempToHumidity);
                    lstLocation.Add(line);
                    break;

                default:
                    break;
            }
        }

        // Find lowest location id

        decimal iLocation = 0;
        bool bLocationSet = false;

        foreach (decimal dSeed in Helper.lstSeeds)
        {
            Decimal dSoil = Helper.checkDictionary(dSeed, lstSoil);
            Decimal dFert = Helper.checkDictionary(dSoil, lstFert);
            Decimal dWater = Helper.checkDictionary(dFert, lstWater);
            Decimal dLight = Helper.checkDictionary(dWater, lstLight);
            Decimal dTemp = Helper.checkDictionary(dLight, lstTemp);
            Decimal dHum = Helper.checkDictionary(dTemp, lstHumidity);
            Decimal dlocation = Helper.checkDictionary(dHum, lstLocation);

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



public static class Helper
{
    public static List<decimal> lstSeeds = new List<decimal>();
    
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
    }
}