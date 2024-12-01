using System;
using System.Collections.Generic;
using NUnit.Framework;
using PikUpStix.Trading.Forecast;
using PikUpStix.Trading.Common.Enums;
using PikUpStix.Trading.Data.Local.SqlDb;

namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    public class PositionAdjusterTests
    {
        [Test]
        public void ExistingPositionAdjustments()
        {
            WhereNewPositionEqualsSingleOldPosition();
            WhereNewPositionLargerLong();
            WhereNewPositionLargerShort();
            WhereNewPositionReverseToShort();
            WhereNewPositionReverseToLong();
            WhereNewLongPositionAggregatesOldAndNew();
            WhereOnePositionMatchesRequirement();
        }

        

        private void TestPositionCorrect(decimal proposedPosition, decimal expectedAdditionalPositionSize, List<Test_Trades> existingPositions)
        {
            var positionAdjuster = new PositionCalculator(proposedPosition, existingPositions);
            Assert.AreEqual(proposedPosition, positionAdjuster.FinalPosition);
            //Assert.AreEqual(expectedAdditionalPositionSize, positionAdjuster.RequiredAdjustmentToVolume);
        }
        private void WhereNewPositionEqualsSingleOldPosition()
        {
            var proposedPosition = 5;
            var listOfExistingPositions = new List<Test_Trades>()
            {
                new Test_Trades()
                {
                    Volume = 5,
                    Direction = PositionType.BUY.ToString()
                }
            };
            var requiredNewPositionAdjustment = 0M;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }
        private void WhereNewPositionLargerShort()
        {
            var proposedPosition = -6;
            var listOfExistingPositions = new List<Test_Trades>()
            {
                new Test_Trades()
                {
                    Volume = -5,
                    Direction = PositionType.SELL.ToString()
                }
            };
            var requiredNewPositionAdjustment = -1M;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }

        private void WhereNewPositionLargerLong()
        {
            var proposedPosition = 6;
            var listOfExistingPositions = new List<Test_Trades>()
            {
                new Test_Trades()
                {
                    Volume = 5,
                    Direction = PositionType.BUY.ToString()
                }
            };
            var requiredNewPositionAdjustment = 1M;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }
        private void WhereNewLongPositionAggregatesOldAndNew()
        {
            var proposedPosition = 20;
            var listOfExistingPositions = new List<Test_Trades>()
            {
                new Test_Trades()
                {
                    Volume = 1,
                    Direction = PositionType.BUY.ToString()
                },
                new Test_Trades()
                {
                    Volume = 4,
                    Direction = PositionType.BUY.ToString()
                },
                new Test_Trades()
                {
                    Volume = 6,
                    Direction = PositionType.BUY.ToString()
                },
                new Test_Trades()
                {
                    Volume = 21,
                    Direction = PositionType.BUY.ToString()
                }
            };
            var newPositionSize = 9M;  // Note Test_Trade with volume of 21 is removed as it's too large.  Then 9 more contracts required to balance
            TestPositionCorrect(proposedPosition, newPositionSize, listOfExistingPositions);
        }

        private void WhereNewLongPositionAggregatesOldAndNewReOrdered()
        {
            var proposedPosition = 20;
            var listOfExistingPositions = new List<Test_Trades>()
            {
                new Test_Trades()
                {
                    Volume = 1,
                    Direction = PositionType.BUY.ToString()
                },
                new Test_Trades()
                {
                    Volume = 4,
                    Direction = PositionType.BUY.ToString()
                },
                new Test_Trades()
                {
                    Volume = 21,
                    Direction = PositionType.BUY.ToString()
                },
                new Test_Trades()
                {
                    Volume = 6,
                    Direction = PositionType.BUY.ToString()
                }
            };
            var newPositionSize = 9M;  // Note Test_Trade with volume of 21 is removed as it's too large.  Then 9 more contracts required to balance
            TestPositionCorrect(proposedPosition, newPositionSize, listOfExistingPositions);
        }

        private void WhereOnePositionMatchesRequirement()
        {
            var proposedPosition = 21;
            var listOfExistingPositions = new List<Test_Trades>()
            {
                new Test_Trades()
                {
                    Volume = 1,
                    Direction = PositionType.BUY.ToString()
                },
                new Test_Trades()
                {
                    Volume = 4,
                    Direction = PositionType.BUY.ToString()
                },
                new Test_Trades()
                {
                    Volume = 6,
                    Direction = PositionType.BUY.ToString()
                },
                new Test_Trades()
                {
                    Volume = 21,
                    Direction = PositionType.BUY.ToString()
                }
            };
            var requiredNewPositionAdjustment = 0M;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }

        private void WhereNewPositionReverseToLong()
        {
            var proposedPosition = 6;
            var listOfExistingPositions = new List<Test_Trades>()
            {
                new Test_Trades()
                {
                    Volume = -5,
                    Direction = PositionType.SELL.ToString()
                }
            };
            var requiredNewPositionAdjustment = 11M;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }

        private void WhereNewPositionReverseToShort()
        {
            var proposedPosition = -6;
            var listOfExistingPositions = new List<Test_Trades>()
            {
                new Test_Trades()
                {
                    Volume = 5,
                    Direction = PositionType.BUY.ToString()
                }
            };
            var requiredNewPositionAdjustment = -11M;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }
    }
}