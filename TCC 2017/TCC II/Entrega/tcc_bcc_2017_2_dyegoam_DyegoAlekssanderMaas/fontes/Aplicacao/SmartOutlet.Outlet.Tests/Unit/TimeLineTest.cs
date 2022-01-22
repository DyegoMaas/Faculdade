//using System;
//using System.Linq;
//using FluentAssertions;
//using Marten.Events;
//using NUnit.Framework;
//using SmartOutlet.Outlet.EventSourcing.Events;
//using SmartOutlet.Outlet.EventSourcing.ProjectionViews;
//
//namespace SmartOutlet.Outlet.Tests.Unit
//{
//    public class TimeLineTest
//    {
//        private TimeLine _timeLine;
//
//        [SetUp]
//        public void SetUp()
//        {
//            _timeLine = new TimeLine();
//        }
//        
//        [Test]
//        public void describing_a_plug_activation()
//        {
//            var activation = new PlugActivated(plugId:Guid.NewGuid(), plugName:"Sanduicheira");
//            
//            _timeLine.Apply(new Event<PlugActivated>(activation));
//
//            _timeLine.Id.Should().Be(activation.PlugId);
//
//            var events = _timeLine.EventDescriptions;
//            events.Should().Contain(x => x.Title.EndsWith("Plugue ativado"));
//        }
//        
//        [Test]
//        public void describing_a_plug_turning_on()
//        {
//            _timeLine.Apply(new Event<PlugTurnedOn>(new PlugTurnedOn())
//            {
//                Timestamp = NewTimestamp(2017, 10, 10, 17.Hours(10.Minutes(5.Seconds())))
//            });
//
//            var events = _timeLine.EventDescriptions;
//            events.Should().Contain("2017/10/10 17:10:05 - Plugue ligado");
//        }
//
//        private static DateTimeOffset NewTimestamp(int year, int month, int day, TimeSpan time)
//        {
//            return new DateTimeOffset(year, month, day, time.Hours, time.Minutes, time.Seconds, TimeSpan.Zero);
//        }
//
//        [Test]
//        public void describing_a_plug_turning_off()
//        {
//            _timeLine.Apply(new Event<PlugTurnedOff>(new PlugTurnedOff())
//            {
//                Timestamp = NewTimestamp(2017, 10, 10, 17.Hours(10.Minutes(5.Seconds())))
//            });
//
//            var events = _timeLine.EventDescriptions;
//            events.Should().Contain("2017/10/10 17:10:05 - Plugue desligado");
//        }
//        
//        [Test]
//        public void describing_a_plug_renamed()
//        {
//            _timeLine.Apply(new Event<PlugRenamed>(new PlugRenamed("Lala"))
//            {
//                Timestamp = NewTimestamp(2017, 10, 10, 17.Hours(10.Minutes(5.Seconds())))
//            });
//
//            var events = _timeLine.EventDescriptions;
//            events.Should().Contain(x => x.EndsWith("Plugue renomeado para Lala"));
//        }
//        
//        [TestCase(CommandType.TurnOn, "Ligar")]
//        [TestCase(CommandType.TurnOff, "Desligar")]
//        public void describing_an_operation_scheduled(CommandType type, string descricaoEsperadaComando)
//        {
//            _timeLine.Apply(new Event<OperationScheduled>(new OperationScheduled(type, timeInFuture:15.Minutes()))
//            {
//                Timestamp = NewTimestamp(2017, 10, 10, 17.Hours(10.Minutes(5.Seconds())))
//            });
//
//            var events = _timeLine.EventDescriptions;
//            events.First().Should().EndWith($"{descricaoEsperadaComando} em 900s (2017/10/10 17:25:05)");
//        }
//    }
//}