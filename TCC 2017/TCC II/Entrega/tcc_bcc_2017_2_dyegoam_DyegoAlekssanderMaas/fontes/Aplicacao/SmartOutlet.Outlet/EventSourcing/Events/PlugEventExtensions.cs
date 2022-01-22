using System;
using System.Globalization;
using Marten.Events;

namespace SmartOutlet.Outlet.EventSourcing.Events
{
    public static class PlugEventExtensions
    {
        public static EventDescription GetDescription<T>(this Event<T> @event) 
            where T : IPlugEvent
        {
            return DescribeEvent(@event, @event.Data);
        }

        private static EventDescription DescribeEvent<T>(Event<T> martenEvent, IPlugEvent @event)
            where T : IPlugEvent
        {
//            var timestampDescription = martenEvent.Timestamp.ToString("yyyy/MM/dd HH:mm:ss", new CultureInfo("pt-BR"));
            var timestampDescription = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", new CultureInfo("pt-BR"));
            switch (@event)
            {
                case PlugActivated _:
                    return new EventDescription(martenEvent.Sequence, "Plugue ativado", timestampDescription, string.Empty);
                case PlugDeactivated _:
                    return new EventDescription(martenEvent.Sequence, "Plugue desativado", timestampDescription, string.Empty);
                case PlugReactivated _:
                    return new EventDescription(martenEvent.Sequence, "Plugue reativado", timestampDescription, string.Empty);
                case PlugRenamed _:
                    return new EventDescription(martenEvent.Sequence, "Plugue renomeado", timestampDescription, "Novo nome: " + ((PlugRenamed) @event).NewName);
                case PlugTurnedOn _:
                    return new EventDescription(martenEvent.Sequence, "Plugue ligado", timestampDescription, string.Empty);
                case PlugTurnedOff _:
                    return new EventDescription(martenEvent.Sequence, "Plugue desligado", timestampDescription, string.Empty);
                case OperationScheduled agendamento:
                    var description = agendamento.Type == CommandType.TurnOn
                        ? $"Ligar em {(int) agendamento.TimeInFuture.TotalSeconds}s ({DateTime.Now + agendamento.TimeInFuture:yyyy/MM/dd HH:mm:ss})"
                        : $"Desligar em {(int) agendamento.TimeInFuture.TotalSeconds}s ({DateTime.Now + agendamento.TimeInFuture:yyyy/MM/dd HH:mm:ss})";
//                        ? $"Ligar em {(int) agendamento.TimeInFuture.TotalSeconds}s ({martenEvent.Timestamp + agendamento.TimeInFuture:yyyy/MM/dd HH:mm:ss})"
//                        : $"Desligar em {(int) agendamento.TimeInFuture.TotalSeconds}s ({martenEvent.Timestamp + agendamento.TimeInFuture:yyyy/MM/dd HH:mm:ss})"; 
                    return new EventDescription(martenEvent.Sequence, "Ação agendada", timestampDescription, description);
            }

            return null;
        }
    }
}
