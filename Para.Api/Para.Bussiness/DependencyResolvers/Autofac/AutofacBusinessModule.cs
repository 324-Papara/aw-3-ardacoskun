using Autofac;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Para.Bussiness.Cqrs;
using Para.Bussiness.Validations;
using Para.Data.Context;
using Para.Data.DapperRepository;
using Para.Data.UnitOfWork;

public  class AutofacBusinessModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register UnitOfWork with its interface
        builder.RegisterType<UnitOfWork>()
               .As<IUnitOfWork>()
               .InstancePerLifetimeScope();

        // Register all implemented interfaces from the assembly containing CreateCustomerCommand
        builder.RegisterAssemblyTypes(typeof(CreateCustomerCommand).Assembly)
               .AsImplementedInterfaces();

        builder.RegisterType<CustomerRepository>().SingleInstance();

        // Register DbContext with configuration for SQL Server
        builder.Register(c =>
        {
            var configuration = c.Resolve<IConfiguration>();
            var connectionString = configuration.GetConnectionString("MsSqlConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ParaDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new ParaDbContext(optionsBuilder.Options);
        }).AsSelf()
          .InstancePerLifetimeScope();

        // Register all validators implementing IValidator<>
        builder.RegisterAssemblyTypes(typeof(CustomerValidator).Assembly)
               .AssignableTo<IValidator>()
               .AsImplementedInterfaces();
    }


}
