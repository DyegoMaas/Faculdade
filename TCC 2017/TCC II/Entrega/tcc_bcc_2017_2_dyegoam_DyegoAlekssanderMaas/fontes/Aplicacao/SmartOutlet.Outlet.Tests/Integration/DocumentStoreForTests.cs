using System;
using System.Configuration;
using Marten;
using SmartOutlet.Outlet.EventSourcing.Projections;

namespace SmartOutlet.Outlet.Tests.Integration
{
    public static class DocumentStoreForTests
    {
        public static DocumentStore NewEventSource(params Type[] eventTypes) 
        {
            var store = DocumentStore.For(_ =>
            {
                _.Connection(GetConnectionString());

                _.Events.AddEventTypes(eventTypes);
                _.Events.InlineProjections.AggregateStreamsWith<Plug>();
                _.Events.InlineProjections.AggregateStreamsWith<TimeLine>();
            });
            
            using (var session = store.LightweightSession())
            {
                session.DeleteWhere<Plug>(reading => true);
                session.DeleteWhere<TimeLine>(reading => true);
                session.SaveChanges();
            }
            
            return store;
        }

        private static string GetConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Tests"].ConnectionString;
            return connectionString;
        }
    }
}