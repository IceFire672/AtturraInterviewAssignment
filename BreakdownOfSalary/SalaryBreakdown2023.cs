using System;

namespace BreakdownOfSalary
{
    static class SalaryBreakdown
    {
        private const int MEDICARE_LEVY_THRESHOLD = 21335;
        private const int MEDICARE_LEVY_MAX = 26668;
        private const int BUDGET_REPAIR_LEVY_THRESHOLD = 180000;
        private const decimal BUDGET_REPAIR_LEVY_RATE = 0.02m;
        private const decimal MEDICARE_LEVY_MID_RATE = 0.1m;
        private const decimal MEDICARE_LEVY_MAX_RATE = 0.02m;

        static void Main(string[] args)
        {
            Console.WriteLine(" ");
            int salaryPackage = GetSalaryPackage();
            string paymentFrequency = GetPaymentFrequency();

            Console.WriteLine(" ");
            Console.WriteLine("Gross package: " + string.Format("{0:C}", salaryPackage));
            decimal superannuation = CalculateSuperannuation(salaryPackage);
            Console.WriteLine("Superannuation: " + string.Format("{0:C}", superannuation));

            Console.WriteLine(" ");
            decimal taxableIncome = CalculateTaxableIncome(salaryPackage, superannuation);
            Console.WriteLine("Taxable Income: " + string.Format("{0:C}", taxableIncome));
            taxableIncome = Math.Floor(taxableIncome);

            Console.WriteLine(" ");
            Console.WriteLine("Deductions: ");
            int medicareLevy = CalculateMedicareLevy(taxableIncome);
            Console.WriteLine("Medicare Levy: " + string.Format("{0:C}", medicareLevy));
            int budgetRepairLevy = CalculateBudgetRepairLevy(taxableIncome);
            Console.WriteLine("Budget Repair Levy: " + string.Format("{0:C}", budgetRepairLevy));
            decimal incomeTax = CalculateIncomeTax(taxableIncome);
            Console.WriteLine("Income Tax: " + string.Format("{0:C}", incomeTax));

            Console.WriteLine(" ");
            decimal netIncome = CalculateNetIncome(salaryPackage, superannuation, medicareLevy, budgetRepairLevy, incomeTax);
            Console.WriteLine("Net income: " + string.Format("{0:C}", netIncome));
            decimal payPacketAmount = CalculatePayPacketAmount(paymentFrequency, netIncome);
            Console.WriteLine("Pay packet: " + string.Format("{0:C}", payPacketAmount) + PayPacketAmountFrequency(paymentFrequency));

            Console.WriteLine(" ");
            Console.WriteLine("Press any key to end...");
            Console.ReadKey();
            
        }

        static int GetSalaryPackage()
        {
            int salaryPackage;
            while (true)
            {
                Console.Write("Enter your salary package amount: ");
                if (int.TryParse(Console.ReadLine(), out salaryPackage))
                {
                    return salaryPackage;
                }
                Console.WriteLine("Invalid input.");
            }
        } // end of GetSalaryPackage() method

        static string GetPaymentFrequency()
        {
            string? paymentFrequency;
            
            while (true)
            {
                Console.Write("Enter payment frequency (W for weekly, F for fortnightly, M for monthly): ");
                
                paymentFrequency = Console.ReadLine();

                if(paymentFrequency != null)
                {
                    paymentFrequency = paymentFrequency.ToLower();
                    if (paymentFrequency == "w" || paymentFrequency == "f" || paymentFrequency == "m")
                    {
                        return paymentFrequency;
                    }
                    else 
                    {
                        Console.WriteLine("Invalid input.");
                    }   
                }
                else
                {
                    throw new Exception("Invalid input cannot be null.");
                }
            }
        } // end of GetPaymentFrequency() method

        public static decimal CalculateSuperannuation(int salaryPackage)
        {
        
            decimal taxableIncome = salaryPackage / 1.095m;
            decimal superannuation = salaryPackage - taxableIncome;
            return Math.Round(superannuation, 2, MidpointRounding.AwayFromZero);
    
        } // end of CalculateSuperannuation() method

        public static decimal CalculateTaxableIncome(int salaryPackage, decimal superannuation)
        {
            decimal taxableIncome = salaryPackage - superannuation;
            return taxableIncome;
        }// end of CalculateTaxableIncome() method

        public static int CalculateMedicareLevy(decimal taxableIncome)
        {
            int medicareLevy;
            if (taxableIncome <= MEDICARE_LEVY_THRESHOLD)
            {
                medicareLevy = 0;
            }
            else if (taxableIncome <= MEDICARE_LEVY_MAX)
            {
                medicareLevy = (int)Math.Ceiling((taxableIncome - MEDICARE_LEVY_THRESHOLD) * MEDICARE_LEVY_MID_RATE);
            }
            else
            {
                medicareLevy = (int)Math.Ceiling(taxableIncome * MEDICARE_LEVY_MAX_RATE);
            }
            return medicareLevy;

        }// end of CalculateMedicareLevy() method

        public static int CalculateBudgetRepairLevy(decimal taxableIncome)
        {
            int budgetRepairLevy;
            if (taxableIncome <= BUDGET_REPAIR_LEVY_THRESHOLD)
            {
                budgetRepairLevy = 0;
            }
            else
            {
                budgetRepairLevy = (int)Math.Ceiling((taxableIncome - BUDGET_REPAIR_LEVY_THRESHOLD) * BUDGET_REPAIR_LEVY_RATE);
            }
            return budgetRepairLevy;
            
        }// end of CalculateBudgetRepairLevy() method

        public static decimal CalculateIncomeTax(decimal taxableIncome)
        {
            decimal incomeTax;
            if (taxableIncome <= 18200)
            {
                incomeTax = 0;
            }
            else if (taxableIncome <= 37000)
            {
                incomeTax = Math.Ceiling((taxableIncome - 18200) * 0.19m);
            }
            else if (taxableIncome <= 87000)
            {
                incomeTax = Math.Ceiling(3572 + (taxableIncome - 37000) * 0.325m);
            }
            else if (taxableIncome <= 180000)
            {
                incomeTax = Math.Ceiling(19822 + (taxableIncome - 87000) * 0.37m);
            }
            else
            {
                incomeTax = Math.Ceiling(54232 + (taxableIncome - 180000) * 0.47m);
            }
            return incomeTax;

        }// end of CalculateIncomeTax() method

        public static decimal CalculateNetIncome(int grossPackage, decimal superannuationContribution, int medicareLevy, int budgetRepairLevy, decimal incomeTax)
        {
            decimal netIncome = grossPackage - superannuationContribution - medicareLevy - budgetRepairLevy - incomeTax;
            return netIncome;

        } // end of CalculateNetIncome() method

        public static decimal CalculatePayPacketAmount(string paymentFrequency, decimal netIncome)
        {
            
            decimal payPacketAmount = 0;
            int payPeriodsPerYear = 0;
            if (paymentFrequency == "w")
            {
                payPeriodsPerYear = 52;
            }
            else if (paymentFrequency == "f")
            {
                payPeriodsPerYear = 26;
            }
            else if (paymentFrequency == "m")
            {
                payPeriodsPerYear = 12;
            }
            else
            {
                Console.WriteLine("Invalid pay frequency");
                return payPacketAmount;
            }

            payPacketAmount = netIncome / payPeriodsPerYear;
            payPacketAmount = Math.Ceiling(payPacketAmount * 100) / 100;
            return payPacketAmount;

        }// end of CalculatePayPacketAmount() method

        public static string PayPacketAmountFrequency(string paymentFrequency)
        {
            if (paymentFrequency == "w") 
            {
                paymentFrequency = " per week";
                return paymentFrequency;
            } 
            else if (paymentFrequency == "f") 
            {
                paymentFrequency = " per fortnight";
                return paymentFrequency;
            } 
            else 
            {
                paymentFrequency = " per month";
                return paymentFrequency;
            }
        }// end of PayPacketAmountFrequency() method

    } //end of class 
}

