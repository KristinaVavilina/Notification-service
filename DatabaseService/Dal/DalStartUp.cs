using Dal.Message;
using Dal.Message.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

public static class DalStartUp
{
    public static IServiceCollection AddDal(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MessageDbContext>(opt => opt.UseNpgsql(connectionString));
        services.AddScoped<IMessageRepository, MessageRepository>();
        return services;
    }
}
