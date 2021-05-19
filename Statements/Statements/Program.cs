using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Statements
{
    class Program
    {
        static void Main(string[] args)
        {
            var plays = new List<Play>()
            {
                new Play("hamlet", "Hamlet", PlayTypeEnum.Tragedy),
                new Play("as-like", "As you like IT", PlayTypeEnum.Comedy),
                new Play("othello", "Othello", PlayTypeEnum.Tragedy)
            };

            var invoice = new Invoice()
            {
                CustomerName = "NESS", Performances = new List<Performance>()
                {
                    new Performance() {Audience = 55, PlayId = "hamlet"},
                    new Performance() {Audience = 35, PlayId = "as-like"},
                    new Performance() {Audience = 40, PlayId = "othello"}
                }
            };

            var statement = new Statement();
            var statementDetails = statement.GetInvoice(invoice, plays);
            Console.WriteLine(statementDetails);
        }
    }

    public class Statement
    {
        public string GetInvoice(Invoice invoice, List<Play> plays)
        {
            double totalAmount = 0;
            var result = $"Statement for {invoice.CustomerName} \n";

            double volumeCredits = 0;

            foreach (var perf in invoice.Performances)
            {
                var play = plays.FirstOrDefault(x => x.PlayId == perf.PlayId);
                double thisAmount = GetAmount(play, perf);

                volumeCredits += CalculateCredits(perf, play);

                //print line for this order
                result += GetOrder(play, thisAmount, perf);
                totalAmount += thisAmount;
            }

            result += $"Amount owed is {totalAmount / 100:C} \n";
            result += $"You earned {volumeCredits} credits \n";

            return result;
        }

        private static string GetOrder(Play play, double thisAmount, Performance perf)
        {
            return $"{play.Name}: {thisAmount / 100.0:C} ({perf.Audience} seats) \n";
        }

        private static double CalculateCredits( Performance perf, Play play)
        {
            double volumeCredits = 0;
            //add volume credits 
            volumeCredits += Math.Max(perf.Audience - 30, 0);
            //add extra credit for every ten comedy attendees
            if (PlayTypeEnum.Comedy == play.Type) volumeCredits += Math.Floor(perf.Audience / 5.0);
            return volumeCredits;
        }

        private static double GetAmount(Play play, Performance perf)
        {
            double thisAmount;
            switch (play.Type)
            {
                case PlayTypeEnum.Tragedy:
                    thisAmount = GetAmount(perf,40000, 30, 1000, 0, 0);
                    break;
                case PlayTypeEnum.Comedy:
                    thisAmount = GetAmount(perf, 30000, 20, 500, 10000, 300);
                    break;
                default: throw new Exception($"Unknown type: {play.Type}");
            }

            return thisAmount;
        }

        private static double GetAmount(Performance perf, double initialValue, double audienceSize, int multiplier, int adjustmentRate, int noAudienceRate)
        {
            double thisAmount = initialValue;
            if (perf.Audience > audienceSize)
            {
                thisAmount += adjustmentRate + multiplier * (perf.Audience - audienceSize);
            }

            thisAmount += noAudienceRate * perf.Audience;
            return thisAmount;
        }
    }


    public class Play
    {
        public Play(string playId, string name, PlayTypeEnum type)
        {
            PlayId = playId;
            Name = name;
            Type = type;
        }

        public string PlayId { get; set; }
        public string Name { get; set; }
        public PlayTypeEnum Type { get; set; }
    }

    public enum PlayTypeEnum
    {
        Tragedy,
        Comedy
    }

    public class Invoice
    {
        public string CustomerName { get; set; }
        public List<Performance> Performances { get; set; }
    }

    public class Performance
    {
        public string PlayId { get; set; }
        public int Audience { get; set; }
    }
}