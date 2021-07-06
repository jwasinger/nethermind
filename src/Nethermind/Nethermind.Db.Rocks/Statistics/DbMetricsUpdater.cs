using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nethermind.Db.Rocks.Config;
using RocksDbSharp;

namespace Nethermind.Db.Rocks.Statistics
{
    internal class DbMetricsUpdater
    {
        private readonly DbOptions _dbOptions;
        private readonly RocksDb _db;
        private readonly IDbConfig _dbConfig;
        private Timer? _timer;

        public DbMetricsUpdater(DbOptions dbOptions, RocksDb db, IDbConfig dbConfig)
        {
            _dbOptions = dbOptions;
            _db = db;
            _dbConfig = dbConfig;
        }

        public void StartUpdating()
        {
            //TODO: Set the interval from the config
            _timer = new Timer(UpdateMetrics, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(600));
        }

        private void UpdateMetrics(object? state)
        {
            var compactionStatsString = _db.GetProperty("rocksdb.stats");
            if (_dbConfig.EnableDbStatistics)
            {
                var dbStatsString = _dbOptions.GetStatisticsString();
            }

            // TODO: Extract stats from the string and update metrics
            Metrics.MetricsDict["testMetric"] = 10;
        }
    }
}
