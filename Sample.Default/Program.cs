using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;
using Environment = NHibernate.Cfg.Environment;

class Program
{
    static void Main()
    {
        Configuration nhConfiguration = new Configuration();

        nhConfiguration.SetProperty(Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
        nhConfiguration.SetProperty(Environment.ConnectionDriver, "NHibernate.Driver.Sql2008ClientDriver");
        nhConfiguration.SetProperty(Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
        nhConfiguration.SetProperty(Environment.ConnectionStringName, "NServiceBus/Persistence");

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomNhMappings.Default");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        //busConfiguration.Transactions().DisableDistributedTransactions();

        busConfiguration
            .UsePersistence<NHibernatePersistence>()
            .UseConfiguration(nhConfiguration);

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Parallel.For(0, 1000, i =>
            {
                bus.SendLocal(new StartOrder
                {
                    OrderId = "123"
                });
                bus.SendLocal(new CompleteOrder
                {
                    OrderId = "123"
                });
            });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }

}
