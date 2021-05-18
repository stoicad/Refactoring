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
            var totalAmount = 0;
            double volumeCredits = 0;
            var result = $"Statement for {invoice.CustomerName} \n";
            //string.Format("{0:c}", 112.236677) // $112.23 - defaults to system
            var format = "{ 0:C}";

            foreach (var perf in invoice.Performances)
            {
                var thisAmount = 0;
                var play = plays.FirstOrDefault(x => x.PlayId == perf.PlayId);
                switch (play.Type)
                {
                    case PlayTypeEnum.Tragedy:
                        thisAmount = 40000;
                        if (perf.Audience > 30)
                        {
                            thisAmount += 1000 * (perf.Audience - 30);
                        }

                        break;
                    case PlayTypeEnum.Comedy:
                        thisAmount = 30000;
                        if (perf.Audience > 20)
                        {
                            thisAmount += 10000 + 500 * (perf.Audience - 20);
                        }

                        thisAmount += 300 * perf.Audience;
                        break;
                    default: throw new Exception($"Unknown type: {play.Type}");
                }

                //add volume credits 
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                //add extra credit for every ten comedy attendees
                if (PlayTypeEnum.Comedy == play.Type) volumeCredits += Math.Floor(perf.Audience / 5.0);

                //print line for this order
                result += $"{play.Name}: {string.Format(format, thisAmount / 100.0)} ({perf.Audience} seats) \n";
                totalAmount += thisAmount;
            }

            result += $"Amount owed is {string.Format(format, totalAmount / 100)} \n";
            result += $"You earned {volumeCredits} credits \n";

            return result;
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