using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Statements.Models;

namespace Statements
{
    class Program
    {
        static void Main(string[] args)
        {
            var plays = new List<Play>()
            {
                new("hamlet", "Hamlet", PlayTypeEnum.Tragedy),
                new("as-like", "As you like IT", PlayTypeEnum.Comedy),
                new("othello", "Othello", PlayTypeEnum.Tragedy)
            };

            var invoice = new Invoice()
            {
                CustomerName = "NESS", Performances = new List<Performance>()
                {
                    new() {Audience = 55, PlayId = "hamlet"},
                    new() {Audience = 35, PlayId = "as-like"},
                    new() {Audience = 40, PlayId = "othello"}
                }
            };

            var statement = new Statement();
            var statementDetails = statement.GetInvoice(invoice, plays);
            Console.WriteLine(statementDetails);
        }
    }
}