using System;
using System.Collections;

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

    class Program
    {
        /*********************************************************************************/
        /***     Constants                                                             ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/

        const int VALID_PARSE = 0;
        const int INVALID_PARSE = -1;

        const string UOM_EACH = "Each";
        const string UOM_POUND = "Pound";

        const double GLOBAL_TAX_RATE = .075;

        const int NUM_PRODUCTS = 10;

        /*********************************************************************************/
        /***     Global Variables                                                      ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static ArrayList prodList = new ArrayList();

        /*********************************************************************************/
        /***     Product Class Type                                                    ***/
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
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static void loadInputFile()
        {
            int intLineNum = 1;

            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\marty\Source\Repos\Swiftly\Swiftly\input.txt");

            foreach (string line in lines)
            {

                Product newProduct = parseInputString(line);

                if (newProduct == null)
                {
                    Console.WriteLine("Input line #{0} could not be parsed: " + line, intLineNum++);
                }
                else
                {
                    prodList.Add(newProduct);
                }
            }
        }

        /*********************************************************************************/
        /***     Main Routine                                                          ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static void Main(string[] args)
        {
            loadInputFile();

            Console.WriteLine("Number of products loaded: {0}", prodList.Count);
        }
    }
}
