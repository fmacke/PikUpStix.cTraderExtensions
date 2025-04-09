using Domain.Entities;
using Domain.Enums;
using PikUpStix.Trading.Forecast;

namespace Application.Tests
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

        

        private void TestPositionCorrect(double proposedPosition, double expectedAdditionalPositionSize, List<Position> existingPositions)
        {
            var positionAdjuster = new PositionCalculator(proposedPosition, existingPositions);
            Assert.AreEqual(proposedPosition, positionAdjuster.FinalPosition);
            //Assert.AreEqual(expectedAdditionalPositionSize, positionAdjuster.RequiredAdjustmentToVolume);
        }
        private void WhereNewPositionEqualsSingleOldPosition()
        {
            var proposedPosition = 5;
            var listOfExistingPositions = new List<Position>()
            {
                new Position()
                {
                    Volume = 5,
                    PositionType = PositionType.BUY
                }
            };
            var requiredNewPositionAdjustment = 0;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }
        private void WhereNewPositionLargerShort()
        {
            var proposedPosition = -6;
            var listOfExistingPositions = new List<Position>()
            {
                new Position()
                {
                    Volume = -5,
                    PositionType = PositionType.SELL
                }
            };
            var requiredNewPositionAdjustment = -1;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }

        private void WhereNewPositionLargerLong()
        {
            var proposedPosition = 6;
            var listOfExistingPositions = new List<Position>()
            {
                new Position()
                {
                    Volume = 5,
                    PositionType = PositionType.BUY
                }
            };
            var requiredNewPositionAdjustment = 1;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }
        private void WhereNewLongPositionAggregatesOldAndNew()
        {
            var proposedPosition = 20;
            var listOfExistingPositions = new List<Position>()
            {
                new Position()
                {
                    Volume = 1,
                    PositionType = PositionType.BUY
                },
                new Position()
                {
                    Volume = 4,
                    PositionType = PositionType.BUY
                },
                new Position()
                {
                    Volume = 6,
                    PositionType = PositionType.BUY
                },
                new Position()
                {
                    Volume = 21,
                    PositionType = PositionType.BUY
                }
            };
            var newPositionSize = 9;  // Note Test_Trade with volume of 21 is removed as it's too large.  Then 9 more contracts required to balance
            TestPositionCorrect(proposedPosition, newPositionSize, listOfExistingPositions);
        }

        private void WhereNewLongPositionAggregatesOldAndNewReOrdered()
        {
            var proposedPosition = 20;
            var listOfExistingPositions = new List<Position>()
            {
                new Position()
                {
                    Volume = 1,
                    PositionType = PositionType.BUY
                },
                new Position()
                {
                    Volume = 4,
                    PositionType = PositionType.BUY
                },
                new Position()
                {
                    Volume = 21,
                    PositionType = PositionType.BUY
                },
                new Position()
                {
                    Volume = 6,
                    PositionType = PositionType.BUY
                }
            };
            var newPositionSize = 9;  // Note Test_Trade with volume of 21 is removed as it's too large.  Then 9 more contracts required to balance
            TestPositionCorrect(proposedPosition, newPositionSize, listOfExistingPositions);
        }

        private void WhereOnePositionMatchesRequirement()
        {
            var proposedPosition = 21;
            var listOfExistingPositions = new List<Position>()
            {
                new Position()
                {
                    Volume = 1,
                    PositionType = PositionType.BUY
                },
                new Position()
                {
                    Volume = 4,
                    PositionType = PositionType.BUY
                },
                new Position()
                {
                    Volume = 6,
                    PositionType = PositionType.BUY
                },
                new Position()
                {
                    Volume = 21,
                    PositionType = PositionType.BUY
                }
            };
            var requiredNewPositionAdjustment = 0;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }

        private void WhereNewPositionReverseToLong()
        {
            var proposedPosition = 6;
            var listOfExistingPositions = new List<Position>()
            {
                new Position()
                {
                    Volume = -5,
                    PositionType = PositionType.SELL
                }
            };
            var requiredNewPositionAdjustment = 11;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }

        private void WhereNewPositionReverseToShort()
        {
            var proposedPosition = -6;
            var listOfExistingPositions = new List<Position>()
            {
                new Position()
                {
                    Volume = 5,
                    PositionType = PositionType.BUY
                }
            };
            var requiredNewPositionAdjustment = -11;
            TestPositionCorrect(proposedPosition, requiredNewPositionAdjustment, listOfExistingPositions);
        }
    }
}