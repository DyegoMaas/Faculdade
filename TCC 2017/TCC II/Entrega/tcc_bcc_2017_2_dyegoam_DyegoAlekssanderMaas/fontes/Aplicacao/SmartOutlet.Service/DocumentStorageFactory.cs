using System;
using System.Configuration;
using Marten;

namespace SmartOutlet.Service
{
    public static class DocumentStorageFactory
    {
        public static DocumentStore NewEventSource<TAgreggator>(params Type[] eventTypes) 
            where TAgreggator : class, new()
        {
            var store = DocumentStore.For(_ =>
            {
                _.Connection(GetConnectionString());

                _.Events.AddEventTypes(eventTypes);
                _.Events.InlineProjections.AggregateStreamsWith<TAgreggator>();
            });
            using (var session = store.LightweightSession())
            {
                session.DeleteWhere<TAgreggator>(reading => true);
                session.SaveChanges();
            }
            return store;
        }
        
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Production"].ConnectionString;
        }
    }
}