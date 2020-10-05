
using System;
using System.Collections;
using System.Threading;

namespace Swiftly
{
    /***     Copyright 2020 MAJE Software. All Rights Reserved.                            ***/
    /***                                                                                   ***/
    /*** Permission is hereby granted, free of charge, to any person obtaining a copy of   ***/
    /*** this software and associated documentation files (the "Software"), to deal in the ***/
    /*** Software without restriction, including without limitation the rights to use,     ***/
    /*** copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the   ***/
    /*** Software, and to permit persons to whom the Software is furnished to do so,       ***/
    /*** subject to the following conditions:                                              ***/
    /***                                                                                   ***/
    /***                                                                                   ***/
    /*** The above copyright notice and this permission notice shall be included in all    ***/
    /*** copies or substantial portions of the Software.                                   ***/
    /***                                                                                   ***/
    /*** THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR        ***/
    /*** IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS  ***/
    /*** FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR    ***/
    /*** COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN ***/
    /*** AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION   ***/
    /*** WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                   ***/

   // NOTE: Originally, I was going to break up this file into smaller files (proper large-project structuring)
   //        with Partial Classes (pretty standard C#). This file is short enough, though, that 
   //        I decided to leave it as one source file to make it easier for you to read through.

    class Swiftly
    {
        /*********************************************************************************/
        /***     Constants                                                             ***/
        /***                                                                           ***/
        /***     Library: None Used                                                    ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/

        const int VALID_PARSE = 0;
        const int INVALID_PARSE = -1;

        const string UOM_EACH = "Each";
        const string UOM_POUND = "Pound";

        const double GLOBAL_TAX_RATE = .075;

        const bool TRUE = true;

        const int NUM_PRODUCTS = 10;
        //const int SLEEP_PERIOD = 60000;       // Wake up every 60 seconds
        const int SLEEP_PERIOD = 5000;          // Wake up every 5 seconds

        // Error/reporting constants
        const int LOG_NO_LOG = 0;
        const int LOG_FATAL = 32;
        const int LOG_ERROR = 16;
        const int LOG_WARN = 8;
        const int LOG_INFO = 4;
        const int LOG_DEBUG = 2;
        const int LOG_TRACE = 1;

        const int LOG_TRACE_DEBUG_INFO = 7;
        const int ALL_FLAGS = 63;

        // Messaging strings
        // NOTE: For a production application, these should be internationaized/localized
        const string NOTIFY_SLEEP_MSG = "Going to sleep...";
        const string NOTIFY_WAKE_MSG = "Waking up...";
        const string NUM_PROD_LOADED_MSG = "Number of products loaded: ";
        const string CANNOT_LOAD_INPUT_FILE = "ERROR! Cannot load input file: ";
        const string INPUT_FILE_NAME_PATH = "C:\\Users\\marty\\Source\\Repos\\Swiftly\\Swiftly\\input.txt";
        const string INPUT_FILE_LINE_NOT_PARSED = "Input file could not be parsed at line: ";

        /*********************************************************************************/
        /***     Global Variables                                                      ***/
        /***                                                                           ***/
        /***     Library: None Used                                                    ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static ArrayList PROD_LIST = new ArrayList();              // Array list of products
        static int ALL_INFO_LOG = 15;

        static int CURRENT_NOTIFY_LEVEL = ALL_INFO_LOG;            // Current error reporting level

        /*********************************************************************************/
        /***     Product Class Type                                                    ***/
        /***                                                                           ***/
        /***     Library: None Used                                                    ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/

        public class Product
        {
            private string ProdId, ProdDesc;
            private string ProdUOM;
            private string ProdSize;
            private double TaxRate = 0;

            // Constructor
            public Product(string id, string desc, string flags, string productSize)
            {
                // Fields for flags
                bool bPerWeight = false;
                bool bTaxable = false;

                // Figure out and set the flags
                bPerWeight = flags.Substring(2, 1).ToUpper() == "Y";
                bTaxable = flags.Substring(4, 1).ToUpper() == "Y";

                // Set base fields
                this.ProdId = id;
                this.ProdDesc = desc;

                // Set the other fields....

                // Reg display price
                // Reg calc price
                // Promotional Display Price
                // Promotional Calculator price

                // Set the unit of measure
                if (bPerWeight)
                {
                    this.ProdUOM = UOM_POUND;
                }
                else
                {
                    this.ProdUOM = UOM_EACH;
                }

                // Set the product size
                this.ProdSize = productSize;

                // Set the tax rate
                if (bTaxable)
                {
                    this.TaxRate = GLOBAL_TAX_RATE;
                }
            }
        }

        /*********************************************************************************/
        /***     Parse Input String                                                    ***/
        /***                                                                           ***/
        /***     Library: None Used                                                    ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static Product parseInputString(string line)
        {
            // NOTE: This code does NOT test the data types/validity of the input values.
            //       A complete program ought to validate that each variable that is parsed from the full line
            //       is: 1) The proper datatype
            //           2) The proper length
            //           3) Contains the proper of significant digits (decimal types)
            //           4) Contains the proper data values (ex: Flags are "Y" or "N")
            try
            {
                // Set initial decimal values
                decimal singularPrice = 0;
                decimal promoSingularPrice = 0;
                decimal splitPrice = 0;
                decimal promoSplitPrice = 0;

                // Get the product ID and description fields
                string productId = line.Substring(0, 8);
                string productDescription = line.Substring(9, 58).Trim();

                // For the decimal fields, get the number and convert to $$/cents.
                decimal.TryParse(line.Substring(69, 8).ToString(), out singularPrice);
                singularPrice /= 100;
                decimal.TryParse(line.Substring(78, 8).ToString(), out promoSingularPrice);
                promoSingularPrice /= 100;
                decimal.TryParse(line.Substring(87, 8).ToString(), out splitPrice);
                splitPrice /= 100;
                decimal.TryParse(line.Substring(96, 8).ToString(), out promoSplitPrice);
                promoSplitPrice /= 100;

                // Get the other numberic fields, the flags, and the text of the "size"
                string regular = line.Substring(105, 8);
                string promo = line.Substring(114, 8);
                string flags = line.Substring(123, 9);
                string productSize = line.Substring(133, 9).Trim();

                // Return the new product object that we created
                return new Product(productId, productDescription, flags, productSize);
            }
            catch
            {
                return null;
            }
        }

        /*********************************************************************************/
        /***     Load Input Data                                                       ***/
        /***                                                                           ***/
        /***     Library: None Used                                                    ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static void loadInputFile()
        {
            int intLineNum = 1;
            string[] lines = null;

            // Open up and read the lines from the product catalog
            // NOTE: If the catalogs get too big, perhaps parse one line at a time to reduce memory allocation
 
            // NOTE: For this test, we are only loading one input file, which doesn't change (in this case)
            //       For a real load, we should investigate identifying and only loading the delta changes
            try
            {
                lines = System.IO.File.ReadAllLines(@INPUT_FILE_NAME_PATH);
            } catch
            {
                Notify(CANNOT_LOAD_INPUT_FILE + INPUT_FILE_NAME_PATH, ALL_FLAGS);
                return;
            }

            // NOTE: In a real system, we could/should read the store number (and hence the region/store) from either the title
            //        of the input file, or some data near the top of the data file. 
            //
            //       For this simple illustration, there is only one input file and the code has no concern about multiple price files because
            //        this simulation is ALSO not tracking a shopping cart.

            // For all of the prices in the catalog for this store...
            foreach (string line in lines)
            {

                Product newProduct = parseInputString(line);

                if (newProduct == null)
                {
                    Notify(INPUT_FILE_LINE_NOT_PARSED + intLineNum++, ALL_INFO_LOG);
                 }
                else
                {
                    PROD_LIST.Add(newProduct);
                }
            }
        }

        /*********************************************************************************/
        /***     Notify User                                                           ***/
        /***                                                                           ***/
        /***     Library: None Used                                                    ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/ 
        static void Notify(string reportString, int notifyLevel)
        {
            if ((CURRENT_NOTIFY_LEVEL & notifyLevel) > 0)            {
                Console.WriteLine(reportString);
            }
        }

        /*********************************************************************************/
        /***     Main Routine                                                          ***/
        /***                                                                           ***/
        /***     Library: None Used                                                    ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static void Main(string[] args)
        {
            while (TRUE)
            {
                // Each time through we would clean out the memory for the pricelist and load it again....
                PROD_LIST = new ArrayList();

                // Load the input file
                // Write the results
                // *IF* there were any rows not loaded...
                //    Notify the proper IT personnel of this issue.
                // Sleep for the pre-defined amount of time
                //
                loadInputFile();
                Notify(NUM_PROD_LOADED_MSG + PROD_LIST.Count.ToString(), LOG_TRACE_DEBUG_INFO);

                // NOTE: This is only for this quick demo. In the real world we would use an Azure daemon and have it:
                //       1. Start up however often it is configured for
                //       2. Write out data (timestamp) upon successful conclusion
                //       3. Have another daemon process start to insure that this daemon has run in a reasonable timeframe, or generate an error to the users
                //       In this test only, we're just waking up at a pre-defined period

                Notify(NOTIFY_SLEEP_MSG, LOG_TRACE_DEBUG_INFO);
                Thread.Sleep(SLEEP_PERIOD);
                Notify(NOTIFY_WAKE_MSG, LOG_TRACE_DEBUG_INFO);
            }

            // NOTE: For a real product, must also have a memory check to insure no memory leakage
            //   (This would be less of an issue with a proper daemon waking up, processing the data, and terminating
        }
    }
}
