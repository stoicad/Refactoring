using System;
using System.Collections.Generic;
using System.Linq;
using Statements.Models;

namespace Statements
{
    public class Statement
    {
        public string GetInvoice(Invoice invoice, List<Play> plays)
        {
            var performanceDetailsList = CalculatePerformancesDetails(invoice.Performances, plays);
            return GenerateStatement(performanceDetailsList, invoice.CustomerName);
        }

        private static IList<PerformanceDetails> CalculatePerformancesDetails(List<Performance> performances,
            List<Play> plays)
            => (from perf in performances
                let play = plays.First(x => x.PlayId == perf.PlayId)
                let credits = CalculateCredits(perf.Audience, play.Type)
                let amount = GetAmount(play.Type, perf.Audience)
                select new PerformanceDetails()
                {
                    Credits = credits,
                    Audience = perf.Audience,
                    Amount = amount,
                    PlayTypeEnum = play.Type,
                    PlayName = play.Name
                }).ToList();

        private static double CalculateCredits(int audience, PlayTypeEnum playType)
        {
            double valueCredits = 0;
            valueCredits += Math.Max(audience - 30, 0);
            if (PlayTypeEnum.Comedy == playType) valueCredits += Math.Floor(audience / 10.0);
            return valueCredits;
        }

        private static int GetAmount(PlayTypeEnum playType, int audience)
        {
            int thisAmount;
            switch (playType)
            {
                case PlayTypeEnum.Tragedy:
                    thisAmount = 40000;
                    if (audience > 30)
                    {
                        thisAmount += 1000 * (audience - 30);
                    }

                    break;
                case PlayTypeEnum.Comedy:
                    thisAmount = 30000;
                    if (audience > 20)
                    {
                        thisAmount += 10000 + 500 * (audience - 20);
                    }

                    thisAmount += 300 * audience;
                    break;
                default: throw new Exception($"Unknown type: {playType}");
            }

            return thisAmount;
        }

        private static string GenerateStatement(IEnumerable<PerformanceDetails> performanceDetailsList, string customerName)
        {
            var performanceDetailsEnumerable = performanceDetailsList.ToList();
            var totalAmount = performanceDetailsEnumerable.Select(x => x.Amount).Sum();
            var totalCredits = performanceDetailsEnumerable.Select(x => x.Credits).Sum();

            return GenerateStatementAsText(customerName, performanceDetailsEnumerable, totalAmount, totalCredits);
        }

        private static string GenerateStatementAsText(string customerName, List<PerformanceDetails> performanceDetailsEnumerable,
            double totalAmount, double totalCredits)
        {
            var result = $"Statement for {customerName} \n";

            foreach (var performanceDetails in performanceDetailsEnumerable)
            {
                result +=
                    $"{performanceDetails.PlayName}: {performanceDetails.Amount / 100.0:C} ({performanceDetails.Audience} seats) \n";
            }

            result += $"Amount owed is {totalAmount / 100:C} \n";
            result += $"You earned {totalCredits} credits \n";

            return result;
        }
    }
}