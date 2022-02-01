
using EnergyDataManager.Domain;
using EnergyDataManager.Domain.ValueObjects;
using EnergyDataReader.File;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace given_an_energy_data_manager.domain.tests
{
    public class when_populating_an_account_with_source_meter_readings
    {
        [Theory]
        [InlineData(null)]
        [InlineData("chew")]
        public void then_detect_invalid_account_id(string accountEXP)
        {
            //ARRANGE 
            var sourceMeterReadingsEXP = new List<SourceMeterReading>(){
                new SourceMeterReading(
                    $"{accountEXP}","24/04/2019  09:26:00", "2002")
            };

            //ACT
            Account accountACT =  Account.Create(sourceMeterReadingsEXP);

            //ASSERT
            accountACT.Should().BeNull();
        }


        public static IEnumerable<object[]> meterData =>
            new List<object[]>
            {
                new object[] {
                    new List<SourceMeterReading>(){
                         new SourceMeterReading(
                            "2344", "22/04/2019 09:24:00", "10024") 
                    },
                    1
                },
                new object[] {
                     new List<SourceMeterReading>(){
                    new SourceMeterReading(
                        "2344","24/04/2019 09:26:00", null) 
                     },
                    0
                },
                new object[]{
                     new List<SourceMeterReading>(){
                         new SourceMeterReading(
                             "2344", "07/05/2019 09:24:00", "88888"),
                         new SourceMeterReading(
                             "2344", "07/05/2019 09:24:00", "88888")
                     },
                    2
                },
                new object[]{
                     new List<SourceMeterReading>(){
                         new SourceMeterReading(
                            "2344","18/05/2019 09:24:00", "11154") ,
                         new SourceMeterReading(
                            "2344","18/05/2019  09:24:00", "1115")
                     },
                     1

                }
            };
        
        [Theory]
        [MemberData(nameof(meterData))]
        public void then_disregard_invalid_readings(
            List<SourceMeterReading> meterReadingsEXP,
            int rowsEXP)
        {
            //ACT
            var accountACT = Account.Create(
                 meterReadingsEXP);

            //ASSERT
            accountACT.Readings.Count().Should().Be(rowsEXP);
        }


        [Fact]
        public void then_populate()
        {
            //ARRANGE
            var readings = new List<SourceMeterReading>(){
                new SourceMeterReading(
                    "2344", "22/04/2019 09:24:00", "11002")
            };

            var readingEXP = new MeterReading_VO(
                 "22/04/2019 09:24:00",
                 "11002");

            //ACT
            var accountACT = Account.Create(readings);

            //ASSERT
            accountACT.Readings.Count().Should()
                .Be(readings.Count());

           var meterReadingACT =  accountACT.Readings.First().MeterReading;
            meterReadingACT.Should().BeEquivalentTo(readingEXP);
        }
    }
}
