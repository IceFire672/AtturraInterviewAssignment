using System;

namespace BreakdownOfSalary
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(" ");
            int salaryPackage = GetSalaryPackage();
            string paymentFrequency = GetPaymentFrequency();
            Console.WriteLine("Gross package: " + string.Format("{0:C}", salaryPackage));
            decimal superannuation = CalculateSuperannuation(salaryPackage);
            Console.WriteLine("Superannuation: " + string.Format("{0:C}", superannuation));

            Console.WriteLine(" ");
            int taxableIncome = CalculateTaxableIncome(salaryPackage, superannuation);
            Console.WriteLine("Taxable Income: " + string.Format("{0:C}", taxableIncome));

            Console.WriteLine(" ");
            Console.WriteLine("Deductions: ");
            int medicareLevy = CalculateMedicareLevy(taxableIncome);
            Console.WriteLine("Medicare Levy: " + string.Format("{0:C}", medicareLevy));
            int budgetRepairLevy = CalculateBudgetRepairLevy(taxableIncome);
            Console.WriteLine("Budget Repair Levy: " + string.Format("{0:C}", budgetRepairLevy));
            int incomeTax = CalculateIncomeTax(taxableIncome);
            Console.WriteLine("Income Tax: " + string.Format("{0:C}", incomeTax));

            Console.WriteLine(" ");
            int netIncome = CalculateNetIncome(salaryPackage, superannuation, medicareLevy, budgetRepairLevy, incomeTax);
            Console.WriteLine("Net income: " + string.Format("{0:C}", netIncome));
            decimal payPacketAmount = CalculatePayPacketAmount(paymentFrequency, netIncome);
            Console.WriteLine("Pay packet: " + string.Format("{0:C}", payPacketAmount));

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
            string paymentFrequency;
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
                    Console.WriteLine("Invalid input.");
                }
                else
                {
                    Console.WriteLine("Invalid input cannot be null.");
                }
            }
        } // end of GetPaymentFrequency() method

        public static decimal CalculateSuperannuation(int salaryPackage)
        {
        
            decimal taxableIncome = salaryPackage / 1.095m;
            decimal superannuation = salaryPackage - taxableIncome;
            return Math.Round(superannuation, 2, MidpointRounding.AwayFromZero);
    
        } // end of CalculateSuperannuation() method

        public static int CalculateTaxableIncome(int salaryPackage, decimal superannuation)
        {
            decimal taxableIncome = salaryPackage - superannuation;
            taxableIncome = Math.Floor(taxableIncome);
            return (int)taxableIncome;
        }// end of CalculateTaxableIncome() method

        public static int CalculateMedicareLevy(int taxableIncome)
        {
            int medicareLevy;
            if (taxableIncome <= 21335)
            {
                medicareLevy = 0;
            }
            else if (taxableIncome <= 26668)
            {
                medicareLevy = (int)Math.Ceiling((taxableIncome - 21335) * 0.1m);
            }
            else
            {
                medicareLevy = (int)Math.Ceiling(taxableIncome * 0.02m);
            }
            return medicareLevy;

        }// end of CalculateMedicareLevy() method

        public static int CalculateBudgetRepairLevy(int taxableIncome)
        {
            int budgetRepairLevy;
            if (taxableIncome <= 180000)
            {
                budgetRepairLevy = 0;
            }
            else
            {
                budgetRepairLevy = (int)Math.Ceiling((taxableIncome - 180000) * 0.02m);
            }
            return budgetRepairLevy;
            
        }// end of CalculateBudgetRepairLevy() method

        public static int CalculateIncomeTax(int taxableIncome)
        {
            int incomeTax;
            if (taxableIncome <= 18200)
            {
                incomeTax = 0;
            }
            else if (taxableIncome <= 37000)
            {
                incomeTax = (int)Math.Ceiling((taxableIncome - 18200) * 0.19m);
            }
            else if (taxableIncome <= 87000)
            {
                incomeTax = (int)Math.Ceiling(3572 + (taxableIncome - 37000) * 0.325m);
            }
            else if (taxableIncome <= 180000)
            {
                incomeTax = (int)Math.Ceiling(19822 + (taxableIncome - 87000) * 0.37m);
            }
            else
            {
                incomeTax = (int)Math.Ceiling(54232 + (taxableIncome - 180000) * 0.47m);
            }
            return incomeTax;

        }// end of CalculateIncomeTax() method

        public static int CalculateNetIncome(int grossPackage, decimal superannuationContribution, int medicareLevy, int budgetRepairLevy, int incomeTax)
        {
            int netIncome = grossPackage - (int)superannuationContribution - medicareLevy - budgetRepairLevy - incomeTax;
            return netIncome;

        } // end of CalculateNetIncome() method

        public static decimal CalculatePayPacketAmount(string paymentFrequency, int netIncome)
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

            payPacketAmount = (decimal)netIncome / payPeriodsPerYear;
            payPacketAmount = Math.Ceiling(payPacketAmount * 100) / 100;
            return payPacketAmount;

        }// end of CalculatePayPacketAmount() method

    } //end of class 
}

