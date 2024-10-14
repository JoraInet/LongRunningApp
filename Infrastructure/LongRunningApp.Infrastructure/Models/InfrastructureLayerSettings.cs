using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongRunningApp.Infrastructure.Models;
public class InfrastructureLayerSettings
{
    public bool UseRedisCache { get; set; }
    public string RedisConnectionString { get; set; }

}
